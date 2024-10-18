using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.IDLE; } }

    public bool IsMoving { get; private set; }
    private bool IsRotating { get; set; }
    private float TargetRotation { get; set; }
    private float ResetCount { get; set; }


    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }

        m_monster.StartIdle();
        StartMove();
    }

    private void StartMove()
    {
        if (!m_monster.IsSpawned) { return; }
        Vector3 destination = m_monster.SetRandomRoaming();
        m_monster.SetDestination(destination);
        IsMoving = true;
        IsRotating = false;
    }

    private void RandomRotate()
    {
        IsRotating = true;
        float dir = m_monster.Direction;
        TargetRotation = dir + Random.Range(-60f, 60f);
        RotateTo();
    }

    private void RotateTo()
    {
        m_monster.RotateToAngle(TargetRotation, ObjectScript.ERotateSpeed.SLOW);
    }

    private void PauseRoaming()
    {
        m_monster.StopMove();
        IsMoving = false;
        IsRotating = false;
        ResetCount = Random.Range(3f, 10f);
    }

    public void Proceed()
    {
        m_monster.DetectCliff();
        m_monster.FindTarget();

        if (m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.APPROACH);
            return;
        }

        if(!IsMoving) 
        { 
            ResetCount -= Time.deltaTime;
            if(IsRotating) { RotateTo(); }
            if(!IsRotating && ResetCount > 1 && Random.Range(0,1f) < 0.001f) { RandomRotate(); }
        }

        if (IsMoving && m_monster.Arrived)
        {
            PauseRoaming();
        }
        else if (!IsMoving && ResetCount <= 0)
        {
            StartMove();
        }
    }
}