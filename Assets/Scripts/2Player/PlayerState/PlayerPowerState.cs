using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.POWER; } }

    private bool IsReady { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        IsReady = true;
        m_player.ReadyPower();
    }


    private void DrawPowerAim()
    {
        m_player.TracePowerAim();
    }

    private void PowerMove()
    {
        Vector2 inputDir = m_player.InputVector;
        m_player.MoveDirection(inputDir, 0.8f);                 // �̵�
        m_player.SetMoveAnimation(inputDir);                    // �ִϸ����� ����
        m_player.LookAim();                                     // ȸ��
    }


    public void Proceed()
    {
        if (IsReady)            // �غ� ������ ��
        {
            if (m_player.AttackTrigger)
            {
                m_player.FirePower();
                IsReady = false;
                return;
            }
            if (m_player.GuardTrigger)
            {
                m_player.CancelPower();
                return;
            }
        }
    }

    public void FixedProceed()
    {
        if (IsReady)
        {
            PowerMove();
            DrawPowerAim();
            if (m_player.IsRaycastPower) { m_player.CheckRaycast(); }
        }
    }
}
