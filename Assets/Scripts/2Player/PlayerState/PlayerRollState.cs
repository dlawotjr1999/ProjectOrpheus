using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerRollState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.ROLL; } }

    private Vector2 RollDirection { get; set; }
    private float TimeCount { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        RollDirection = m_player.InputVector;
        if(RollDirection == Vector2.zero) { RollDirection = Vector2.up; }

        m_player.StartRoll();
        TimeCount = m_player.RollingTime;
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if(TimeCount <= 0) { m_player.RollDone(); return; }
    }

    public void FixedProceed()
    {
        m_player.GroundMove(RollDirection, m_player.RollMultiplier * m_player.MoveSpeedMultiplier);
        m_player.RotateDirection(RollDirection);
    }
}
