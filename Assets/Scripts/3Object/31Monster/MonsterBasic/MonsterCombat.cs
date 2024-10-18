using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public abstract partial class MonsterScript
{
    // 전투 매니저
    private MonsterBattler m_battleManager;
    public bool AgainstMonster { get { return m_battleManager.AgainstMonster; } set { m_battleManager.AgainstMonster = value; } }
    private void SetBattleTarget(ObjectScript _obj)
    {
        if (_obj == CurTarget) { return; }
        if (_obj.IsPlayer) { CurTarget = _obj; }
        else if (_obj.IsMonster && AgainstMonster) { CurTarget = _obj; }
    }
    private bool CheckMonsterBattle(HitData _hit)
    {
        if (_hit.Attacker.IsPlayer) { return false; }
        MonsterScript monster = (MonsterScript)_hit.Attacker;
        if (!AgainstMonster && !monster.AgainstMonster) { return false; }
        AgainstMonster = true;
        return false;
    }


    // 기본 전투
    public override void SetHP(float _hp)
    {
        base.SetHP(_hp);
        ApplyHPUI();
    }                   // HP 설정
    public override bool GetHit(HitData _hit)    // 맞음
    {
        if (CheckMonsterBattle(_hit)) { return false; }
        SetBattleTarget(_hit.Attacker);
        SetDeathType(_hit.Attacker);
        CheckPowerProp(_hit.Property);
        if (!base.GetHit(_hit)) { return false; }
        return true;
    }
    public override void PlayHitAnim(HitData _hit)
    {
        base.PlayHitAnim(_hit);
        ChangeState(EMonsterState.HIT);
    }
    public override void SetDead()
    {
        base.SetDead();
        ChangeState(EMonsterState.DIE);
    }
    public void GetInstantHit(PowerInfo _skill, GameObject _part, ObjectScript _attacker)
    {
        GetDamage(new(_attacker, 100, _part.transform.position));
    }
    public virtual void AttackedPlayer(HitData _hit) { }

    private void CheckPowerProp(EPowerProperty _prop)
    {
        if (_prop == EPowerProperty.LAST) { return; }
        GetPropHit(_prop);
    }
    public virtual void GetPropHit(EPowerProperty _prop) { }


    // 공격 관련
    [SerializeField]
    protected GameObject[] m_normalAttacks;                                      // 기본 공격 프리펍
    [SerializeField]
    protected float[] m_normalDamageMultiplier;
    [SerializeField]
    protected AudioClip[] m_attackSounds; 
    protected bool IsCorrectNormalnum(int _num) { return m_normalDamageMultiplier.Length == _num; }
    public virtual bool CheckNormalCount() { return true; }



    protected int AttackIdx { get; set; }
    protected float NormalDamage(int _idx) { return Attack * m_normalDamageMultiplier[_idx]; }

    public virtual bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }      // 공격 가능 여부
    public float AttackTimeCount { get; set; } = 0;                                                                 // 공격 쿨타임

    public virtual void SetAttackCooltime()
    {
        AttackTimeCount = AttackSpeed;
    }
    public override void AttackTriggerOn()
    {
        AttackTriggerOn(0);
    }
    public virtual void AttackTriggerOn(int _idx)
    {
        m_normalAttacks[_idx].SetActive(true);

        AttackObject = m_normalAttacks[_idx].GetComponent<ObjectAttackScript>();
        AttackObject.SetAttack(this, Attack);

        PlayAttackSound(_idx);
        base.AttackTriggerOn();
    }
    protected void PlayAttackSound(int _idx)
    {
        if (m_attackSounds.Length <= _idx) { return; }
        GameManager.PlaySE(m_attackSounds[_idx], transform.position);
    }
    public override void AttackTriggerOff()
    {
        base.AttackTriggerOff();
    }
    public override void CreateAttack()
    {
        AttackTriggerOn();
    }

    public override void AttackDone()
    {
        SetAttackCooltime();
        if (CurTarget != null)
            ChangeState(EMonsterState.APPROACH);
        else
            ChangeState(EMonsterState.IDLE);
    }


    // 스킬
    public int SkillNum { get { return m_skillManager.SkillNum; } }
    public float[] SkillCooltime { get { return m_skillManager.SkillCooltimes; } }
    public float[] SkillTimeCount { get { return m_skillManager.SkillTimeCount; } }
    public virtual bool CanSkill => HasSkill && HasTarget && m_skillManager.CanSkill;
    public bool AnySkillTimeCheck { get { return m_skillManager.AnySkillTimeCheck; } }
    public bool SkillTimeCheck { get { return m_skillManager.SkillTimeCheck; } }
    public ObjectAttackScript[] SkillList { get { return m_skillManager.SkillList; } }
    public float[] SkillDamages { get { return m_skillManager.SkillDamages; } }


    public ObjectAttackScript CurSkill { get; protected set; }
    public int CurSkillIdx { get { return m_skillManager.CurSkillIdx; } }
    public int NextSkillIdx { get { return m_skillManager.NextSkillIdx; } }
    public bool CreatedSkill { get; protected set; }

    public virtual void StartSkill()
    {
        m_skillManager.StartSkill();
        if(CurSkillIdx == -1) { return; }
        if(SkillNum > 1) { m_anim.SetInteger("SKILL_IDX", CurSkillIdx); }
        m_anim.SetBool("IS_SKILLING", true);
        StopMove();
        CreatedSkill = false;
    }
    public virtual void SkillOn() { CreatedSkill = true; }
    public virtual void SkillOff() { m_anim.SetBool("IS_SKILLING", false); }
    public virtual void CreateSkill() { CreatedSkill = true; m_anim.SetBool("IS_SKILLING", false); }
    public virtual void SkillDone()
    {
        m_skillManager.SkillUsed(CurSkillIdx);
        if (CurTarget != null)
            ChangeState(EMonsterState.APPROACH);
        else
            ChangeState(EMonsterState.IDLE);
    }



    // 전투 대상 관련
    public readonly float MissTargetDelay = 5f;

    public ObjectScript CurTarget { get; protected set; }                                               // 현재 타겟
    public bool HasTarget { get { return (CurTarget != null && !CurTarget.IsDead); } }                  // 타겟을 가지고 있는지
    public float TargetDistance { get { if (HasTarget) return Vector3.Distance(Position, CurTarget.Position); return -1; } }    // 목표와의 거리
    public bool TargetInAttackRange { get { return TargetDistance <= AttackRange; } }                   // 공격범위 내인지

    public void FindTarget()            // 타겟 탐색 (타겟 X)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, ViewRange);

        if (targets.Length == 0) return;
        foreach (Collider col in targets)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();         // 일단 플레이어만 체크
            if (player == null) { continue; }
            Vector3 targetPos = col.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            Vector3 look = FunctionDefine.AngleToDir(Direction);
            float targetAngle = Mathf.Acos(Vector3.Dot(look, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(transform.position, targetDir, ViewRange))
            {
                CurTarget = player;
            }
        }
    }
    public bool CheckTarget()           // 타겟 확인 (타겟 O)
    {
        if (CurTarget == null) { return false; }
        if (Vector3.Distance(CurTarget.Position, Position) > ReturnRange) { return false; }
        return true;
    }
    public void MissTarget()            // 타겟 잃기
    {
        CurTarget = null;
    }

    public virtual bool HasPath { get; set; } = true;
    public virtual void ApproachTarget()            // 타겟에게 접근
    {
        if (!HasTarget) { HasPath = false; return; }
        Vector2 dir = (CurTarget.Position2 - Position2);
        if (AttackTimeCount > 0 && TargetInAttackRange) { RotateToDir(dir, ERotateSpeed.SLOW); HasPath = false; return; }

        if (AttackTimeCount > 0 && dir.magnitude < FunctionDefine.Max(0.5f, AttackRange - 2)) { m_aiPath.destination = transform.position - 0.1f * new Vector3(dir.x, 0, dir.y); HasPath = true; }
        else if (dir.magnitude < AttackRange - 0.5f) { StopMove(); HasPath = false; }
        else { m_aiPath.destination = CurTarget.Position; HasPath = true; }
    }


    // 피격 관련
    public readonly float StunDelay = 1f;                       // 피격 시 경직
    private readonly float HitGuardEndTime = 1.5f;              // 가드 중인 플레이어 타격 후 피격 애니메이션 재생 기간

    private float HitGuardCooltime { get; set; }

    private bool HitGuarding { get; set; }                                                              // 플레이어 가드 중 때림
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // 공격 모션 캔슬 불가인지

    public void HitGuardingPlayer()
    {
        HitGuarding = true;
        if (HitGuardCooltime > 0) { HitGuardCooltime = HitGuardEndTime; return; }
        else
        {
            HitGuardCooltime = HitGuardEndTime;
            StartCoroutine(HitGuardEnd());
        }
    }
    private IEnumerator HitGuardEnd()
    {
        while (HitGuardCooltime > 0)
        {
            HitGuardCooltime -= Time.deltaTime;
            yield return null;
        }
        HitGuarding = false;
    }


    private readonly float InfectRadius = 1.5f;
    public bool IsMelancholySource { get; private set; }
    public override void GetMelancholy(HitData _hit)
    {
        IsMelancholySource = true;
        StartCoroutine(InfectMelancholy());
        base.GetMelancholy(_hit);
    }
    public void GetMelancholy()
    {
        base.GetMelancholy(HitData.Null);
    }
    private IEnumerator InfectMelancholy()
    {
        yield return new WaitForSeconds(0.1f);
        while (!IsDead && IsMelancholy)
        {
            FindInfectTarget();
            yield return new WaitForSeconds(0.1f);
        }
        IsMelancholySource = false;
    }
    private void FindInfectTarget()
    {
        Collider[] cols = Physics.OverlapSphere(Position, InfectRadius, ValueDefine.HITTABLE_LAYER);
        List<MonsterScript> targets = new();
        foreach (Collider col in cols)
        {
            if (col.CompareTag(ValueDefine.MONSTER_TAG)) { continue; }
            MonsterScript monster = col.GetComponent<MonsterScript>();
            if (monster == null || monster.IsDead || monster.IsMelancholy || targets.Contains(monster)) { continue; }
            targets.Add(monster);
        }
        foreach (MonsterScript monster in targets) { monster.GetMelancholy(); }
    }
}
