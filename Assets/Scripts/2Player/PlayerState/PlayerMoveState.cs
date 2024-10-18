using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.MOVE; } }

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputDir = m_player.InputVector;

        m_player.GroundMove(inputDir, m_player.MoveSpeedMultiplier);        // 이동
        m_player.SetMoveAnimation(inputDir);                                // 애니
        m_player.LookAim();                                                 // 회전
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

        Vector2 inputDir = m_player.InputVector;
        if (inputDir == Vector2.zero)
        {
            m_player.ChangeState(EPlayerState.IDLE);
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
        MovePlayer();
    }
}