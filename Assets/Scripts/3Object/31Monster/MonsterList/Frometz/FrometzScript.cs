using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : RangedAttackMonster
{
    public override void AddIdleState() { m_monsterStates[(int)EMonsterState.IDLE] = gameObject.AddComponent<FrometzIdleState>(); }
    public override void AddApproachState() { m_monsterStates[(int)EMonsterState.APPROACH] = gameObject.AddComponent<FrometzApproachState>(); }

    public override bool CanPurify => PlayerDistance > 0 && PlayerDistance <= PurifyDistance;

    [SerializeField]
    private float m_waterDeep = 0.2f;

    public override Vector3 AttackOffset => new(0, 1, 1.25f);

    private readonly float PurifyDistance = 8;
    private float PlayerDistance { get { return PlayManager.GetDistToPlayer(Position2); } }

    private bool IsLanded { get; set; }

    public override void StartIdle() { }
    public override void StartApproach() { }

    public override void OnSpawned()
    {
        base.OnSpawned();
        m_rigid.useGravity = true;
        IsLanded = false;
        StartCoroutine(WaterCoroutine());
    }
    public void OnTriggerStay(Collider _other)
    {
        if (IsLanded || !_other.CompareTag(ValueDefine.WATER_TAG)) { return; }
        if(Position.y <= _other.transform.position.y - m_waterDeep) { IsLanded = true; }
    }
    private void FrometzLanded()
    {
        m_rigid.useGravity = false;
        m_rigid.velocity = Vector3.zero;
    }

    private IEnumerator WaterCoroutine()
    {
        while (!IsLanded) { yield return null; }
        FrometzLanded();
    }
}
