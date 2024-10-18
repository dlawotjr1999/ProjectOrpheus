using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkbeakEvadeState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum => EMonsterState.ATTACK;

    private BlinkbeakScript Blinkbeak { get { return (BlinkbeakScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Blinkbeak == null) { m_monster = _monster; }

        Blinkbeak.StartEvade();
    }

    public void Proceed()
    {
        Blinkbeak.EvadeProceed();
    }
}
