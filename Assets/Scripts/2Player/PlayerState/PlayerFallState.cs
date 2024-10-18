using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerFallState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.FALL; } }

    private const float MoveAdjustRate = 0.33f;         // 방향 조정 비율

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.StartFall();
    }

    private void FallMove()                         // 기존 점프 방향 + 이후 방향키 입력에 따른 공중 이동
    {
        if (m_player.InputVector != Vector2.zero)
        {
            Vector2 move = m_player.InputVector * MoveAdjustRate;
            m_player.ForceMove(move);
        }
    }

    public void Proceed()
    {
        if (m_player.IsGrounded && m_player.IsDead)
        {
            m_player.ChangeState(EPlayerState.DIE);
        }
    }

    public void FixedProceed()
    {
        if(!m_player.IsGrounded)
            FallMove();
    }
}
