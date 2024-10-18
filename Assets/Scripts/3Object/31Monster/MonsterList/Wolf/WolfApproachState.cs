using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfApproachState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.APPROACH; } }

    private WolfScript Wolf { get { return (WolfScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (Wolf == null) { m_monster = _monster; }

        Wolf.StartApproach();
    }

    public void Proceed()
    {
        if (Wolf.CanAttack)
        {
            Wolf.ChangeState(EMonsterState.ATTACK);
            return;
        }
        
        Wolf.ApproachTarget();
    }
}
