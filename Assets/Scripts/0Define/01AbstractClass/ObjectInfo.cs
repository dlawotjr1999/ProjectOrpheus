using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EAdjType
{
    DAMAGE,
    ATTACK,
    MAGIC,
    MOVE_SPEED,
    MAX_HP,
    WEAPON_CC,

    LAST
}

[Serializable]
public struct AdjustInfo
{
    public EAdjType Type;
    public float Amount;
    public float Time;
    public bool IsDistinctive;
    public readonly bool IsNull { get { return Type == EAdjType.LAST; } }
    public static AdjustInfo Null { get { return new(EAdjType.LAST, 0, 0); } }
    public AdjustInfo(EAdjType _type, float _amount, float _time)
    {
        Type = _type; Amount = _amount; Time = _time; IsDistinctive = false;
    }
}

[Serializable]
public class BuffNDebuff
{
    [SerializeField]
    private AdjustInfo m_adjustInfo;
    [SerializeField]
    private float m_timeCount;
    public ObjectAttackScript Attack { get; private set; }
    public AdjustInfo AdjustInfo { get { return m_adjustInfo; } }
    public EAdjType Type { get { return m_adjustInfo.Type; } }
    public float Amount { get { return m_adjustInfo.Amount; } }
    public bool IsAttacked { get { return Attack != null; } }
    public bool IsDistinctive { get { return m_adjustInfo.IsDistinctive; } }
    public float TimeCount { get { return m_timeCount; } private set { m_timeCount = value; } }
    public bool IsDone { get { return TimeCount <= 0; } }
    public void SetTime(float _time) { TimeCount = _time; }
    public void ProcTime() { TimeCount -= Time.deltaTime; }
    public BuffNDebuff(AdjustInfo _info) : this(_info, null) { }
    public BuffNDebuff(AdjustInfo _info, ObjectAttackScript _attack) 
    { m_adjustInfo = _info; TimeCount = _info.Time; Attack = _attack; }
}


[Serializable]
public class ObjectBaseInfo             // 기본 정보
{
    public string ObjectName;           // 이름
    public float AttackSpeed = 1;       // 공격 속도
    public float MoveSpeed = 5;         // 이동 속도
    public void SetInfo(MonsterScriptable _monster)
    {
        ObjectName = _monster.MonsterName;
        AttackSpeed = _monster.AttackSpeed;
        MoveSpeed = _monster.MoveSpeed;
    }
}

[Serializable]
public class ObjectCombatInfo     // 전투 정보
{
    public float MaxHP;             // 최대 HP
    public float Defense;           // 방어력
    public float Attack;            // 물리 공격력
    public virtual void SetInfo(MonsterScriptable _monster)
    {
        MaxHP = _monster.MaxHP;
        Defense = 5;
        Attack = _monster.Attack;
    }
    public virtual float GetStat(ECombatInfoName _name)
    {
        return _name switch
        {
            ECombatInfoName.MAX_HP => MaxHP,
            ECombatInfoName.MAX_STAMINA => -1,
            ECombatInfoName.DEFENSE => Defense,
            ECombatInfoName.ATTACK => Attack,
            ECombatInfoName.MAGIC => -1,
            ECombatInfoName.OVERDRIVE => -1,
            ECombatInfoName.TOLERANCE => -1,
            _ => -1,
        };
    }
}

public abstract partial class ObjectScript
{
    // 기본 컴포넌트
    protected Rigidbody m_rigid;            // Rigidbody
    protected Animator m_anim;              // Animator


    // 기본 정보
    [SerializeField]
    protected ObjectBaseInfo m_baseInfo = new();                // 기본 정보
    public string ObjectName { get { return m_baseInfo.ObjectName; } }                      // 이름
    public virtual float AttackSpeed { get { return m_baseInfo.AttackSpeed; } }             // 공격 속도
    public float MoveSpeed { get { return m_baseInfo.MoveSpeed * MoveSpeedMultiplier; } }   // 이동 속도
    public virtual float ObjectHeight { get { return 2; } }                                 // 오브젝트 높이

    public float MoveSpeedMultiplier { get; protected set; } = 1;                           // 이동 배율

    public virtual void SetMoveMultiplier(float _multiplier) { MoveSpeedMultiplier = _multiplier; }     // 이동 배율 설정


    // 전투 정보
    public virtual ObjectCombatInfo CombatInfo { get; }       // 전투 정보
    public float MaxHP { get { return CombatInfo.MaxHP * MaxHPMultiplier; } }       // 최대 HP
    public virtual float Attack { get { return CombatInfo.Attack; } }               // 물리 공격력
    public float Defense { get { return CombatInfo.Defense; } }                     // 방어력

    public float MaxHPMultiplier { get; protected set; } = 1;

    private float ExtraHP { get; set; } = 0;
    public virtual void SetMaxHPMultiplier(float _multiplier)
    {
        if(_multiplier == 1) { ResetMaxHP(); return; }

        CurHP -= ExtraHP;
        MaxHPMultiplier = _multiplier;
        ExtraHP = MaxHP - CombatInfo.MaxHP;
        CurHP += ExtraHP;

        ApplyHPUI();
    }
    public virtual void ResetMaxHP()
    {
        MaxHPMultiplier = 1;
        if(ExtraHP > 0) { CurHP -= ExtraHP; }

        ExtraHP = 0;
        ApplyHPUI();
    }
    public virtual void ApplyHPUI() { }


    public virtual float DamageMultiplier { get; protected set; } = 1;
    public float AttackMultiplier { get; protected set; } = 1;
    public float MagicMultiplier { get; protected set; } = 1;

