using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScriptable : ScriptableObject
{
    public uint                 Idx;
    public EPowerName           PowerEnum;
    public string               ID;
    public ECastType            CastType;
    public EPowerProperty[]     PowerProps;
    public string               PowerName;
    public float                Attack;
    public float                Magic;
    public float                MoveSpeed;
    public float                CastingRange;
    public float                HitRadius;
    public float                PreDelay;
    public float                TotalDelay;
    public float                Cooltime;
    public int                  StaminaCost;
    public AdjustInfo           StatAdjust;
    [TextArea]
    public string               Description;
    public int                  PowerPrice;
    public GameObject           PowerPrefab;
    public Sprite               PowerIcon;
    public EPowerSE             CreateSound = EPowerSE.LAST;
    public EPowerSE             HitSound = EPowerSE.LAST;
    public int                  MotionIdx;
    public bool                 HideWeapon;
    public bool                 ShowCastingEffect;
    public EPowerTrailType      PowerTrail;

    public bool IsTrailOn { get { return PowerTrail > (int)EPowerTrailType.NONE; } }

    private ECastType Name2Type(EPowerName _powerName)
    {
        if (_powerName < EPowerName.MELEE_KNOCKBACK)
            return ECastType.MELEE;
        else if (_powerName < EPowerName.RANGED_PROJ1)
            return ECastType.MELEE_CC;
        else if (_powerName < EPowerName.RANGED_FATIGUE1)
            return ECastType.RANGED;
        else if (_powerName < EPowerName.CREATION_SOUL)
            return ECastType.RANGED_CC;
        else if (_powerName < EPowerName.AROUND_SHOCKWAVE)
            return ECastType.SUMMON;
        else if (_powerName < EPowerName.AROUND_FATIGUE)
            return ECastType.AROUND;
        else if (_powerName < EPowerName.BUFF_MAXHP)
            return ECastType.AROUND_CC;
        else if (_powerName < EPowerName.LAST)
            return ECastType.BUFF;
        else
            return ECastType.LAST;

    }


    public void SetPowerScriptable(uint _idx, string[] _data, GameObject _prefab, Sprite _icon)
    {
        Idx =               _idx;
        PowerEnum =         (EPowerName)_idx;
        ID =                _data[(int)EPowerAttribute.ID];
        CastType =          Name2Type(PowerEnum);
        PowerProps =        PowerManager.String2PowerProps(_data[(int)EPowerAttribute.PROPERTY]);
        PowerName =         _data[(int)EPowerAttribute.NAME];
        float.TryParse(     _data[(int)EPowerAttribute.ATTACK],           out Attack);
        float.TryParse(     _data[(int)EPowerAttribute.MAGIC],            out Magic);
        float.TryParse(     _data[(int)EPowerAttribute.MOVE_SPEED],       out MoveSpeed);
        float.TryParse(     _data[(int)EPowerAttribute.CASTING_RANGE],    out CastingRange);
        float.TryParse(     _data[(int)EPowerAttribute.HIT_RADIUS],       out HitRadius);
        float.TryParse(     _data[(int)EPowerAttribute.PRE_DELAY],        out PreDelay);
        float.TryParse(     _data[(int)EPowerAttribute.TOTAL_DELAY],      out TotalDelay);
        float.TryParse(     _data[(int)EPowerAttribute.COOLTIME],         out Cooltime);
        int.TryParse(       _data[(int)EPowerAttribute.STAMINA_COST],     out StaminaCost);
        EAdjType type =     DataManager.String2Adj(_data[(int)EPowerAttribute.ADJ_TYPE]);
        float.TryParse(     _data[(int)EPowerAttribute.ADJ_AMOUNT], out float amount);
        float.TryParse(     _data[(int)EPowerAttribute.ADJ_TIME], out float time);
        if(amount == 0)     { amount = (float)DataManager.String2CC(_data[(int)EPowerAttribute.ADJ_AMOUNT]); }
        StatAdjust =        new(type, amount, time);
        Description =       _data[(int)EPowerAttribute.DESCRIPTION];
        int.TryParse(       _data[(int)EPowerAttribute.PRICE],            out PowerPrice);
        PowerPrefab =       _prefab;
        if(_icon != null) { PowerIcon = _icon; }
    }
}
