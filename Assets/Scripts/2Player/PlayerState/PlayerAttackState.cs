using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.ATTACK; } }

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.StartAttack();
        // GameManager.PlaySE(EPlayerSE.SLASH1);
    }

    public void Proceed()
    {
        if (m_player.AttackFinished && m_player.CanGaurd)
        {
            m_player.BreakAttack();
        }
        if (m_player.CanAttack && m_player.AttackCreated)
        {
            m_player.ToNextAttack();
            return;
        }
    }

    public void FixedProceed()
    {
        m_player.LookAim();         // 조준 방향 바라보기
        if (!m_player.AttackFinished) m_player.AttackForward();
    }
}