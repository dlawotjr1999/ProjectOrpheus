using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yum2Script : RangedAttackMonster
{
    public override bool CanPurify => HitPropertyPower;

    private bool HitPropertyPower { get; set; }

    public override void GetPropHit(EPowerProperty _prop)
    {
        if (HitPropertyPower) { return; }
        switch (m_yumType)
        {
            case EProperty.WATER: HitPropertyPower = _prop == EPowerProperty.SOUL; break;
            case EProperty.LIFE: HitPropertyPower = _prop == EPowerProperty.EXPLOSION; break;
            case EProperty.CRYSTAL: HitPropertyPower = _prop == EPowerProperty.SLASH; break;
        }
    }


    [SerializeField]
    private EProperty m_yumType;

    public override Vector3 AttackOffset => new(0, 0.8f, 0.5f);


    public override void OnSpawned()
    {
        base.OnSpawned();
        HitPropertyPower = false;
    }
}
