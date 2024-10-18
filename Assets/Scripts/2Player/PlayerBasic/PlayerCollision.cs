using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void OnTriggerEnter(Collider _other)
    {
        switch (_other.tag)
        {
            case ValueDefine.WATER_TAG:
                PlayerDrowned();
                break;
            case ValueDefine.MONSTER_ATTACK_TAG:            // 몬스터 공격 맞음
                ObjectAttackScript attack = _other.GetComponent<ObjectAttackScript>();
                if(attack == null) { Debug.LogError("공격 스크립트 없음"); return; }
                if(!attack.IsAttacking) { return; }
                if(IsDead || attack.CheckHit(this)) { return; }
                Vector3 point = _other.ClosestPoint(Position);
                HitData hit = new(attack.Attacker, attack.Damage, point, attack.Impulse, attack.CCList);
                GetHit(hit);
                attack.AddHitObject(this);
                break;
            case ValueDefine.REGION_TAG:
                RegionMarker marker = _other.GetComponent<RegionMarker>();
                if(marker == null || CurRegion == marker.Region) { return; }
                EnterRegion(marker.Region);
                break;
        }
    }
}
