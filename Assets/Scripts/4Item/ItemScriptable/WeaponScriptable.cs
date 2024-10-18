using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScriptable : ItemScriptable
{
    public EWeaponName WeaponEnum;
    public FRange Attack;
    public FRange Magic;
    public float AttackSpeed;

    public override void SetItemScriptable(uint _idx, string[] _data, GameObject _prefab, Sprite _image)
    {
        base.SetItemScriptable(_idx, _data, _prefab, _image);
        WeaponEnum =    (EWeaponName)_idx;
        float min, max;
        float.TryParse( _data[(int)EItemAttribute.MIN_ATTACK],      out min);
        float.TryParse(_data[(int)EItemAttribute.MAX_ATTACK],       out max);
        Attack = new(min, max);
        float.TryParse( _data[(int)EItemAttribute.MIN_MAGIC],       out min);
        float.TryParse(_data[(int)EItemAttribute.MAX_MAGIC],        out max);
        Magic = new(min, max);
        float.TryParse( _data[(int)EItemAttribute.ATTACK_SPEED],    out AttackSpeed);
    }
}
