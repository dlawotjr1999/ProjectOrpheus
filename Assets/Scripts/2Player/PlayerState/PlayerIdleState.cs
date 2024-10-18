using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.IDLE; } }

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.StopMove();
        m_player.SetIdleAnimator();
    }

    public void Proceed()
    {
        if (m_player.LightTrigger)
        {
            m_player.LightChange();
        }

        if (m_player.CanRoll)
        {
            m_player.ChangeState(EPlayerState.ROLL);
            return;
        }
        if (m_player.CanJump)
        {
            m_player.ChangeState(EPlayerState.JUMP);
            return;
        }
        if (m_player.CanAttack)
        {
            m_player.ChangeState(EPlayerState.ATTACK);
            return;
        }
        if (m_player.CanUsePower)
        {
            m_player.ChangeState(EPlayerState.POWER);
            return;
        }
        if (m_player.CanGaurd)
        {
            m_player.ChangeState(EPlayerState.GUARD);
            return;
        }
        if (m_player.InputVector != Vector2.zero)
        {
            m_player.ChangeState(EPlayerState.MOVE);
            return;
        }
        if (m_player.CanThrow)
        {
            m_player.ReadyThrow();
            return;
        }
    }

    public void FixedProceed()
    {

    }
}
