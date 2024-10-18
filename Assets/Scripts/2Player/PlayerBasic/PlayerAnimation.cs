using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // ���̾� �ε���
    private const int UpperLayerIdx = 1;                                        // ��ü ���̾� ��ȣ

    // �ִϸ��̼� �̸�
    private const string UpperIdleAnimName = "PlayerUpperIdle";                 // ��ü �⺻ �ִϸ��̼�

    // �ؽ� ��
    private int MoveXHash;                                                      // MoveTree X ��
    private int MoveZHash;                                                      // MoveTree Z ��
    private int FallHash;                                                       // ���� ��
    private int LandHash;                                                       // ���� ��
    private int GuardHash;                                                      // ���� ��
    private int ThrowReadyHash;                                                 // ������ �غ� ��
    private int PowerHash;                                                      // ��ų ��


    // �ִϸ��̼� ����
    public bool IsUpperAnimOn { get { return m_anim.GetLayerWeight(UpperLayerIdx) == 1; } }                     //  ��ü ���̾� on
    private bool IsUpperIdleAnim { get {                                                                        // ���� Guard Stop �ִϸ��̼�����
            return FunctionDefine.CheckCurAnimation(m_anim, UpperLayerIdx, UpperIdleAnimName); } }
    private bool IsLandAnimReady { get { return m_anim.GetBool(LandHash); } }                                   // ���� �ִϸ��̼� �غ� ����


    // �⺻ �ִϸ��̼�
    public void SetIdleAnimator() { SetMoveXAnimation(0); SetMoveZAnimation(0); AttackProcing = false; AttackStack = 0; }          // IDLE
    public void UpperAnimStart() { m_anim.SetLayerWeight(UpperLayerIdx, 1); }                                   // ��ü �ִϸ��̼� ����
    public void UpperAnimDone() { m_anim.SetLayerWeight(UpperLayerIdx, 0); }                                    // ��ü �ִϸ��̼� �ߴ�
    public override void HitAnimation()                                                                         // �ǰ�
    {
        if (IsGuarding) { GuardHitAnim(); return;  }
        AttackOffAnim();
        PowerAnimDone();
        base.HitAnimation();
    }
    public void GuardHitAnim() { m_anim.SetTrigger("GUARD_HIT"); }


    // �̵� ����
    public void SetMoveAnimation(Vector2 _move) { SetMoveXAnimation(_move.x); SetMoveZAnimation(_move.y); }     // �̵� ����
    private void SetMoveXAnimation(float _x) { m_anim.SetFloat(MoveXHash, _x); }                                // X�� �̵�
    private void SetMoveZAnimation(float _z) { m_anim.SetFloat(MoveZHash, _z); }                                // z�� �̵�

    // �� �� ������ ����
    public void JumpAnim() { m_anim.SetTrigger("JUMP"); }                                                       // ����
    public void RollAnim() { m_anim.SetTrigger("ROLL"); }                                                       // ������
    public void StartFallAnim() { m_anim.SetBool(FallHash, true); m_anim.SetBool(LandHash, false); }            // ���� ����
    public void StopFallAnim() { m_anim.SetBool(FallHash, false); }                                             // ���� �ߴ�
    public void ReadyLandAnim() { m_anim.SetBool(LandHash, true); }                                             // ����


    // ���� ����
    public void AttackAnim() { m_anim.SetBool("ATTACK", true); }                                                // ���� ����
    public void SetAttackAnim(int _idx) { m_anim.SetInteger("ATTACK_COUNT", _idx); }                            // ���� ����
    public void ResetAnim() { m_anim.SetTrigger("RESET"); }                                                     // ���� �ߴ�
    public void AttackOffAnim() { SetAttackAnim(0); }                                                           // ���� ����

    // ��ų ����
    public void PowerStartAnim() { m_anim.SetBool(PowerHash, true); }                                           // ��ų �غ�
    public void BuffStartAnim() { m_anim.SetBool(PowerHash,true); }                                             // ���� �غ�
    public void PowerFireAnim(int _idx) { m_anim.SetInteger("POWER_IDX", _idx);  m_anim.SetTrigger("POWER_FIRE"); } // ��ų �߻�
    public void PowerAnimDone() { m_anim.SetBool(PowerHash, false); }                                           // ��ų ����

    // ���� ����
    public void GuardAnimStart() { m_anim.SetBool(GuardHash, true); }                                           // ���� ���·� ��ȯ
    public void GuardAnimStop() { m_anim.SetBool(GuardHash, false); }                                           // ���� ������ ��ȯ

    // ȸ�� ����
    public void HealAnimStart() { UpperAnimStart(); m_anim.SetTrigger("HEAL"); }                                // ȸ�� ����
    public void HealAnimDone() { UpperAnimDone(); }                                                             // ȸ�� ��
    public void CancelHealAnim() { HealAnimDone(); }                                                            // ȸ�� �ߴ�

    // ������ ����
    public void ReadyThrowAnim() { UpperAnimStart(); m_anim.SetBool(ThrowReadyHash, true); }                    // ������ �غ�
    public void ThrowAnim() { m_anim.SetTrigger("THROW"); }                                                     // ������
    public void CancelThrowAnim() { UpperAnimDone(); m_anim.SetBool(ThrowReadyHash, false); }                   // ������ ���


    // ��� ����
    private void SetWeaponAnimationLayer(EWeaponType _type)                                                     // ���⺰ ���̾� ����
    {
        m_anim.SetInteger("WEAPON_IDX", (int)_type);
    }
    private void HideWeapon() { CurWeapon.gameObject.SetActive(false); }                                        // ���� �����
    private void ShowWeapon() { CurWeapon.gameObject.SetActive(true); }                                         // ���� ���̱�


    public void RestAnimation()
    {
        m_anim.SetTrigger("REST");
    }



    // �ʱ� ����
    private void SetAnimator()          // �ִϸ����� �ؽ� ����
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
