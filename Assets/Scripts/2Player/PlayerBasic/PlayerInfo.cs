using Cinemachine;
using System;
using UnityEngine;

public enum ECooltimeName                               // 쿨타임 종류
{
    JUMP,
    ROLL,
    GUARD,
    THROW,
    HEAL,
    POWER1,

    LAST = POWER1 + ValueDefine.MAX_POWER_SLOT
}

public struct SPlayerWeaponInfo                         // 무기 정보
{
    public FRange WeaponAttack;
    public FRange WeaponMagic;
    public float WeaponAttackSpeed;
    public SPlayerWeaponInfo(WeaponScript _weapon) { WeaponAttack = _weapon.WeaponAttack; WeaponMagic = _weapon.WeaponMagic; WeaponAttackSpeed = _weapon.WeaponAttackSpeed; }
}


[Serializable]
public class PlayerStatInfo                             // 플레이어 스탯 정보
{
    public float m_health;          // 체력
    public float m_endure;          // 지구력
    public float m_strength;        // 근력
    public float m_intellect;       // 지력
    public float m_rapid;           // 민첩   
    public float m_mental;          // 정신

    public void SetStat(EStatName _name, float _num)      // 스탯 설정
    {
        switch (_name)
        {
            case EStatName.HEALTH: m_health = _num; break;
            case EStatName.ENDURE: m_endure = _num; break;
            case EStatName.STRENGTH: m_strength = _num; break;
            case EStatName.INTELLECT: m_intellect = _num; break;
            case EStatName.RAPID: m_rapid = _num; break;
            case EStatName.MENTAL: m_mental = _num; break;
        }
    }
    public void UpgradeStat(EStatName _name, float _up)
    {
        switch (_name)
        {
            case EStatName.HEALTH: m_health += _up; break;
            case EStatName.ENDURE: m_endure += _up; break;
            case EStatName.STRENGTH: m_strength += _up; break;
            case EStatName.INTELLECT: m_intellect += _up; break;
            case EStatName.RAPID: m_rapid += _up; break;
            case EStatName.MENTAL: m_mental += _up; break;
        }
    }
    public float GetStat(EStatName _name)
    {
        return _name switch
        {
            EStatName.HEALTH => m_health,
            EStatName.ENDURE => m_endure,
            EStatName.STRENGTH => m_strength,
            EStatName.INTELLECT => m_intellect,
            EStatName.RAPID => m_rapid,
            EStatName.MENTAL => m_mental,
            _ => -1,
        };
    }
    public PlayerStatInfo() { for(int i = 0; i<(int)EStatName.LAST; i++) { SetStat((EStatName)i, ValueDefine.INITIAL_STAT); } }
    public PlayerStatInfo(PlayerStatInfo _other)
    {
        m_health = _other.m_health;
        m_endure = _other.m_endure;
        m_strength = _other.m_strength;
        m_intellect = _other.m_intellect;
        m_rapid = _other.m_rapid;
        m_mental = _other.m_mental;
    }
    public PlayerStatInfo(PlayerStatInfo _info, int[] _point)
    {
        m_health = _info.m_health + _point[0];
        m_endure = _info.m_endure + _point[1];
        m_strength = _info.m_strength + _point[2];
        m_intellect = _info.m_intellect + _point[3];
        m_rapid = _info.m_rapid + _point[4];
        m_mental = _info.m_mental + _point[5];
    }
}

[Serializable]
public class PlayerCombatInfo : ObjectCombatInfo        // 플레이어 전투 정보
{
    public float MaxStamina;        // 최대 스테미나
    public float Magic;             // 마법 공격력
    public float Overdrive;         // 과열 데미지
    public float Tolerance;         // 내성

    public void SetCombatInfo(PlayerStatInfo _stat)
    {
        MaxHP = FunctionDefine.RoundF1(28.85f + 15 * Mathf.Sqrt(1.6f*_stat.m_health + 0.65f * _stat.m_strength));
        MaxStamina = FunctionDefine.RoundF1(64.6f + (1.5f * _stat.m_endure + _stat.m_health + 0.8f * _stat.m_intellect + 0.24f * _stat.m_rapid));
        Defense = FunctionDefine.RoundF3((0.5f * (_stat.m_endure + 0.3f * _stat.m_mental) - 6.5f)* 0.01f);
        Attack = FunctionDefine.RoundF1(6 * Mathf.Sqrt(_stat.m_strength) - 7);
        Magic = FunctionDefine.RoundF1(6 * Mathf.Sqrt(_stat.m_intellect) - 7);
        Overdrive = FunctionDefine.RoundF3(0.73f + (15 * Mathf.Sqrt(_stat.m_rapid) * 0.01f));
        Tolerance = FunctionDefine.RoundF3(1.2f * _stat.m_mental * 0.01f - 0.12f);
    }
    public override float GetStat(ECombatInfoName _name)
    {
        return _name switch
        {
            ECombatInfoName.MAX_HP => MaxHP,
            ECombatInfoName.MAX_STAMINA => MaxStamina,
            ECombatInfoName.DEFENSE => Defense,
            ECombatInfoName.ATTACK => Attack,
            ECombatInfoName.MAGIC => Magic,
            ECombatInfoName.OVERDRIVE => Overdrive,
            ECombatInfoName.TOLERANCE => Tolerance,
            _ => -1,
        };
    }
    public PlayerCombatInfo(PlayerStatInfo _stat)
    {
        SetCombatInfo(_stat);
    }
}

