using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WolfScript : MonsterScript
{
    public override void AddApproachState() { m_monsterStates[(int)EMonsterState.APPROACH] = gameObject.AddComponent<WolfApproachState>(); }
    public override void AddAttackState() { m_monsterStates[(int)EMonsterState.ATTACK] = gameObject.AddComponent<WolfAttackState>(); }

    public override bool CanPurify => m_peck != null ? m_peck.CurPurifyTurn == PeckIdx : false;

    private WolfPeckScript m_peck;
    public void SetPeck(WolfPeckScript _peck, int _idx) { m_peck = _peck; PeckIdx = _idx; }

    public override bool IsMoving => base.IsMoving || IsPositioning;

    private bool IsPositioning { get { return CurState.GetType() == typeof(WolfJabState) && ((WolfJabState)CurState).IsMoving; } }


    public int PeckIdx { get; private set; } = -1;
    public EWolfRole CurRole { get; private set; } = EWolfRole.MAIN;
    public void SetRole(EWolfRole _role) { CurRole = _role; }
    public void ResetRole() { if (m_peck == null) { return; } m_peck.ResetRole(); }

    private void JabAnimation() { m_anim.SetTrigger("JAB"); }

    public override void UpdateAnimation()
    {
        base.UpdateAnimation();
        m_anim.SetBool("IN_COMBAT", InCombat);
    }

    public override bool HasPath => true;

    // 늑대 스테이트
    private IMonsterState m_positionState, m_jabState;
    public void JabWolf()
    {
        m_stateManager.ChangeState(m_jabState);
    }
    public void PositionWolf()
    {
        m_stateManager.ChangeState(m_positionState);
    }


    // 늑대 수치
    public float JabDistance { get; set; } = 5;
    public float ApproachOffset { get; private set; } = 0.5f;
    public readonly float MaxJabOffset = 1;


    // 늑대 속성
    public float PositioningDistance { get { return Vector3.Distance(Position, PositionTarget); } }
    public Vector3 PositionTarget { get {
            if(CurTarget == null) { return Vector3.positiveInfinity; }
            Vector3 target = CurTarget.Position;
            Vector2 aim2;
            if (CurTarget.IsPlayer) { aim2 = PlayManager.PlayerAimDirection; }
            else { aim2 = FunctionDefine.DegToVec(CurTarget.Direction); }
            Vector3 aim = new(aim2.x, 0, aim2.y);
            switch (CurRole)
            {
                case EWolfRole.MAIN:
                    return target + JabDistance * aim;
                case EWolfRole.RIGHT_PUNCH:
                    Vector2 right = FunctionDefine.RotateVector2(aim2, 120);
                    Vector3 offset1 = new(right.x, 0, right.y);
                    return target + JabDistance * offset1;
                case EWolfRole.LEFT_PUNCH:
                    Vector2 left = FunctionDefine.RotateVector2(aim2, 240);
                    Vector3 offset2 = new(left.x, 0, left.y);
                    return target + JabDistance * offset2;
                case EWolfRole.RIGHT_JAB:
                    right = FunctionDefine.RotateVector2(aim2, 60);
                    offset1 = new(right.x, 0, right.y);
                    return target + JabDistance * offset1;
                case EWolfRole.LEFT_JAB:
                    left = FunctionDefine.RotateVector2(aim2, 300);
                    offset2 = new(left.x, 0, left.y);
                    return target + JabDistance * offset2;
                default:
                    return target;
            }
        }
    }
    public float TargetRotGap { get
        {
            if(CurTarget == null) { return 0; }
            float target;
            if (CurTarget.IsPlayer) { target = FunctionDefine.VecToDeg(PlayManager.PlayerAimDirection); }
            else { target = CurTarget.Direction; }
            Vector2 dir = (Position2 - CurTarget.Position2);
            float deg = FunctionDefine.VecToDeg(dir);
            float gap = target-deg;
            if (gap > 180) { gap -= 180; }
            return Mathf.Abs(gap);
        } }

    private readonly float NearPeckDistance = 15;

    private bool IsFarFromPeck { get { if (m_peck == null) { return false; } return Vector3.Distance(Position, m_peck.PeckCenter) > NearPeckDistance; } }
    public override Vector3 SetRandomRoaming()
    {
        Vector3 destination = m_peck.PeckCenter;
        for (int i = 0; i<32; i++)
        {
            destination = base.SetRandomRoaming();
            if(!IsFarFromPeck && (Vector3.Distance(Position, m_peck.PeckCenter) > Vector3.Distance(destination, m_peck.PeckCenter))) { break; }
        }
        return destination;
    }


    public override void StartIdle()
    {
        base.StartIdle();
        m_peck.DisengageWolfs();
    }
    public override void DespawnMonster()
    {
        m_peck.ReleaseWolfs();
        base.DespawnMonster();
    }


    // 늑대 기본 메소드
    public void StartPosition()
    {
        JabDistance = Random.Range(4.5f, 5.5f);
        m_aiPath.maxSpeed = ApproachSpeed;

    }
    public void StartJab()
    {
        JabAnimation();
        StopMove();
    }
    public override void StartApproach()
    {
        CurSpeed = ApproachSpeed;
        ApproachOffset = Random.Range(0.25f, 0.5f);
        if (m_peck != null && !m_peck.Engaging)
        {
            m_peck.EngageWolfs(this, CurTarget);
        }
    }

    public override void CreateAttack()
    {
        base.CreateAttack();
        StopMove();
    }


    // 늑대 세부 메소드
    public void SetAttackTarget(ObjectScript _target)
    {
        CurTarget = _target;
    }

    public override void ApproachTarget()            // 타겟에게 접근
    {
        if (AttackTimeCount > 0) { base.ApproachTarget(); return; }
        Vector2 approach2 = CurTarget.Position2;
        if (AttackTimeCount > 0 && TargetInAttackRange)
        {
            Vector2 dir = (approach2 - Position2);
            RotateToDir(dir, ERotateSpeed.FAST);
            return; 
        }
        m_aiPath.destination = CurTarget.Position;
    }
    public void WolfPositioning()
    {
        Vector2 approach2 = new(PositionTarget.x, PositionTarget.z);
        if (AttackTimeCount > 0 && TargetInAttackRange) { Vector2 dir = (approach2 - Position2); RotateToDir(dir, ERotateSpeed.FAST); return; }
        m_aiPath.destination = PositionTarget;
    }
    
    public void AttackMove()
    {
        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        m_rigid.velocity = ApproachSpeed * new Vector3(dir.x, 0, dir.y);
        LookTarget();
    }
    public override void AttackDone()
    {
        if (CurTarget != null)
            PositionWolf();
        else
            ChangeState(EMonsterState.IDLE);
        SetAttackCooltime();
    }

    public override void SetDead()
    {
        base.SetDead();
        if(m_peck != null) { m_peck.WolfDead(this); }
    }




    public override void StartDissolve()
    {
        base.StartDissolve();
        VisualEffect[] effects = GetComponentsInChildren<VisualEffect>();
        foreach(VisualEffect effect in effects) { effect.Stop(); }
    }



    // 늑대 초기 설정
    public override void SetStates()
    {
        base.SetStates();
        m_positionState = gameObject.AddComponent<WolfPositionState>();
        m_jabState = gameObject.AddComponent<WolfJabState>();
    }
}
