using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ObjectAnimationLink : MonoBehaviour            // �ִϸ��̼ǰ� ��ũ��Ʈ Ÿ�̹��� ���ߴ� �뵵�Դϴ�
{
    private ObjectScript m_object;                          // �ִϸ����Ͱ� ���� ������Ʈ

    // ���� �Լ��� (�ִϸ��̼� �ȿ��� ȣ��)
    public void CreateAttackTiming()                        // ���� ������Ʈ ���� Ÿ�̹�
    {
        m_object.CreateAttack();
    }
    public void AttackDoneTiming()                          // ���� �Ϸ� Ÿ�̹�
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
    public void AttackTriggerOn()                           // ���� Ʈ���� on
    {
        m_object.AttackTriggerOn();
    }

    public void AttackTriggerOff()                          // ���� Ʈ���� off
    {
        m_object.AttackTriggerOff();
    }


    // ���� ����
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
