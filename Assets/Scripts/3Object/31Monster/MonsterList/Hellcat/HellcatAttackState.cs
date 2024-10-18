using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellcatAttackState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.ATTACK; } }

    private HellcatScript Hellcat { get { return (HellcatScript)m_monster; } }

    private bool IsJumpAttack { get { return Hellcat.IsJumpAttack; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        m_monster.StartAttack();
    }

    public void Proceed()
    {
        if (IsJumpAttack)
        {
            if (!Hellcat.HasJumped) { m_monster.LookTarget(); }

            Hellcat.JumpAttackMove();
        }
        else
        {
            m_monster.LookTarget();
        }
    }
}
