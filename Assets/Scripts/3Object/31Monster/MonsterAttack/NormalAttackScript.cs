using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackScript : ObjectAttackScript
{
    private const float NORMAL_ATTACK_TIME = 0.1f;

    public void AttackOn(float _time)
    {
        base.AttackOn();
        gameObject.SetActive(true);
        StartCoroutine(OffDelay(_time));
    }
    public override void AttackOn()
    {
        AttackOn(NORMAL_ATTACK_TIME);
    }
    private IEnumerator OffDelay(float _time)
    {
        yield return new WaitForSeconds(_time);
        AttackOff();
    }
    public override void AttackOff()
    {
        base.AttackOff();
        gameObject.SetActive(false);
    }

    public override void Start()
    {
        m_attacker = GetComponentInParent<ObjectScript>();
    }
}
