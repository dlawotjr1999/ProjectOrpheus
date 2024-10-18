using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.THROW; } }

    private const float NormalHitDelay = 0.667f;
    private const float GuardHitDelay = 0.333f;
    private float TimeCount { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        if (m_player.IsGuarding) { TimeCount = GuardHitDelay; }
        else { TimeCount = NormalHitDelay; }
        m_player.StartHit();
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            m_player.ActionDone();
        }
    }

    public void FixedProceed()
    {

    }
}
