using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGuardianSkillState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster { get; set; }

    public EMonsterState StateEnum => EMonsterState.SKILL;

    private LifeGuardianScript LifeGuardian { get { return (LifeGuardianScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(LifeGuardian == null) { m_monster = _monster; }

        LifeGuardian.StartSkill();
    }

    public void Proceed()
    {
        if (LifeGuardian.CurSkillIdx == 2 && LifeGuardian.RushStarted)
        {
            LifeGuardian.RushForward();
        }
    }
}
