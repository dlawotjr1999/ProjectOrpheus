using System.Collections;
using UnityEngine;

public class ExplodeScript : ObjectAttackScript
{
    [SerializeField]
    private EPowerSE m_explodeSound;

    private Vector3 OrigianlPosition { get; set; }
    private Transform ReturnTransform { get; set; }

    public void SetAttack(ObjectScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        if (m_explodeSound != EPowerSE.NONE) { GameManager.PlaySE(m_explodeSound, transform.position); }
        StartCoroutine(LoseDamage(_time));
    }

    private void CheckExplosion(Collider _other)
    {
        IHittable hittable=_other.GetComponentInParent<IHittable>();
        hittable ??=_other.GetComponentInChildren<IHittable>();
        if (hittable == null) { return; }
        if (hittable.IsPlayer) { return; } // 이 부분 삭제하면 전 hittable 공격 가능한 script로 변경
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
    }

    private void OnTriggerEnter(Collider _other)
    {
        CheckExplosion(_other);
    }

    public void SetReturnTransform(Transform _transform)
    {
        ReturnTransform = _transform;
        OrigianlPosition = transform.localPosition;
        transform.SetParent(null);
    }

    private IEnumerator LoseDamage(float _time)
    {
        yield return new WaitForSeconds(_time);
        AttackOff();
    }

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage * Attacker.MagicMultiplier * Attacker.DamageMultiplier, _point, m_impulseAmount, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
    }

    public override void AttackOff()
    {
        base.AttackOff();
        if (ReturnTransform != null)
        {
            transform.SetParent(ReturnTransform);
            transform.localPosition = OrigianlPosition;
            ReturnTransform = null;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Start() { }
}
