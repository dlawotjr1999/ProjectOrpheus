using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.THROW; } }

    private bool IsReady { get; set; }              // 던지기 준비 상태인지

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        IsReady = true;
        m_player.ReadyThrowAnim();
    }

    private void ThrowMove()
    {
        Vector2 inputDir = m_player.InputVector;
        m_player.MoveDirection(inputDir, 0.8f);                 // 이동
        m_player.SetMoveAnimation(inputDir);                    // 애니메이터 설정
        m_player.LookAim();                                     // 회전
    }

    public void Proceed()
    {
        if (IsReady)            // 준비 상태일 때
        {
            if (m_player.AttackTrigger)
            {
                m_player.ThrowItem();
                IsReady = false;
                return;
            }
            if (m_player.GuardTrigger)
            {
                m_player.CancelThrow();
                return;
            }

            Vector3 force = m_player.PlayerAimVector * m_player.ThrowPower;
            PlayManager.DrawThrowLine(force, 0.1f, m_player.ThrowOffset);
        }
    }

    public void FixedProceed()
    {
        if (IsReady)
        {
            ThrowMove();
        }
    }
}
