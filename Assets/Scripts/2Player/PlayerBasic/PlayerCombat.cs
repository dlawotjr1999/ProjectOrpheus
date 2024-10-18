using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    private readonly float ImpulseCooldown = 1f;
    private readonly float GuardImpulseDecreaase = 0.5f;

    private float ImpulseTimeCount { get; set; }

    // 전투 기본
    public override bool IsUnstoppable { get { return false; } }        // 공격 모션 안끊김

    public override bool GetHit(HitData _hit)                           // 공격 맞음
    {
        if (IsInvincible && !_hit.CCList.Contains(ECCType.AIRBORNE)) { return false; }    // 구르는 중이고 에어본이 아니면
        MonsterScript monster = (MonsterScript)_hit.Attacker;
        if (!IsDead && IsGuarding && _hit.Attacker.IsMonster) { monster.HitGuardingPlayer(); }
        if (!base.GetHit(_hit)) { return false; }
        monster.AttackedPlayer(_hit);
        return true;
    }
    public override void PlayHitAnim(HitData _hit)                      // 피격 애니메이션
    {
        if (IsHealing) { CancelHeal(); }
        if (IsPowering) { PowerAnimDone(); CurWeapon.PowerTrailOff(); }
        if (IsAttacking) { AttackOffAnim(); CurWeapon.AttackOff(); }
        if (!_hit.CCList.Contains(ECCType.AIRBORNE) && !_hit.CCList.Contains(ECCType.KNOCKBACK))
        {
            StopMove();
        }
        HitAnimation();
        ChangeState(EPlayerState.HIT);
    }
    public override void SetHP(float _hp)                               // HP 설정
    {
        base.SetHP(_hp);
        PlayManager.SetPlayerCurHP(_hp);
    }
    public override void SetDead()                                      // 죽음 설정
    {
        base.SetDead();
        if (IsHealing) { CancelHeal(); }
        ChangeState(EPlayerState.DIE);
        if (PlayManager.IsPlaying) { StartCoroutine(RestartCoroutine()); }
    }
    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(3);
        PlayManager.PlayerDead();
    }
    public override void GetDamage(HitData _hit)
    {
        base.GetDamage(_hit);
        CurWeapon.PowerTrailOff();

        if (_hit.HasImpulse && ImpulseTimeCount <= 0)
        {
            float impulse = _hit.Impulse * 0.1f;
            if (IsGuarding) { impulse *= GuardImpulseDecreaase; }
            GetImpulse(impulse);
            ImpulseTimeCount = ImpulseCooldown;
            StartCoroutine(ImpulseDelay());
        }
    }
    public void GetImpulse(float _impulse)
    {
        m_impulseSource.GenerateImpulse(_impulse);
    }
    private IEnumerator ImpulseDelay()
    {
        while (ImpulseTimeCount > 0)
        {
            ImpulseTimeCount -= Time.deltaTime;
            yield return null;
        }
    }

    public override void GetKnockBack(HitData _hit)
    {
        if (IsGuarding) { return; }
        base.GetKnockBack(_hit);
    }


    // 무적 관련
    private bool IsInvincible { get; set; }                             // 무적 상태 여부

    public void StartInvincible() { IsInvincible = true; }              // 무적 시작
    public void StopInvincible() { IsInvincible = false; }              // 무적 중단



    // 공격 관련
    public const int MAX_ATTACK = 3;                                    // 공격 연격 수
    private readonly float AttackStaminaUse = 20;                       // 기본공격 스테미나 소모
    private readonly float[,] ForwardDist = new float[3, 3] {           // 무기, 순서 별 전진 속도
        { 1.5f, 1.5f, 2 },
        { 3, 1, 3 },
        { 1.5f, 1.5f, 2 }
    };

    public int AttackStack { get; private set; }                        // 현재 공격 순서 (1, 2, 3)
    private float CurForwardDist
    {
        get
        {                                // 순서에 따른 전진 속도
            return ForwardDist[(int)CurWeapon.WeaponType, AttackStack-1];
        }
    }
    public bool CanAttack
    {
        get
        {                                       // 공격 가능
            return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3;
        }
    }
    public bool AttackCreated { get; private set; }                     // 공격 오브젝트 생성 (다음 공격 가능 기준)
    public bool AttackFinished { get; private set; }                    // 공격 완료 (가드 이행 기준)
    private bool AttackProcing { get; set; }                            // 연격 이어갈지

    public void StartAttack()                                           // 공격 상태 시작
    {
        StopMove();
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackStack = 1;
        SetAttackAnim(AttackStack);
        if (IsHealing) { CancelHeal(); }
        GuardDelayStart();
    }
    public void ToNextAttack()                                          // 다음 공격으로 이행
    {
        if (AttackProcing) { return; }
        AttackStack++;
        SetAttackAnim(AttackStack);
        AttackProcing = true;
        GuardDelayStart();
    }
    public override void AttackTriggerOn()                              // 무기 히트 판정 on
    {
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
#if UNITY_EDITOR
        // PlayManger.AddSoul(1);
        // PlayManager.AddInventoryItem(new(EItemType.PATTERN, UnityEngine.Random.Range(0, (int)EPatternName.LAST)), 1);
        // GetAdjust(new(EAdjType.MAX_HP, 1.1f, 10));
        // PlayManager.AddIngameAlarm("얍얍얍얍얍ㅇ");
#endif
    }
    public override void AttackTriggerOff()                             // 무기 히트 판정 off
    {
        CurWeapon.AttackOff();
        AttackFinished = true;
    }
    public void ChkAttackDone()                                         // 공격 종료 후 연격 or IDLE 판단
    {
        if (!AttackProcing) { AttackDone(); }
        else { AttackProcing = false; AttackFinished = false; }
    }
    public override void AttackDone()                                   // 공격 종료
    {
        AttackOffAnim();
        ChangeState(EPlayerState.IDLE);
    }
    public void BreakAttack()                                           // 공격 중단
    {
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackDone();
        ResetAnim();
    }
    public void AttackForward()                                         // 공격 시 전진
    {
        Vector3 forward = transform.forward * CurForwardDist;
        Vector2 forward2 = new(forward.x, forward.z);
        ForceMove(forward2);
    }

    public void HitTarget()                                             // 몬스터 때림
    {
        if (!IsOverload) { return; }
        OverloadRestoreLight();
    }


    // 권능 관련
    private EPowerName[] PowerSlot { get { return PlayManager.PowerSlot; } }                // 스킬 슬롯
    public int PowerIdx { get {
            for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)          // 눌린 스킬
            { if (PowerTriggers[i]) return i; }
            return -1; } }
    public bool CanUsePower { get {
            return !IsOverload && !IsOblivion && PowerIdx != -1     // 스킬 사용 가능 여부
                && PowerSlot[PowerIdx] != EPowerName.LAST && PowerCooltime[PowerIdx] <= 0; } }
    public EPowerName PowerInHand { get; private set; } = EPowerName.LAST;                  // 사용 중인 스킬
    private PowerInfo PowerInfoInHand { get {                                               // ㄴ의 정보
            if (PowerInHand == EPowerName.LAST) return null;
            return GameManager.GetPowerInfo(PowerInHand); } }
    public int UsingPowerIdx { get; private set; } = -1;                                    // ㄴ의 슬롯 번호

    public bool IsRaycastPower { get { return PowerInHand == EPowerName.RANGED_KNOCKBACK2; } }

    public void ReadyPower()                                                                // 스킬 사용 준비
    {
        UsingPowerIdx = PowerIdx;
        PowerInHand = PowerSlot[UsingPowerIdx];

        PowerStartAnim();
        if (PowerInfoInHand.HideWeapon) { HideWeapon(); }
        if (PowerInfoInHand.ShowCastingEffect) { CastingEffectOn(); GameManager.PlaySE(EPlayerSE.CASTING, transform.position); }

        if (IsHealing) { CancelHeal(); }

        if (PowerInfoInHand.CastType == ECastType.SUMMON)
        {
            ShowPowerAim(PowerInfoInHand.PowerRadius, PowerInfoInHand.PowerCastRange);
        }
        if (IsRaycastPower)
        {
            PlayManager.ShowRaycastAim();
        }
        GuardDelayStart();
    }
    public void FirePower()                                                                 // 스킬 사용
    {
        if(IsRaycastPower && CheckRaycast().IsNull) { CancelPower(); return; }
        if (CurStamina < PowerInfoInHand.PowerData.StaminaCost) { CancelPower(); return; }

        float coolTime = PowerInfoInHand.PowerCooltime;
        SetPowerCooltime(UsingPowerIdx, coolTime);
        PlayManager.UsePowerSlot(UsingPowerIdx, coolTime);
        int idx = PowerInfoInHand.MotionIdx;
        PowerFireAnim(idx);
        UseStamina(PowerInfoInHand.PowerData.StaminaCost);
        HidePowerAim();
    }
    public void CreatePower()                                                               // 스킬 오브젝트 생성
    {
        if(PowerInfoInHand == null) { return; }
        ECastType type = PowerInfoInHand.CastType;

        EPowerSE createSound = PowerInfoInHand.PowerData.CreateSound;
        if(createSound != EPowerSE.NONE) { GameManager.PlaySE(createSound, transform.position); }

        if (type == ECastType.BUFF)
        {
            AdjustInfo adjust = PowerInfoInHand.PowerData.StatAdjust;
            GetAdjust(adjust);
            PowerAnimDone();
            return;
        }

        GameObject power = GameManager.GetPowerObj(PowerInHand);
        power.transform.SetParent(transform);

        if (IsRaycastPower)
        {
            power.transform.SetParent(null);
            PlayerPowerScript raycast= power.GetComponent<PlayerPowerScript>();
            raycast.SetPower(this, Attack, Magic);
            RaycastDone(power);
            PowerAnimDone();
            return;
        }

        switch (type)
        {
            case ECastType.MELEE:
            case ECastType.MELEE_CC:
                power.transform.localPosition = PowerInfoInHand.PowerData.PowerPrefab.transform.localPosition;
                power.transform.localEulerAngles = Vector3.zero;
                MeleePowerScript melee = power.GetComponentInChildren<MeleePowerScript>();
                melee.SetPower(this, Attack, Magic);
                break;
            case ECastType.RANGED:
            case ECastType.RANGED_CC:
                power.transform.localPosition = PowerInfoInHand.PowerData.PowerPrefab.transform.localPosition;
                power.transform.SetParent(null);
                ProjectilePowerScript projectile = power.GetComponentInChildren<ProjectilePowerScript>();
                projectile.SetPower(this, Attack, Magic, PlayerAimDirection);
                break;
            case ECastType.SUMMON:
                power.transform.SetParent(null);
                power.transform.position = PlayManager.TracePowerAim(Position, PowerInfoInHand.PowerCastRange);
                PlayerPowerScript summon = power.GetComponentInChildren<PlayerPowerScript>();
                summon.SetPower(this, Attack, Magic);
                break;
            case ECastType.AROUND:
            case ECastType.AROUND_CC:
                power.transform.localPosition = PowerInfoInHand.PowerData.PowerPrefab.transform.localPosition;
                power.transform.SetParent(null);
                AroundPowerScript around = power.GetComponentInChildren<AroundPowerScript>();
                around.SetPower(this, Attack, Magic);
                break;
            default:
                PlayerPowerScript script = power.GetComponentInChildren<PlayerPowerScript>();
                script.SetPower(this, Attack, Magic);
                break;
        };

        PowerAnimDone();
    }
    public void CancelPower()                                                               // 스킬 취소
    {
        PowerAnimDone();
    }
    public void PowerDone()                                                                 // 스킬 사용 종료
    {
        if (PowerInHand == EPowerName.LAST) { return; }
        HidePowerAim();
        if (PowerInfoInHand.ShowCastingEffect)
        {
            CastingEffectOff();
        }
        PowerInHand = EPowerName.LAST;
        UsingPowerIdx = -1;
        ShowWeapon();
        ChangeState(EPlayerState.IDLE);
    }

    public void PowerTrailOn()
    {
        if (PowerInfoInHand == null) { return; }
        
        EPowerTrailType type = PowerInfoInHand.PowerData.PowerTrail;
        if (type < EPowerTrailType.HAND1) { CurWeapon.PowerTrailOn(type); }
        else { SetHandTrail(type, true); }
    }
    public void PowerTrailOff()
    {
        EPowerTrailType type = PowerInfoInHand.PowerData.PowerTrail;
        if (type < EPowerTrailType.HAND1) { CurWeapon.PowerTrailOff(); }
        else { SetHandTrail(type, false); }
    }


    public struct RaycastTargetInfo
    {
        public GameObject Target;
        public Vector3 Point;
        public bool IsNull { get { return Target == null; } }
        public static RaycastTargetInfo Null { get { return new(null, Vector3.zero); } }
        public RaycastTargetInfo(GameObject _target, Vector3 _point) { Target = _target; Point = _point; }
    }

    private MonsterScript RaycastTarget { get; set; }
    public RaycastTargetInfo CheckRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 50, ValueDefine.HITTABLE_LAYER, QueryTriggerInteraction.Collide);
        List<RaycastHit> hitList = hits.ToList();
        hitList.Sort((elm1, elm2) => (Vector3.Distance(Position, elm1.point) < Vector3.Distance(Position, elm2.point) ? -1 : 1));

        foreach (RaycastHit hit in hitList)
        {
            if (!hit.collider.isTrigger) { continue; }
            MonsterScript monster = hit.collider.GetComponentInParent<MonsterScript>();
            if (monster == null) { continue; }
            RaycastTarget = monster;
            PlayManager.SetRaycastAimState(true);
            Debug.Log(monster);
            return new(hit.collider.gameObject, hit.point);
        }
        PlayManager.SetRaycastAimState(false);
        return RaycastTargetInfo.Null;
    }
    private void RaycastDone(GameObject _power)
    {
        RaycastTargetInfo info = CheckRaycast();
        if (info.IsNull) { return; }
        _power.transform.position = info.Point;
        _power.GetComponent<EffectScript>().SetDestroyTime(2); ;
        RaycastTarget.GetInstantHit(PowerInfoInHand, info.Target, this);
    }

    // 버프 디버프 관련
    public override void SetAdjustEffect(EAdjType _type, float _mul) 
    {
        if(_type == EAdjType.WEAPON_CC) { return; }

        if (_type == EAdjType.MAX_HP) { SetBuffEffect(EBuffType.MAX_HP, _mul != 1); }
        else { SetBuffEffect(EBuffType.STATUS, _mul != 1); }
    }


    // 가드 관련
    private readonly float GuardDelay = 0.8f;

    public bool CanGaurd { get { return GuardPressing && GuardCooltime <= 0; } }
    public void GuardStart()                                                                // 가드 시작
    {

    }
    public void GuardStop()                                                                 // 가드 중단
    {

    }
    public void GuardDone()
    {
        ActionDone();
        GuardDelayStart();
    }
    public void GuardDelayStart()
    {
        GuardCooltime = GuardDelay;
    }


    // 회복 관련
    private readonly float HealDelay = 5;                                                                           // 회복 딜레이

    private bool CanHeal
    {
        get
        {
            return HealInHand != EPatternName.LAST && HealItemTrigger && !IsHealing &&         // 회복 가능
                (IsIdle || IsMoving) && HealCooltime <= 0;
        }
    }
    public bool IsHealing { get; private set; }                                                                     // 회복 중    
    private EPatternName HealInHand { get { return PlayManager.CurHealPattern; } }                                  // 장착된 회복 아이템
    private float HealAmountInHand
    {
        get
        {
            if (HealInHand == EPatternName.LAST) return -1;                          // ㄴ의 회복량
            ItemInfo info = GameManager.GetItemInfo(new SItem(EItemType.PATTERN, (int)HealInHand));
            return ((PatternScriptable)info.ItemData).HealAmount;
        }
    }

    public void HealUpdate()                                                                                        // 회복 여부 확인
    {
        if (CanHeal)
        {
            HealAnimStart();
            HealCooltime = HealDelay;
            IsHealing = true;
        }
    }
    public void UseHealItem()
    {
        if (!IsHealing) { return; }
        HealObj(HealAmountInHand);
        PlayManager.UseHealPattern();
        CreateHealEffect();
        GameManager.PlaySE(EPlayerSE.HEAL, transform.position);
    }
    private void CreateHealEffect()
    {
        GameObject heal = GameManager.GetEffectObj(EEffectName.HEAL);
        heal.transform.SetParent(transform);
        heal.transform.localPosition = Vector3.zero;
    }
    public void CancelHeal()
    {
        CancelHealAnim();
        IsHealing = false;
    }
    public void HealDone()                                                                                          // 회복 완료(애니메이션)
    {
        HealAnimDone();
        IsHealing = false;
    }


    // 던지기 관련
    public readonly int ThrowPower = 60;                                                    // 던지기 힘
    private readonly float ThrowDelay = 1.5f;                                               // 던지기 딜레이

    private readonly Vector3 TempThrowOffset = new(0.371f, 1.628f, 0.664f);                 // 임시 던지기 생성 오프셋
    private readonly Vector3 TempThrowRotation = new(-64.449f, -30.487f, 40.266f);          // 임시 던지기 오브젝트 회전

    public bool CanThrow { get { return IsUpperIdleAnim && ThrowCooltime <= 0 && ThrowItemTrigger; } }     // 투척 가능 여부
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;                  // 던지기 준비 중인 아이템 Enum
    private GameObject InHandPrefab { get; set; }                                           // 손에 든 아이템 오브젝트
    public Vector3 ThrowOffset
    {
        get
        {
            Vector3 pos = transform.position;
            Vector2 dir = PlayerAimDirection;
            Vector2 right = new(dir.y, -dir.x);
            Vector2 offset = dir*TempThrowOffset.z + right*TempThrowOffset.x;
            pos += new Vector3(offset.x, TempThrowOffset.y, offset.y);
            return pos;
        }
    }                                                          // 던지기 오프셋

    public void ReadyThrow()                                                                // 던지기 준비
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { return; }

        HideWeapon();
        SetThrowItem(item);
        ChangeState(EPlayerState.THROW);
    }
    public void SetThrowItem(EThrowItemName _item)                                          // 아이템 들기
    {
        ItemInHand = _item;

        GameObject item = GameManager.GetThorwItemPrefab(_item);
        item.transform.SetParent(m_throwItemTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = TempThrowRotation;
        InHandPrefab = item;
        Destroy(InHandPrefab.GetComponent<CapsuleCollider>());
        Destroy(InHandPrefab.GetComponent<Rigidbody>());
        Destroy(InHandPrefab.GetComponent<ThrowItemScript>());
    }
    public void CancelThrow()                                                               // 던지기 취소
    {
        PlayManager.HideThrowLine();
        DestroyInHand();
        DoneThrow();
    }
    private void DestroyInHand()
    {
        if (InHandPrefab == null) { return; }
        Destroy(InHandPrefab);
    }
    public void ThrowItem()                                                                 // 아이템 던지기
    {
        PlayManager.HideThrowLine();
        ThrowAnim();
    }
    public void CreateThrowItem()                                                           // 던질 아이템 생성
    {
        Vector3 force = PlayerAimVector * ThrowPower;
        GameObject item = GameManager.GetThorwItemPrefab(ItemInHand);
        item.transform.SetParent(transform);
        item.transform.localPosition = TempThrowOffset;
        item.transform.localEulerAngles = TempThrowOffset;
        item.transform.SetParent(null);

        ThrowItemScript script = item.GetComponent<ThrowItemScript>();
        script.SetFlying(force);
        DestroyInHand();
        PlayManager.UseThrowItem();

        ThrowCooltime = ThrowDelay;
    }
    public void DoneThrow()                                                                 // 던지기 완료
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ActionDone();
    }


    // CC 관련
    private IEnumerator OblivionCoroutine;
    public bool IsOblivion { get { return m_ccCount[(int)ECCType.OBLIVION] > 0; } }
    public override void GetOblivion()
    {
        // 스킬 사용 불가 표시
        if (OblivionCoroutine == null) { OblivionCoroutine = ResetOblivion(); StartCoroutine(OblivionCoroutine); }
    }
    private IEnumerator ResetOblivion()
    {
        while (!IsDead && IsOblivion)
        {
            yield return null;
        }
        // 스킬 사용 가능 표시
    }

    private IEnumerator BlindCoroutine;
    public override void GetBlind()
    {
        PlayManager.ShowBlindMark();
        if (BlindCoroutine == null) { BlindCoroutine = ResetBlind(); StartCoroutine(BlindCoroutine); }
    }
    private IEnumerator ResetBlind()
    {
        while (!IsDead && m_ccCount[(int)ECCType.BLIND] > 0)
        {
            yield return null;
        }
        PlayManager.HideBlindMark();
    }
}
