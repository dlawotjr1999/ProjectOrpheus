using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class WeaponInfo
{
    public EWeaponType WeaponType;
    public EHitType HitType;
    public string WeaponName;
    public FRange Attack;
    public FRange Magic;
    public float AttackSpeed;
    public string Description;
    public void SetInfo(WeaponScriptable _scriptable)
    {
        WeaponType = ItemManager.IDToWeaponType(_scriptable.ID);
        if(WeaponType == EWeaponType.SCEPTER) { HitType = EHitType.BLOW; }
        else { HitType = EHitType.SLASH; }
        WeaponName = _scriptable.ItemName;
        Attack = _scriptable.Attack;
        Magic = _scriptable.Magic;
        AttackSpeed = _scriptable.AttackSpeed;
        Description = _scriptable.Description;
    }
}

public class WeaponScript : AnimateAttackScript
{
    [SerializeField]
    private WeaponInfo m_weaponInfo = new();
    public EWeaponType WeaponType { get { return m_weaponInfo.WeaponType; } }
    public EHitType HitType { get { return m_weaponInfo.HitType; } }
    public string WeaponName { get { return m_weaponInfo.WeaponName; } }
    public FRange WeaponAttack { get { return m_weaponInfo.Attack; } }
    public FRange WeaponMagic { get { return m_weaponInfo.Magic; } }
    public float WeaponAttackSpeed { get { return m_weaponInfo.AttackSpeed; } }
    public string WeaponDescription { get { return m_weaponInfo.Description; } }
    public void SetWeaponInfo(WeaponScriptable _scriptable) { m_weaponInfo.SetInfo(_scriptable); }


    [SerializeField]
    private WeaponScriptable m_scriptable;      // 정보
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public EWeaponName WeaponEnum { get { return (EWeaponName)m_scriptable.Idx; } }

    private PlayerController Player { get { return (PlayerController)m_attacker; } }

    public override float Damage => Player.Attack * Player.AttackMultiplier * Player.DamageMultiplier;

    [SerializeField]
    private AudioClip m_swingSound;


    public override void CreateHitEffect(IHittable _hittable, Vector3 _pos)
    {
        if (_hittable.IsMonster && _pos != Vector3.zero)
        {
            EEffectName effectName = HitType == EHitType.SLASH ? EEffectName.HIT_SLASH : EEffectName.HIT_BLOW;
            GameObject effect = GameManager.GetEffectObj(effectName);
            effect.transform.position = _pos;

            PlayWeaponHitSound(_pos);
        }
    }

    private void PlayWeaponHitSound(Vector3 _pos)
    {
        if (HitType == EHitType.SLASH)
        {
            GameManager.PlaySE(EPlayerSE.SLSAH, _pos);
        }
        else if (HitType == EHitType.BLOW)
        {
            GameManager.PlaySE(EPlayerSE.BLOW, _pos);
        }
    }


    public void SetScriptable(WeaponScriptable _scriptable)
    { 
        m_scriptable = _scriptable; 
        SetInfo();
    }
    private void SetInfo()
    {
        m_weaponInfo.SetInfo(m_scriptable);
    }


    private WeaponTrailEffect m_trailEffect;
    public override void AttackOn() { base.AttackOn(); m_trailEffect.SetNormalTrail(true); GameManager.PlaySE(m_swingSound, transform.position); }
    public override void AttackOff() { base.AttackOff(); m_trailEffect.SetNormalTrail(false); }
    public void PowerTrailOn(EPowerTrailType _type) { if (!Player.IsAttacking && !Player.IsPowering) { return; } m_trailEffect.PowerTrailOn(_type); }
    public void PowerTrailOff() { m_trailEffect.PowerTrailOff(); }


    private WeaponBuffEffectScript m_buffEffect;
    public void BuffEffectOn(ECCType _cc) { m_buffEffect.EffectOn(_cc); }
    public void BuffEffectOff() { m_buffEffect.EffectOff(); }

    public override void SetComps()
    {
        base.SetComps();
        m_buffEffect = GetComponentInChildren<WeaponBuffEffectScript>();
        m_buffEffect.InitWeapon(WeaponType);
        m_trailEffect = GetComponentInChildren<WeaponTrailEffect>();
        m_trailEffect.SetComps();
    }
}
