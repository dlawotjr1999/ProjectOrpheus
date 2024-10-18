using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectedMeleePower : MeleePowerScript
{
    public override void SetPower(PlayerController _player, float _attack, float _magic)
    {
        base.SetPower(_player, _attack, _magic);
        PowerEffect.EffectOn(transform, m_lastTime);
    }
}
