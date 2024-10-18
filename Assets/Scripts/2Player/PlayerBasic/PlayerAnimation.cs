using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // 레이어 인덱스
    private const int UpperLayerIdx = 1;                                        // 상체 레이어 번호

    // 애니메이션 이름
    private const string UpperIdleAnimName = "PlayerUpperIdle";                 // 상체 기본 애니메이션

    // 해쉬 값
    private int MoveXHash;                                                      // MoveTree X 값
    private int MoveZHash;                                                      // MoveTree Z 값
    private int FallHash;                                                       // 낙하 값
    private int LandHash;                                                       // 착지 값
    private int GuardHash;                                                      // 가드 값
    private int ThrowReadyHash;                                                 // 던지기 준비 값
    private int PowerHash;                                                      // 스킬 값


    // 애니메이션 상태
    public bool IsUpperAnimOn { get { return m_anim.GetLayerWeight(UpperLayerIdx) == 1; } }                     //  상체 레이어 on
    private bool IsUpperIdleAnim { get {                                                                        // 현재 Guard Stop 애니메이션인지
            return FunctionDefine.CheckCurAnimation(m_anim, UpperLayerIdx, UpperIdleAnimName); } }
    private bool IsLandAnimReady { get { return m_anim.GetBool(LandHash); } }                                   // 착지 애니메이션 준비 상태


    // 기본 애니메이션
    public void SetIdleAnimator() { SetMoveXAnimation(0); SetMoveZAnimation(0); AttackProcing = false; AttackStack = 0; }          // IDLE
    public void UpperAnimStart() { m_anim.SetLayerWeight(UpperLayerIdx, 1); }                                   // 상체 애니메이션 시작
    public void UpperAnimDone() { m_anim.SetLayerWeight(UpperLayerIdx, 0); }                                    // 상체 애니메이션 중단
    public override void HitAnimation()                                                                         // 피격
    {
        if (IsGuarding) { GuardHitAnim(); return;  }
        AttackOffAnim();
        PowerAnimDone();
        base.HitAnimation();
    }
    public void GuardHitAnim() { m_anim.SetTrigger("GUARD_HIT"); }


    // 이동 관련
    public void SetMoveAnimation(Vector2 _move) { SetMoveXAnimation(_move.x); SetMoveZAnimation(_move.y); }     // 이동 벡터
    private void SetMoveXAnimation(float _x) { m_anim.SetFloat(MoveXHash, _x); }                                // X축 이동
    private void SetMoveZAnimation(float _z) { m_anim.SetFloat(MoveZHash, _z); }                                // z축 이동

    // 그 외 움직임 관련
    public void JumpAnim() { m_anim.SetTrigger("JUMP"); }                                                       // 점프
    public void RollAnim() { m_anim.SetTrigger("ROLL"); }                                                       // 구르기
    public void StartFallAnim() { m_anim.SetBool(FallHash, true); m_anim.SetBool(LandHash, false); }            // 낙하 시작
    public void StopFallAnim() { m_anim.SetBool(FallHash, false); }                                             // 낙하 중단
    public void ReadyLandAnim() { m_anim.SetBool(LandHash, true); }                                             // 착지


    // 공격 관련
    public void AttackAnim() { m_anim.SetBool("ATTACK", true); }                                                // 공격 시작
    public void SetAttackAnim(int _idx) { m_anim.SetInteger("ATTACK_COUNT", _idx); }                            // 공격 순서
    public void ResetAnim() { m_anim.SetTrigger("RESET"); }                                                     // 공격 중단
    public void AttackOffAnim() { SetAttackAnim(0); }                                                           // 공격 종료

    // 스킬 관련
    public void PowerStartAnim() { m_anim.SetBool(PowerHash, true); }                                           // 스킬 준비
    public void BuffStartAnim() { m_anim.SetBool(PowerHash,true); }                                             // 버프 준비
    public void PowerFireAnim(int _idx) { m_anim.SetInteger("POWER_IDX", _idx);  m_anim.SetTrigger("POWER_FIRE"); } // 스킬 발사
    public void PowerAnimDone() { m_anim.SetBool(PowerHash, false); }                                           // 스킬 종료

    // 가드 관련
    public void GuardAnimStart() { m_anim.SetBool(GuardHash, true); }                                           // 가드 상태로 전환
    public void GuardAnimStop() { m_anim.SetBool(GuardHash, false); }                                           // 가드 해제로 전환

    // 회복 관련
    public void HealAnimStart() { UpperAnimStart(); m_anim.SetTrigger("HEAL"); }                                // 회복 시작
    public void HealAnimDone() { UpperAnimDone(); }                                                             // 회복 끝
    public void CancelHealAnim() { HealAnimDone(); }                                                            // 회복 중단

    // 던지기 관련
    public void ReadyThrowAnim() { UpperAnimStart(); m_anim.SetBool(ThrowReadyHash, true); }                    // 던지기 준비
    public void ThrowAnim() { m_anim.SetTrigger("THROW"); }                                                     // 던지기
    public void CancelThrowAnim() { UpperAnimDone(); m_anim.SetBool(ThrowReadyHash, false); }                   // 던지기 취소


    // 장비 관련
    private void SetWeaponAnimationLayer(EWeaponType _type)                                                     // 무기별 레이어 설정
    {
        m_anim.SetInteger("WEAPON_IDX", (int)_type);
    }
    private void HideWeapon() { CurWeapon.gameObject.SetActive(false); }                                        // 무기 숨기기
    private void ShowWeapon() { CurWeapon.gameObject.SetActive(true); }                                         // 무기 보이기


    public void RestAnimation()
    {
        m_anim.SetTrigger("REST");
    }



    // 초기 설정
    private void SetAnimator()          // 애니메이터 해시 설정
    {
        MoveXHash = Animator.StringToHash("MOVE_X");
        MoveZHash = Animator.StringToHash("MOVE_Z");
        FallHash = Animator.StringToHash("IS_FALLING");
        LandHash = Animator.StringToHash("IS_LANDING");
        GuardHash = Animator.StringToHash("IS_GUARDING");
        ThrowReadyHash = Animator.StringToHash("IS_THROW_READY");
        PowerHash = Animator.StringToHash("POWER_ON");
    }
}
