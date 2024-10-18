using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPositionState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum { get { return EMonsterState.APPROACH; } }

    private WolfScript Wolf { get { return (WolfScript)m_monster; } }

    private float TimeCount { get; set; }
    private readonly float ApproachTime = 4;

    public void ChangeTo(MonsterScript _monster)
    {
        if(Wolf == null) { m_monster = _monster; }

        Wolf.StartPosition();
        TimeCount = Random.Range(2 / Wolf.AttackSpeed, ApproachTime);
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            Wolf.ChangeState(EMonsterState.APPROACH);
            return;
        }
        if (Wolf.CanAttack)
        {
            Wolf.ChangeState(EMonsterState.ATTACK);
            return;
        }
        if (Wolf.PositioningDistance < Wolf.ApproachOffset)
        {
            Wolf.JabWolf();
            return;
        }

        Wolf.WolfPositioning();
    }
}
