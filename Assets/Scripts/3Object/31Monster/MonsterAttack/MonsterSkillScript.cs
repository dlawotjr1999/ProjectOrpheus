using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillScript : ObjectAttackScript
{
    private Transform ReturnTransform { get; set; }

    public AudioClip HitSound { get { return m_hitSound; } }

    public void SetAttack(ObjectScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        gameObject.SetActive(true);
        if (_time > 0) { StartCoroutine(LoseDamage(_time)); }
    }
    public void SetReturnTransform(Transform _transform)
    {
        ReturnTransform = _transform;
    }

    private IEnumerator LoseDamage(float _time)
    {
        yield return new WaitForSeconds(_time);
        AttackOff();
    }


    public override void AttackOff()
    {
        base.AttackOff();
        if (ReturnTransform != null) { transform.SetParent(ReturnTransform); }
        ReturnTransform = null;
        gameObject.SetActive(false);
    }

    private void OnEnable() { AttackOn(); }
    public override void Start() { }
}
