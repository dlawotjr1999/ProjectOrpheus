using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ObjectAnimationLink : MonoBehaviour            // 애니메이션과 스크립트 타이밍을 맞추는 용도입니다
{
    private ObjectScript m_object;                          // 애니메이터가 속한 오브젝트

    // 실행 함수들 (애니메이션 안에서 호출)
    public void CreateAttackTiming()                        // 공격 오브젝트 생성 타이밍
    {
        m_object.CreateAttack();
    }
    public void AttackDoneTiming()                          // 공격 완료 타이밍
    {
        m_object.AttackDone();
    }

    public void StopMoveTiming()
    {
        m_object.StopMove();
    }

    public void StartTracing()
    {
        m_object.StartTracing();
    }
    public void AttackTriggerOn()                           // 공격 트리거 on
    {
        m_object.AttackTriggerOn();
    }

    public void AttackTriggerOff()                          // 공격 트리거 off
    {
        m_object.AttackTriggerOff();
    }


    // 몬스터 전용
    public void SkillOnTiming()
    {
        ((MonsterScript)m_object).SkillOn();
    }
    public void CreateSkillTiming()
    {
        ((MonsterScript)m_object).CreateSkill();
    }
    public void SkillOffTiming()
    {
        ((MonsterScript)m_object).SkillOff();
    }
    public void SkillDoneTiming()
    {
        ((MonsterScript)m_object).SkillDone();
    }




    private void Awake()
    {
        m_object = GetComponentInParent<ObjectScript>();
    }
}
