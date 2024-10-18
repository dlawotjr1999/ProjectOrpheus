using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAttackState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum => EMonsterState.ATTACK;

    private QueenScript Queen { get { return (QueenScript)m_monster; } }

    public EQueenSkillName QueenSkillIdx { get { return Queen.SkillIdx; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Queen == null) { m_monster = _monster; }

        Queen.StartAttack();
    }

    private void SpinQueen()
    {
        Queen.LookTarget();
    }

    public void Proceed()
    {
        if (QueenSkillIdx == EQueenSkillName.CREATE_SKURRABY)
        {
            Queen.LookTarget();
        }
        else if (QueenSkillIdx == EQueenSkillName.SPIT_POISON)
        {
            SpinQueen();
        }
    }
}
