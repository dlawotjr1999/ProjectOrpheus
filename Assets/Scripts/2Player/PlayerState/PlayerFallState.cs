using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerFallState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.FALL; } }

    private const float MoveAdjustRate = 0.33f;         // ���� ���� ����

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.StartFall();
    }

    private void FallMove()                         // ���� ���� ���� + ���� ����Ű �Է¿� ���� ���� �̵�
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
