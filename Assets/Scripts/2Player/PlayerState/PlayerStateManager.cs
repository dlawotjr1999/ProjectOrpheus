using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
    IDLE,
    MOVE,
    JUMP,
    FALL,
    ATTACK,
    GUARD,
    POWER,
    ROLL,
    THROW,
    HIT,
    DIE,
    LAST
}

public interface IPlayerState
{
    public EPlayerState StateEnum { get; }                  // 상태에 대항하는 Enum (필요한진 모르겠음, 나중에 안쓰면 없애도 됨)
    public void ChangeTo(PlayerController _player);         // 상태로 전환
    public void Proceed();                                  // 현재 상태일 시 Update마다 실행
    public void FixedProceed();                             // Fixed Update마다 실행 (물리 관련)
}

public class PlayerStateManager
{
    private readonly PlayerController m_player;             // 관리자를 갖고 있는 플레이어 클래스


    public IPlayerState CurState { get; private set; }      // 현재 상태
    public void ChangeState(IPlayerState _state) { CurState = _state; CurState.ChangeTo(m_player); }    // 상태 전환


    public PlayerStateManager(PlayerController _player) { m_player = _player; }     // 생성자
}
