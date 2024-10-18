using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlinkbeakScript : MonsterScript
{
    public override bool CanPurify => EvadeTimeCount >= (EvadeCooltime - PurifyTime);

    [Tooltip("회피 후 성불 기간")]
    [SerializeField]
    private float PurifyTime = 5;
    [SerializeField]
    private AudioClip m_evadeSound;

    private IMonsterState m_evadeState;

    private readonly float EvadeDissolveTime = 0.5f;

    private readonly Color EvadeColor = new(0f, 90/255f, 191/255f, 191/255f);

    private readonly float EvadeSpeed = 8;
    private readonly float EvadeCooltime = 4;
    
    private float EvadeTimeCount { get; set; }
    private Vector3 EvadeTarget { get; set; }

    private void EvadeBlinkbeak() { m_stateManager.ChangeState(m_evadeState); }

    public void StartEvade()
    {
        EvadeTimeCount = EvadeCooltime;
        Vector3 dir = (Position - CurTarget.Position).normalized;
        float dist;
        do
        {
            EvadeTarget = Position + dir * 8 + new Vector3(Random.Range(-8, 8f), 0, Random.Range(-8, 8f));
            dist = Vector3.Distance(EvadeTarget, Position);
        } while (dist < 6 || dist > 10);
        SetDestination(EvadeTarget);
        CurSpeed = EvadeSpeed;
        StartEvadeDissolve();
        GameManager.PlaySE(m_evadeSound, transform.position);
    }

    public void EvadeProceed()
    {
        if (!m_aiPath.reachedDestination) { return; }
        EvadeDone();
    }
    private void EvadeDone()
    {
        StopMove();
        EndEvadeDissolve();
        base.AttackDone();
    }

    private void StartEvadeDissolve()
    {
        HideHPUI();
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            StartCoroutine(EvadeDissolve(smr.materials, true));
        }
    }
    private void EndEvadeDissolve()
    {
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            StartCoroutine(EvadeDissolve(smr.materials, false));
        }
    }
    private IEnumerator EvadeDissolve(Material[] _mats, bool _on)
    {
        if(_on)
        {
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetColor("_Dissolvecolor", EvadeColor);
            }
        }

        float counter = 0;
        
        float change = 1 / EvadeDissolveTime;
        if (!_on) { change *= -1; }
        while ((_on && _mats[0].GetFloat("_DissolveAmount") < 1) || 
            (!_on && _mats[0].GetFloat("_DissolveAmount") > 0))
        {
            counter += Time.deltaTime * change;
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetFloat("_DissolveAmount", counter);
            }
            yield return null;
        }

        if (!_on)
        {
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetColor("_Dissolvecolor", DissolveColor);
                ApplyHPUI();
            }
        }
    }


    public override void LookTarget()
    {
        base.LookTarget();
        if (Vector3.Distance(Position, CurTarget.Position) > AttackRange)
        {
            m_rigid.velocity = MoveSpeed  * 2/ 3f * transform.forward;
        }
    }


    [Tooltip("회피에 필요한 공격 횟수")]
    [SerializeField]
    private int EvadeAttackStack = 2;
    private int AttackStack { get; set; }

    public override void AttackDone()
    {
        AttackStack++;
        if (AttackStack >= EvadeAttackStack && EvadeTimeCount <= 0) 
        {
            EvadeBlinkbeak();
            AttackStack = 0;    
            return;
        }
        base.AttackDone();
    }


    public override void SetStates()
    {
        base.SetStates();
        m_evadeState = gameObject.AddComponent<BlinkbeakEvadeState>();
    }
    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if(EvadeTimeCount > 0) { EvadeTimeCount -= Time.deltaTime; }
    }
}
