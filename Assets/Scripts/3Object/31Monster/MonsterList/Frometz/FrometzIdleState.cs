using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrometzIdleState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.IDLE; } }

    private FrometzScript Frometz { get { return (FrometzScript)m_monster; } }


    private float RotateCount { get; set; }
    private float RotateDir { get; set; } = 0;


    public void ChangeTo(MonsterScript _monster)
    {
        if(m_monster == null) { m_monster = _monster; }

        Frometz.StartIdle();

        RotateCount = Random.Range(2, 5f);
        RotateDir = 0;
    }

    private void FrometzRotate()
    {
        RotateCount -= Time.deltaTime;
        if (RotateCount > 0) {  return; }
        else if(RotateCount > -1f) 
        {
            if (RotateDir == 0)
            {
                RotateDir = Random.Range(-90f, 90f); 
            }
            float angle = Frometz.Direction + RotateDir;
            Frometz.RotateToAngle(angle, ObjectScript.ERotateSpeed.SLOW); 
        }
        else { RotateDir = 0; RotateCount = Random.Range(2, 5f); }
    }

    public void Proceed()
    {
        FrometzRotate();

        m_monster.FindTarget();

        if (m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.ATTACK);
            return;
        }
    }
}
