using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // 이동 관련
    private Vector2 GetMoveVector(Vector2 _dir)                                             // 에임에 맞춰 벡터 회전
    { return FunctionDefine.RotateVector2(_dir, PlayManager.CameraRotation); }
    public override void MoveTo(Vector3 _dir)                                               // 이동을 위한 속도 설정
    {
        Vector3 vel = m_rigid.velocity;
        base.MoveTo(_dir);
        Vector3 vel2 = m_rigid.velocity;
        if (!IsGrounded && !IsOnSlope) { vel2.y = vel.y; m_rigid.velocity = vel2; }
    }
    public void MoveDirection(Vector2 _dir, float _mul)                                     // 방향으로 이동
    {
        Vector2 move = GetMoveVector(_dir) * _mul;
        Vector3 move3 = new(move.x, 0, move.y);
        MoveTo(move3);
    }
    public void MoveDirection(Vector3 _dir, float _mul)                                     // 3D 이동
    {
        Vector3 move3 = _mul * _dir;
        if (!IsGrounded) { move3.y = 0; }
        MoveTo(move3);
    }
    public void GroundMove(Vector2 _dir, float _mul)                                        // 지형에 맞춰 이동
    {
        if (_dir != Vector2.zero)
        {
            Vector2 adjDir = GetMoveVector(_dir);
            TargetAngle = FunctionDefine.VecToDeg(adjDir);
            Vector3 move = new(adjDir.x, PlayerForward.y, adjDir.y);
            MoveDirection(move, _mul);
        }
    }
    public override void StopMove()                                                         // 이동 중지
    {
        m_rigid.velocity = Vector3.zero;
    }


    // 회전 관련
    public void RotateDirection(Vector2 _dir)                                               // 방향으로 회전
    {
        Vector2 move = GetMoveVector(_dir);
        RotateToDir(move, ERotateSpeed.FAST);
    }
    public void LookAim()
    {
        RotateToDir(PlayerAimDirection, ERotateSpeed.FAST);
    }


    // 점프 관련
    private readonly float JumpPower = 10;                                                  // 점프 파워
    private readonly float JumpDelay = 0.25f;                                               // 점프 간격
    public readonly float JumpStaminaUse = 10f;                                             // 점프 스테미나 소모

    public Vector2 JumpRollDirection { get; set; }                                          // 점프 or 구르기 방향
    public bool CanJump { get { return IsGrounded && ((IsOnSlope && CurSurfaceAngle <= m_maxSlopeAngle) || !IsOnSlope)      // 점프 가능 여부
                 && !IsTouchingWall && JumpCooltime <= 0 && JumpPressing && CurStamina >= JumpStaminaUse; } }

    public void StartJump()                                                                 // 점프 상태 시작
    {
        ResetAnim();
        JumpAnim();
        JumpCooltime = -1;
        m_rigid.velocity += JumpPower * Vector3.up;
        UseStamina(JumpStaminaUse);
        if (IsHealing) { CancelHeal(); }
        GuardDelayStart();
    }


    // 구르기 관련
    private readonly float RollDelay = 0.25f;                                               // 구르기 간격
    public readonly float RollMultiplier = 10/7f;                                           // 구르기 / 걷기 속도 배율
    public readonly float RollingTime = 0.85f;                                              // 구르기 진행 시간
    public readonly float RollStaminaUse = 15f;                                             // 구르기 스테미나 소모

    public bool CanRoll { get {                                                             // 구르기 가능 여부
            return (IsGrounded || IsOnSlope) && RollCooltime <= 0 && RollPressing
                && CurStamina >= RollStaminaUse; } }

    public void StartRoll()                                                                 // 구르기 상태 시작
    {
        ResetAnim();
        RollAnim();
        UseStamina(RollStaminaUse);
        if (IsHealing) { CancelHeal(); }
        GuardDelayStart();
    }
    public void RollDone()                                                                  // 구르기 종료
    {
        ActionDone();
        RollCooltime = RollDelay;
    }


    // 낙하 관련
    private readonly float FallVelocity = -1;                                               // 낙하 시작 기준

    public void StartFall()                                                                 // 낙하 상태 시작
    {
        StartFallAnim();
    }


    // 착지 관련
    private readonly float LandVelocity = -15;                                              // 착지 애니메이션 기준
    private readonly float FallDamageVelocity = -21.6f;                                     // 최소 낙하 데미지 기준
    private readonly float FallDeathVelocity = -44;                                         // 낙하 사망 기준

    public void PlayerLand(float _velocity)                                                 // 착지
    {
        StopFallAnim();
        JumpCooltime = JumpDelay;
        if (_velocity <= FallDeathVelocity) { GetRawDamage(MaxHP); }
        else if (_velocity <= FallDamageVelocity) 
        {
            float ratio = 0.125f + ((FallDamageVelocity - _velocity) / (FallDamageVelocity - FallDeathVelocity)) * 7 / 8f;
            float damage = MaxHP * Mathf.Round(ratio * 1000) * 0.001f;
            GetRawDamage(damage);
        }
        else if(_velocity > LandVelocity)
        {
            ChangeState(EPlayerState.IDLE);
        }
    }


    // 기타 이동
    public void ForceMove(Vector2 _dir)                                                     // 방향으로 물리 힘 받음
    {
        transform.position += Time.deltaTime * new Vector3(_dir.x, 0, _dir.y);
    }
    public void TeleportPlayer(Vector3 _pos)                                                // 텔레포트
    {
        transform.position = _pos;
    }
}