    public virtual ObjectAttackScript AttackObject { get; set; }    // 부속 공격 판정


    // 상속 정보
    public virtual bool IsPlayer { get { return false; } }
    public virtual bool IsMonster { get { return false; } }

    [SerializeField]
    private AudioClip[] m_hitSounds;
    protected void PlayHitSound() 
    {
        if(m_hitSounds.Length == 0) { return; }
        GameManager.PlaySE(m_hitSounds[UnityEngine.Random.Range(0, m_hitSounds.Length)], Position);
    }
    [SerializeField]
    private AudioClip[] m_dieSounds;
    protected void PlayDieSound()
    {
        if (m_dieSounds.Length == 0) { return; }
        GameManager.PlaySE(m_dieSounds[UnityEngine.Random.Range(0, m_dieSounds.Length)], Position);
    }


    // 버프 디버프 정보
    [SerializeField]
    protected List<BuffNDebuff> m_buffNDebuff = new();                // 버프, 디버프 리스트

    public virtual void GetAdjust(AdjustInfo _adjust)           // 임시 조정
    {
        GetAdjust(_adjust, null);
    }
    public bool CheckAdjusted(ObjectAttackScript _attack)
    {
        foreach (BuffNDebuff adj in m_buffNDebuff)
        {
            if (adj.IsAttacked && adj.Attack == _attack) { return true; }
        }
        return false;
    }
    public void GetAdjust(AdjustInfo _adjust, ObjectAttackScript _attack)
    {
        BuffNDebuff info = new(_adjust, _attack);

        m_buffNDebuff.Add(info);
        ApplyAdjust(_adjust);
    }
    public void ModifyAdjust(ObjectAttackScript _attack, float _time)
    {
        foreach (BuffNDebuff adj in m_buffNDebuff)
        {
            if (!adj.IsAttacked || adj.Attack != _attack) { continue; }
            adj.SetTime(_time);
        }
    }
    private void ApplyAdjust(AdjustInfo _adjust)
    {
        EAdjType type = _adjust.Type;
        if (type != EAdjType.WEAPON_CC)
        {
            SetMultiplier(type);
        }
        else
        {
            ApplyWeaponCC();
        }
    }
    private void SetMultiplier(EAdjType _type)
    {
        float multiplier = 1;
        float plus = 1;
        float minus = 1;
        foreach (BuffNDebuff adj in m_buffNDebuff)
        {
            if (adj.IsDone) { return; }
            float amount = adj.Amount;
            if (adj.IsDistinctive) { multiplier *= amount; return; }
            if (amount > 1 && amount > plus) { plus = amount; }
            else if (amount < 1 && amount < minus) { minus = amount; }
        }
        multiplier *= plus * minus;
        ApplyMultiplier(_type, multiplier);
        SetAdjustEffect(_type, multiplier);
    }
    private void ApplyMultiplier(EAdjType _type, float _multiplier)       // 배율 설정
    {
        switch (_type)
        {
            case EAdjType.DAMAGE:
                DamageMultiplier = _multiplier;
                break;
            case EAdjType.ATTACK:
                AttackMultiplier = _multiplier;
                break;
            case EAdjType.MAGIC:
                MagicMultiplier = _multiplier;
                break;
            case EAdjType.MOVE_SPEED:
                SetMoveMultiplier(_multiplier);
                break;
            case EAdjType.MAX_HP:
                SetMaxHPMultiplier(_multiplier);
                break;
        }
        if (IsPlayer) { PlayManager.UpdateInfoUI(); }
    }

    private bool[] m_changeCheck = new bool[(int)EAdjType.LAST];
    private void BuffNDebuffProc()                                      // 버프 디버프 쿨타임 적용
    {
        for(int i = 0; i<(int)EAdjType.LAST; i++) { m_changeCheck[i] = false; }
        List<BuffNDebuff> removeList = new();
        foreach (BuffNDebuff adj in m_buffNDebuff)
        {
            EAdjType type = adj.Type;
            adj.ProcTime(); 
            if (adj.IsDone)
            { 
                removeList.Add(adj);
                if (!m_changeCheck[(int)type]) { m_changeCheck[(int)type] =  true; }
            }
        }
        foreach (BuffNDebuff adj in removeList)
        {
            m_buffNDebuff.Remove(adj);
        }
        for(int i = 0; i<(int)EAdjType.LAST; i++) { if (m_changeCheck[i]) { SetMultiplier((EAdjType)i); } }
    }


    private void ApplyWeaponCC() 
    {
        float max = -1;
        ECCType cur = ECCType.NONE;
        foreach (BuffNDebuff adj in m_buffNDebuff)
        {
            if(adj.IsDone || adj.Type != EAdjType.WEAPON_CC) { return; }
            if(adj.TimeCount > max) { max = adj.TimeCount; cur = (ECCType)adj.Amount; }
        }
        SetWeaponCC(cur);
    }
    public virtual void SetWeaponCC(ECCType _cc) { }
    public virtual void SetAdjustEffect(EAdjType _type, float _mul) { }




    // 초기 설정
    public virtual void SetComps()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        m_ccEffect = GetComponentInChildren<CCEffects>();
        SetAttackObject();
    }
    public virtual void SetInfo() { }
    public virtual void SetAttackObject()
    {
        AttackObject = GetComponentInChildren<ObjectAttackScript>();
    }

    public virtual void Awake() { SetComps(); }
    public virtual void Start() { SetHP(MaxHP); CurSpeed = MoveSpeed; }
}
