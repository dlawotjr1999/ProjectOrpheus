using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGaurdState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;

    public EPlayerState StateEnum => EPlayerState.GUARD;

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        m_player.GuardStart();
    }

    private void GuardMove()
    {
        Vector2 inputDir = m_player.InputVector;

        m_player.GroundMove(inputDir, 0.5f);            // 이동
        m_player.SetMoveAnimation(inputDir);            // 애니
        m_player.LookAim();                      // 회전
    }

    public void Proceed()
    {
        if (m_player.LightTrigger)
        {
            m_player.LightChange();
        }

        if (!m_player.GuardPressing)
        {
            m_player.GuardStop();
            return;
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
    }

    public void FixedProceed()
    {
        GuardMove();
    }
}
