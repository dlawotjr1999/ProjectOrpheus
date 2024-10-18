using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EMonsterState
{
    IDLE,
    APPROACH,
    ATTACK,
    SKILL,
    HIT,
    DIE,
    LAST
}
public interface IMonsterState
{
    public EMonsterState StateEnum { get; }
    public void ChangeTo(MonsterScript _monster);
    public void Proceed();

}
public class MonsterStateManager
{
    private readonly MonsterScript m_monster;
    public IMonsterState CurMonsterState { get; set; }      // 현재 몬스터 상태
    public void ChangeState(IMonsterState _state) { CurMonsterState = _state; CurMonsterState.ChangeTo(m_monster);}

    public MonsterStateManager(MonsterScript _monster) { m_monster = _monster; }
}
