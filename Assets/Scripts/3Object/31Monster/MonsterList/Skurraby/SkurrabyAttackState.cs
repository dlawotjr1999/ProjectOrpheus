using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class SkurrabyAttackState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.ATTACK; } }

    private SkurrabyScript Skurraby { get { return (SkurrabyScript)m_monster; } }

    private const float TimeToFire = 1.7f;
    private const float AfterFireDelay = 3;
    private float FireCount { get; set; }
    private bool HasFired { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        Skurraby.StartAttack();

        FireCount = TimeToFire;
        HasFired = false;
    }

    private void FireBombcrab()
    {
        Skurraby.FireSkurraby();
        FireCount = AfterFireDelay;
        HasFired = true;
    }

    public void Proceed()
    {
        if (!HasFired && FireCount > 0)
        {
            if(!m_monster.HasTarget) { m_monster.ChangeState(EMonsterState.IDLE); return; }

            FireCount -= Time.deltaTime;
            m_monster.LookTarget();
            return;
        }

        FireCount -= Time.deltaTime;
        Skurraby.CheckFlyDone();
        if (!HasFired)
        {
            FireBombcrab();
        }
        else if(FireCount <= 0)
        {
            if (m_monster.HasTarget)
                m_monster.ChangeState(EMonsterState.APPROACH);
            else
                m_monster.ChangeState(EMonsterState.IDLE);
        }
    }
}
