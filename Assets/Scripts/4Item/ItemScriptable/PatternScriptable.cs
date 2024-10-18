using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternScriptable : ItemScriptable
{
    public float HealAmount;
    public float BuffTime;
    public override void SetItemScriptable(uint _idx, string[] _data, GameObject _prefab, Sprite _image)
    {
        base.SetItemScriptable(_idx, _data, _prefab, _image);
        float.TryParse(_data[(int)EItemAttribute.HEAL_AMOUNT],  out HealAmount);
        float.TryParse(_data[(int)EItemAttribute.BUFF_TIME],    out BuffTime);
    }
}
