using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.DIE; } }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        m_player.StopMove();
        m_player.DieAnimation();
    }

    public void Proceed()
    {

    }

    public void FixedProceed()
    {

    }
}
