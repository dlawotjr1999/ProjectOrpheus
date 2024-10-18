using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGuardianSkillState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster { get; set; }

    public EMonsterState StateEnum => EMonsterState.SKILL;

    private CrystalGuardianScript CrystalGuardian { get { return (CrystalGuardianScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (CrystalGuardian == null) { m_monster = _monster; }

        CrystalGuardian.StartSkill();
    }

    public void Proceed()
    {
        if (CrystalGuardian.CurSkillIdx != 1)
        {
            CrystalGuardian.LookTarget();
        }
    }
}
