using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfJabState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.ATTACK; } }

    private WolfScript Wolf { get { return (WolfScript)m_monster; } }

    public bool IsMoving { get; private set; }
    private float TimeCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (Wolf == null) { m_monster = _monster; }

        Wolf.StartJab();
        if (Wolf.CurRole != EWolfRole.MAIN)
        {
            TimeCount = Random.Range(0.5f, 2);
        }
        else
        {
            TimeCount = Random.Range(2, 4);
        }
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount < 0 && Wolf.PositioningDistance > Wolf.MaxJabOffset)
        {
            Wolf.PositionWolf();
            IsMoving = true;
            return;
        }
        IsMoving = false;

        if (Wolf.CurRole == EWolfRole.MAIN)
        {
            if (Wolf.TargetRotGap > 60)
            {
                Wolf.ChangeState(EMonsterState.APPROACH);
                Wolf.ResetRole();
                return;
            }
        }
        else
        {
            if (TimeCount < 0)
            {
                Wolf.ChangeState(EMonsterState.APPROACH);
                return;
            }
        }

        Wolf.LookTarget();
    }
}