public partial class PlayerController
{
    // 최초 스폰 위치
    private readonly Vector3 InitialPos = new(496.01f, -0.644f, 244.86f);

    // 데이터
    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { m_statInfo = new(); InitRegion(EOasisName.LAST); return; }

        SaveData data = PlayManager.CurSaveData;

        Vector3 spawnPos = data.OasisPoint == EOasisName.LAST ? InitialPos
            : PlayManager.OasisList[(int)data.OasisPoint].RespawnPoint;

        transform.position = spawnPos;
        transform.localEulerAngles = data.PlayerRot;

        m_statInfo = new(data.StatInfo);

        InitRegion(data.OasisPoint);
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.PlayerRot = transform.localEulerAngles;

        data.StatInfo = new(m_statInfo);
    }


    // 기본 컴포넌트
    private CapsuleCollider m_collider;
    [SerializeField]
    private Transform m_cameraFocus;
    public Transform CameraFocus { get { return m_cameraFocus; } }


    // 기초 정보
    public override float ObjectHeight { get { return m_collider.height; } }        // 오브젝트 높이
    private float HalfHeight { get { return ObjectHeight / 2; } }

    public ERegion CurRegion { get; private set; } = ERegion.LAST;
    private void InitRegion(EOasisName _oasis)
    {
        ERegion region = EnvironmentManager.Oasis2Region(_oasis);
        if(region == ERegion.LAST) { return; }
        EnterRegion(region);
    }
    public void EnterRegion(ERegion _region)
    {
        GameManager.PlaySE(EEnvironmentSE.REGION_ENTER);
        CurRegion = _region;
        PlayManager.ShowEnterRegion(_region);
    }


    // 스탯 정보
    [SerializeField]
    protected PlayerStatInfo m_statInfo;    // 스탯 정보
    public float Health { get { return m_statInfo.m_health; } }                     // 체력
    public float Endure { get { return m_statInfo.m_endure; } }                     // 지구력
    public float Strength { get { return m_statInfo.m_strength; } }                 // 근력
    public float Intellect { get { return m_statInfo.m_intellect; } }               // 지력
    public float Rapid { get { return m_statInfo.m_rapid; } }                       // 민첩
    public float Mental { get { return m_statInfo.m_mental; } }                     // 정신
    public PlayerStatInfo GetStatInfo() { return m_statInfo; }
    public void ResetStatInfo() { m_statInfo = new(); ApplyStat(); }
    public void SetStat(EStatName _name, float _num) { m_statInfo.SetStat(_name, _num); ApplyStat(); }      // 스탯 설정


    // 전투 정보
    [SerializeField]
    private PlayerCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }
    public override float Attack => base.Attack + WeaponAttack.Num;                 // 공격력
    public float MaxStamina { get { return m_combatInfo.MaxStamina; } }             // 최대 스테미나
    public override float AttackSpeed => base.AttackSpeed * WeaponAttackSpeed;      // 공격 속도
    public float Magic { get { return m_combatInfo.Magic + WeaponMagic.Num; } }     // 주술 공격력
    public float Tolerance { get { return m_combatInfo.Tolerance; } }               // 내성
    public float Defence { get { return m_combatInfo.Defense; } }                   // 방어력
    public float Overdrive { get { return m_combatInfo.Overdrive; } }               // 증폭
    public void ApplyStat() { m_combatInfo.SetCombatInfo(m_statInfo); }            // 스탯 적용

    public override float DamageMultiplier => base.DamageMultiplier * Overdrive;


    // 장비 관련
    [SerializeField]
    private WeaponScript[] m_weapons;
    [SerializeField]
    private Transform m_throwItemTransform;

    public override ObjectAttackScript AttackObject { get { return CurWeapon; } }
    private WeaponScript CurWeapon { get; set; }
    public SPlayerWeaponInfo CurWeaponInfo { get { return new(CurWeapon); } }
    public FRange WeaponAttack { get { return CurWeaponInfo.WeaponAttack; } }
    public FRange WeaponMagic { get { return CurWeaponInfo.WeaponMagic; } }
    public float WeaponAttackSpeed { get { return CurWeaponInfo.WeaponAttackSpeed; } }

    private void InitWeapon()
    {
        EWeaponName name = PlayManager.CurWeapon;
        SetCurWeapon(name);
    }
    public void SetCurWeapon(EWeaponName _weapon)
    {
        foreach (WeaponScript weapon in m_weapons)
        {
            if (weapon.WeaponEnum == _weapon) { CurWeapon = weapon; weapon.gameObject.SetActive(true); }
            else if (weapon.gameObject.activeSelf) { weapon.gameObject.SetActive(false); }
        }
        SetWeaponAnimationLayer(CurWeapon.WeaponType);
        SetWeaponName();
    }
    private void SetWeaponName()
    {
        string name;
        switch (CurWeapon.WeaponType)
        {
            case EWeaponType.BLADE: name = "Blade"; break;
            case EWeaponType.SWORD: name = "Sword"; break;
            case EWeaponType.SCEPTER: name = "Scepter"; break;
            default: name = "Weapon"; break;
        }
        CurWeapon.gameObject.name = name;
    }
    public override void SetWeaponCC(ECCType _cc)
    {
        CurWeapon.SetCCType(_cc);
        if (_cc == ECCType.NONE) { CurWeapon.BuffEffectOff(); }
        else { CurWeapon.BuffEffectOn(_cc); }
    }



    private PowerCastingEffect m_castingEffect;
    private void CastingEffectOn() { m_castingEffect.ShowEffect(); }
    private void CastingEffectOff() { m_castingEffect.HideEffect(); }

    private BuffEffectList m_buffEffect;
    public void SetBuffEffect(EBuffType _type, bool _on) { m_buffEffect.SetEffect(_type, _on); }



    private PowerTrailEffectScript[] m_handTrailEffect;
    public void SetHandTrail(EPowerTrailType _type, bool _on)
    {
        int idx = _type - EPowerTrailType.HAND1;
        if(idx >= m_handTrailEffect.Length) { Debug.LogError("이상한 권능 입력"); return; }
        if (_on) { m_handTrailEffect[idx].EffectOn(); }
        else { m_handTrailEffect[idx].EffectOff(); }
    }



    // 상속 정보
    public override bool IsPlayer { get { return true; } }


    // 초기 설정
    public override void SetComps()
    {
        base.SetComps();
        m_collider = GetComponentInChildren<CapsuleCollider>();
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
        SetImpulseInfo();
        FunctionDefine.SetFriction(m_collider, FloorFriction, true);
        m_castingEffect = GetComponentInChildren<PowerCastingEffect>();
        m_castingEffect.SetComps();
        m_buffEffect = GetComponentInChildren<BuffEffectList>();
        m_buffEffect.SetComps();
        m_handTrailEffect = GetComponentsInChildren<PowerTrailEffectScript>();
    }
    private void SetStates()                // 상태들 추가
    {
        m_stateManager = new(this);
        m_playerStates[(int)EPlayerState.IDLE] = gameObject.AddComponent<PlayerIdleState>();
        m_playerStates[(int)EPlayerState.MOVE] = gameObject.AddComponent<PlayerMoveState>();
        m_playerStates[(int)EPlayerState.JUMP] = gameObject.AddComponent<PlayerJumpState>();
        m_playerStates[(int)EPlayerState.FALL] = gameObject.AddComponent<PlayerFallState>();
        m_playerStates[(int)EPlayerState.ATTACK] = gameObject.AddComponent<PlayerAttackState>();
        m_playerStates[(int)EPlayerState.GUARD] = gameObject.AddComponent<PlayerGaurdState>();
        m_playerStates[(int)EPlayerState.POWER] = gameObject.AddComponent<PlayerPowerState>();
        m_playerStates[(int)EPlayerState.ROLL] = gameObject.AddComponent<PlayerRollState>();
        m_playerStates[(int)EPlayerState.THROW] = gameObject.AddComponent<PlayerThrowState>();
        m_playerStates[(int)EPlayerState.HIT] = gameObject.AddComponent<PlayerHitState>();
        m_playerStates[(int)EPlayerState.DIE] = gameObject.AddComponent<PlayerDieState>();
    }
    public override void SetInfo()
    {
        m_combatInfo = new PlayerCombatInfo(m_statInfo);
        ApplyStat();
    }

    public override void Start()
    {
        LoadData();
        SetStates();
        SetAnimator();
        SetInfo();
        base.Start();
        PlayManager.SetCurPlayer(this);     // PlayerManger에 플레이어 등록
        ChangeState(EPlayerState.IDLE);     // Idle로 상태 전이
        PlayManager.SetPlayerMaxHP(MaxHP);  // 체력바 설정
        InitStamina();                      // 스테미나 설정
        InitLight();                        // 능력 초기 설정
        InitWeapon();                       // 무기 설정
        HidePowerAim();                     // 스킬 에임 끄기
    }
}