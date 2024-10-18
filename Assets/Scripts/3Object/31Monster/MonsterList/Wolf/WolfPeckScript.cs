using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EWolfRole
{
    MAIN,
    LEFT_PUNCH,
    RIGHT_PUNCH,
    LEFT_JAB,
    RIGHT_JAB,

    LAST
}

public class WolfPeckScript : MonoBehaviour
{
    [SerializeField]
    private List<WolfScript> m_wolfs = new();
    public Vector3 PeckCenter { get {
            Vector3 center = Vector3.zero;
            foreach (WolfScript wolf in m_wolfs)
                center += wolf.Position;
            return center / m_wolfs.Count; } }

    [SerializeField]
    private AudioClip m_engageSound;


    public bool Engaging { get; private set; }

    public int CurPurifyTurn { get; private set; }

    public void EngageWolfs(WolfScript _wolf, ObjectScript _target) // 늑대 전투 개시
    {
        Engaging = true;
        GameManager.PlaySE(m_engageSound, _wolf.transform.position);
        ResetRole();
        for (int i = 0; i<m_wolfs.Count; i++)
        {
            if (m_wolfs[i] == _wolf) { continue; }
            m_wolfs[i].SetAttackTarget(_target);
        }
        CurPurifyTurn = 0;
    }


    public void WolfDead(WolfScript _wolf)
    {
        if(_wolf.PeckIdx != CurPurifyTurn) { CurPurifyTurn = -1; }
        else { CurPurifyTurn++; }
        m_wolfs.Remove(_wolf);
        if(m_wolfs.Count == 0) { Destroy(gameObject); return; }
        ResetRole();
    }

    public void AddWolf(WolfScript _wolf)
    {
        _wolf.SetPeck(this, m_wolfs.Count);
        m_wolfs.Add(_wolf);
        _wolf.transform.SetParent(transform);
        ResetRole();
    }


    public void DisengageWolfs()                // 늑대 전투 해제
    {
        foreach(WolfScript wolf in m_wolfs) 
        {
            if (wolf.IsIdle) { continue; }
            wolf.ChangeState(EMonsterState.IDLE);
        }
        Engaging = false;
    }

    public void ReleaseWolfs()
    {
        foreach(WolfScript wolf in m_wolfs) { wolf.transform.SetParent(null); }
        m_wolfs.Clear();
        Destroy(gameObject);
    }

    public void ResetRole()
    {
        m_wolfs.Sort((wolf1, wolf2) => { return wolf1.TargetDistance < wolf2.TargetDistance ? -1 : 1; });
        for (int i = 0; i<m_wolfs.Count; i++)
        {
            m_wolfs[i].SetRole((EWolfRole)i);
            m_wolfs[i].PositionWolf();
        }
    }

    private void InitPeck()
    {
        if (m_wolfs.Count >= (int)EWolfRole.LAST)
        {
            List<WolfScript> leavingWolfs = new();
            leavingWolfs.AddRange(m_wolfs.GetRange((int)EWolfRole.LAST, m_wolfs.Count - (int)EWolfRole.LAST));
        }
        for (int i=0;i<m_wolfs.Count;i++)
        {
            WolfScript wolf = m_wolfs[i];
            wolf.SetPeck(this, i);
        }
    }

    private void Start()
    {
        InitPeck();
    }
}
