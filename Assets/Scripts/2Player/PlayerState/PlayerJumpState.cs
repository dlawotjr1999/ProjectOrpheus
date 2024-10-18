using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.JUMP; } }

    private Vector2 JumpDirection { get { return m_player.JumpRollDirection; } set { m_player.JumpRollDirection = value; } }         // 점프 방향
    private const float MoveAdjustRate = 0.33f;         // 방향 조정 비율

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.StartJump();
        if (m_player.IsGuarding) { m_player.GuardStop(); }
        JumpDirection = m_player.InputVector;       // 점프 방향 설정
    }

    private void JumpMove()                         // 기존 점프 방향 + 이후 방향키 입력에 따른 공중 이동
    {
        Vector2 move = JumpDirection;
        if (m_player.InputVector != Vector2.zero)
        {
            move += m_player.InputVector * MoveAdjustRate;
            if (move.magnitude > JumpDirection.magnitude)
                move = JumpDirection;
        }
        m_player.ForceMove(move);
    }

    public void Proceed() { }

    public void FixedProceed()
    {
        JumpMove();
    }
}