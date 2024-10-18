using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGuardianSkillState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster { get; set; }

    public EMonsterState StateEnum => EMonsterState.SKILL;

    private WaterGuardianScript WaterGuardian { get { return (WaterGuardianScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (WaterGuardian == null) { m_monster = _monster; }

        WaterGuardian.StartSkill();
    }

    public void Proceed()
    {
        if (!WaterGuardian.CreatedSkill) { WaterGuardian.LookTarget(); }
    }
}
