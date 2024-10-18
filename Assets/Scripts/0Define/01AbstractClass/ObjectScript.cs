using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract partial class ObjectScript : MonoBehaviour, IHittable
{
    // 위치, 회전
    public Vector3 Velocity { get { return m_rigid.velocity; } }
    public Vector2 Velocity2 { get { return new(m_rigid.velocity.x, m_rigid.velocity.z); } }        // 2차원 속도
    public Vector3 Position { get { return transform.position; } }                                  // 좌표
    public Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }    // 평면 좌표
    public Vector3 Rotation { get { return transform.localEulerAngles; } private set { transform.localEulerAngles = value; } }
    public float Direction                                               // y축 각도
    {
        get { return Rotation.y; }
        protected set { Vector3 rot = Rotation; rot.y = value; Rotation = rot; }
    }

    // 현재 상태
    [SerializeField]
    private float m_curHP;
    [SerializeField]
    private float m_curSpeed;

    public float CurHP { get { return m_curHP; }                        // 현재 HP
        protected set { m_curHP = value; } }
    public virtual float CurSpeed { get { return m_curSpeed; }          // 현재 속도
        protected set { m_curSpeed = value; } }
    public bool IsDead { get; protected set; }                          // 죽음 상태
    public virtual bool IsUnstoppable { get; } = true;                  // 히트 상태 가능 여부


    // 물리 관련
    protected Vector3 m_velocityRef = Vector3.zero;                     // 속도 참조
    private float m_rotationRef;                                        // 각도 참조
    private readonly float GroundCheckerThrashold = 0.1f;               // IsGrounded 확인 범위

    public bool IsGrounded { get; protected set; }                      // 땅 위에 있는지
    protected virtual float DampSpeedUp { get { return 0.2f; } }        // 가속도
    protected virtual float DampSpeedDown { get { return 0.1f; } }      // 감속도

    public virtual void CheckGrounded()                                 // 땅 위에 있는지 확인
    {
        IsGrounded = Physics.CheckSphere(Position, GroundCheckerThrashold, ValueDefine.GROUND_LAYER);
    }


    // 애니메이션
    public virtual void AttackAnimation() { m_anim.SetTrigger("ATTACK"); }
    public virtual void HitAnimation() { m_anim.SetTrigger("HIT"); }
    public virtual void DieAnimation() { m_anim.SetTrigger("DIE"); }


    // UI
    [SerializeField]
    protected float m_uiOffset = 2;                                     // 머리 위 UI 높이


    // 기본 메소드
    public virtual void OnInitiated() { }                               // 생성 시


    // 이동 관련
    public virtual void MoveTo(Vector3 _dir)                            // 방향으로 이동
    {
        float damp = _dir == Vector3.zero ? DampSpeedDown : DampSpeedUp;
        m_rigid.velocity = Vector3.SmoothDamp(m_rigid.velocity, _dir * CurSpeed, ref m_velocityRef, damp);
    }
    public virtual void StopMove()                                      // 움직임 중지
    {
        Vector3 vel = m_rigid.velocity;
        if (vel.magnitude < MoveSpeed + 0.1f) { m_rigid.velocity = new(0, vel.y, 0); }
            
    }
    public virtual void StartTracing() { }
    public virtual void AddForce(Vector3 _dir)
    {
        m_rigid.AddForce(_dir);
    }
    public void SetVelocity(Vector3 _vel)
    {
        m_rigid.velocity = _vel;
    }


    public enum ERotateSpeed { FAST, DEFAULT, SLOW, LAST }
    private readonly float[] SpinSpeed = new float[(int)ERotateSpeed.LAST] { 0.05f, 0.5f, 1.5f };    // 회전 속도(1바퀴 기준 초)


    // 회전 관련
    public void RotateToAngle(float _deg, ERotateSpeed _speed)
    {
        if (Direction == _deg) { return; }
        float gap = FunctionDefine.Abs(Direction - _deg);
        if (gap > 360) { gap -= 360; }
        if (gap >= 180) { gap = 360 - 180; }
        float time = gap / 360 * SpinSpeed[(int)_speed];
        float angle = Mathf.SmoothDampAngle(Direction, _deg, ref m_rotationRef, time);
        Direction = angle;
    }
    public void RotateToDir(Vector2 _dir, ERotateSpeed _speed)
    {
        float deg = FunctionDefine.VecToDeg(_dir);
        RotateToAngle(deg, _speed);
    }


    // 전투 관련
    public virtual void CreateAttack() { }                              // 공격 생성 타이밍
    public virtual void AttackTriggerOn() { if (!AttackObject) return; AttackObject.AttackOn(); }       // 공격 트리거 on
    public virtual void AttackTriggerOff() { if (!AttackObject) return; AttackObject.AttackOff(); }     // 공격 트리거 off
    public virtual void AttackDone() { }                                    // 공격 모션 끝
    public virtual bool GetHit(HitData _hit)                                // 공격 맞음
    {
        if (IsDead) { return false; }
        if (!IsGrounded && _hit.CCList.Contains(ECCType.AIRBORNE)) { return false; }
        float damage = _hit.Damage * (100-Defense) * 0.01f;
        _hit.Damage = damage;
        if (CurHP > damage) { GetCC(_hit); }
        GetDamage(_hit);
#if UNITY_EDITOR
        if (!BattleDebugger.HideHitInfo && damage > 0) { Debug.Log($"{_hit.Attacker.ObjectName} => {ObjectName} {damage} 데미지"); } 
#endif
        if (!IsUnstoppable) { PlayHitAnim(_hit); }
        return true;
    }
    public virtual void GetDamage(HitData _hit)    // 데미지 받음
    {
        float damage = _hit.Damage;
        ObjectScript attacker = _hit.Attacker;
        float hp = CurHP;
        hp -= damage;
        if (attacker != null)
        {
            attacker.GaveDamage(this, damage);
        }
        if (hp <= 0 || CheckVoid(damage)) { hp = 0; SetDead(); }
        if (ExtraHP > 0) { ExtraHP -= damage; }
        if (!IsDead) { PlayHitSound(); }
        SetHP(hp);
    }
    public virtual void GetRawDamage(float _damage)
    {
        GetDamage(new(null, _damage, transform.position));
    }
    public virtual void GaveDamage(ObjectScript _target, float _damage) { }
    public virtual void PlayHitAnim(HitData _hit)                           // 피격 애니메이션 재생
    {
        HitAnimation();
    }
    public virtual void SetHP(float _hp)
    {
        CurHP = _hp;
    }                   // HP 설정
    public virtual void SetDead() { IsDead = true; PlayDieSound(); }        // 죽음 설정
    public virtual void HealObj(float _heal)                                // 회복
    {
        float hp = CurHP + _heal;
        if (hp > MaxHP) { hp = MaxHP; }
        SetHP(hp);
    }


    // CC기
    private CCEffects m_ccEffect;

    protected readonly float[] m_ccCount = new float[(int)ECCType.LAST];    // CC 쿨타임
    private readonly float[] m_ccTime = new float[(int)ECCType.LAST]
    { 0, 10, 10, 3, 0, 0, 10, 5, 10, 10, 10 };

    private readonly float ExtortionDamage = 5;
    private readonly float MelancholyDamage = 2;


    private void CCProc() 
    {
        for (int i = 0; i<(int)ECCType.LAST; i++)
        {
            if (m_ccCount[i] > 0) 
            {
                m_ccCount[i]-=Time.deltaTime;
                if (m_ccCount[i] <= 0)
                {
                    CCDone((ECCType)i);
                }
            }
        } 
    }
    public virtual void GetCC(HitData _hit)                                 // CC 받기
    {
        for (int i = 0; i<_hit.CCList.Length; i++)
        {
            ECCType type = _hit.CCList[i];
            switch (type)
            {
                case ECCType.FATIGUE:           // 피로
                    GetFatigue();
                    break;
                case ECCType.MELANCHOLY:        // 우울
                    GetMelancholy(_hit);
                    break;
                case ECCType.EXTORTION:         // 갈취
                    GetExtortion(_hit);
                    break;
                case ECCType.AIRBORNE:          // 띄움
                    GetAirborne();
                    break;
                case ECCType.KNOCKBACK:         // 밀림
                    GetKnockBack(_hit);
                    break;
                case ECCType.WEAKNESS:          // 나약
                    GetWeakness();
                    break;
                case ECCType.BIND:              // 속박
                    GetBind();
                    break;
                case ECCType.VOID:              // 상실
                    GetVoid();
                    break;
                case ECCType.OBLIVION:          // 망각
                    GetOblivion();
                    break;
                case ECCType.BLIND:             // 실명
                    GetBlind();
                    break;
            }
            m_ccCount[(int)type] = m_ccTime[(int)type];
            SetCCEffect(type, true);
        }
    }
    private void CCDone(ECCType _cc)
    {
        SetCCEffect(_cc, false);
    }
    private void SetCCEffect(ECCType _cc, bool _on)
    {
        m_ccEffect.SetCCEffect(_cc, _on);
    }
    protected void AllCCEffectOff() { m_ccEffect.AllOff(); }
    #region CC기 세부
    //  임시 상태 CC
    public bool IsFatigure { get { return m_ccCount[(int)ECCType.FATIGUE] > 0; } }
    private void GetFatigue()       // 피로
    {
        AdjustInfo slow = new(EAdjType.MOVE_SPEED, ValueDefine.DEFAULT_CC[(int)ECCType.FATIGUE], m_ccTime[(int)ECCType.FATIGUE]);
        GetAdjust(slow);
    }
    private void GetWeakness()      // 나약
    {
        AdjustInfo hp = new(EAdjType.MAX_HP, ValueDefine.DEFAULT_CC[(int)ECCType.WEAKNESS], m_ccTime[(int)ECCType.WEAKNESS]);
        GetAdjust(hp);
    }
    private void GetBind()          // 속박
    {
        AdjustInfo slow = new(EAdjType.MOVE_SPEED, 0, m_ccTime[(int)ECCType.BIND]);
        GetAdjust(slow);
    }

    // 즉발 CC
    private void GetAirborne()
    {
        m_rigid.AddForce(ValueDefine.DEFAULT_CC[(int)ECCType.AIRBORNE] * Vector3.up, ForceMode.VelocityChange);
    }
    public virtual void GetKnockBack(HitData _hit)
    {
        Vector2 flatDir = (Position2 -_hit.Attacker.Position2).normalized;
        Vector3 dir = new(flatDir.x, 0, flatDir.y);
        m_rigid.velocity = dir * ValueDefine.DEFAULT_CC[(int)ECCType.KNOCKBACK];
    }

    // 조건부 CC
    private IEnumerator DamageCoroutine(HitData _hit, ECCType _cc)
    {
        float damage = _cc == ECCType.MELANCHOLY ? MelancholyDamage : ExtortionDamage;
        _hit.Damage = damage;
        ObjectScript attacker = _hit.Attacker;
        yield return null;
        while (!IsDead && m_ccCount[(int)_cc] > 0)
        {
            yield return new WaitForSeconds(1);
            GetDamage(_hit);
            if (_cc == ECCType.EXTORTION) { attacker.HealObj(attacker.MaxHP * 0.1f); }
        }
        m_ccCount[(int)_cc] = 0;
    }

    public bool IsMelancholy { get { return m_ccCount[(int)ECCType.MELANCHOLY] > 0; } }
    public virtual void GetMelancholy(HitData _hit)
    {
        if(IsMelancholy) { return; }
        bool startCor = !IsMelancholy;
        if (startCor) { StartCoroutine(DamageCoroutine(_hit, ECCType.MELANCHOLY)); }
    }
    public bool IsExtorted { get { return m_ccCount[(int)ECCType.EXTORTION] > 0; } }
    private void GetExtortion(HitData _hit)
    {
        bool startCor = !IsExtorted;
        if (startCor) { StartCoroutine(DamageCoroutine(_hit, ECCType.EXTORTION)); }
    }

    private readonly float VoidDamagePercent = 0.25f;
    public virtual bool IsVoid { get { return m_ccCount[(int)ECCType.VOID] > 0; } }
    private float VoidDamageCount { get; set; }
    private void GetVoid()
    {
        VoidDamageCount = 0;
    }
    private bool CheckVoid(float _damage)
    {
        if (!IsVoid) { return false; } 
        VoidDamageCount += _damage; 
        if(VoidDamageCount < MaxHP * VoidDamagePercent) { return false; }
        return true;
    }

    // 플레이어 CC
    public virtual void GetOblivion() { }
    public virtual void GetBlind() { }
    #endregion


    // 업데이트
    public virtual void ProcCooltime()
    {
        BuffNDebuffProc();
        CCProc();
    }

    public virtual void Update()
    {
        ProcCooltime();
    }

    public virtual void FixedUpdate()
    {
        CheckGrounded();
    }
}
