using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemScriptable : ItemScriptable
{
    public EThrowItemName   ItemEnum;
    public float            ThrowDamage;
    public float            ThrowSpeed;
    public float            ExplodeTime;
    public override void SetItemScriptable(uint _idx, string[] _data, GameObject _prefab, Sprite _image)
    {
        base.SetItemScriptable(_idx, _data, _prefab, _image);
        ItemEnum =      (EThrowItemName)_idx;
        float.TryParse( _data[(int)EItemAttribute.THROW_DAMAGE],    out ThrowDamage);
        float.TryParse( _data[(int)EItemAttribute.THROW_SPEED],     out ThrowSpeed);
        float.TryParse( _data[(int)EItemAttribute.EXPLODE_TIME],    out ExplodeTime);
    }
}
