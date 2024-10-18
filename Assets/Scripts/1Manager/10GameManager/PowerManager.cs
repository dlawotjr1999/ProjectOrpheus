using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PowerInfo
{
    public PowerScriptable PowerData { get; private set; }
    public string PowerName { get { return PowerData.PowerName; } }
    public string PowerDescription { get { return PowerData.Description; } }
    public ECastType CastType { get { return PowerData.CastType; } }
    public EPowerProperty[] PowerProps { get { return PowerData.PowerProps; } }
    public float PowerCooltime { get { return PowerData.Cooltime; } }
    public float PowerRadius { get { return PowerData.HitRadius; } }
    public float PowerCastRange { get{ return PowerData.CastingRange; } } 
    public int MotionIdx { get { return PowerData.MotionIdx; } }
    public bool HideWeapon { get { return PowerData.HideWeapon; } }
    public bool ShowCastingEffect { get { return PowerData.ShowCastingEffect; } }

    public PowerInfo(PowerScriptable _scriptable)
    {
        PowerData = _scriptable;
    }
}

public class PowerManager : MonoBehaviour
{
    private readonly PowerInfo[] m_powerInfo = new PowerInfo[(int)EPowerName.LAST];
    public PowerInfo GetPowerInfo(EPowerName _skill) { return m_powerInfo[(int)_skill]; }

    [SerializeField]
    private PowerScriptable[] m_powerData;

    public GameObject[] PowerArrays { get {
            GameObject[] array = new GameObject[m_powerData.Length];
            for(int i = 0; i<array.Length; i++) { array[i] = m_powerData[i].PowerPrefab; }
            return array; } }


    public PowerScriptable GetPowerData(EPowerName _skill)
    {
        if (_skill == EPowerName.LAST)
        {
            Debug.Log("여기");
        }
        return m_powerData[(int)_skill];
    }
    public GameObject GetPowerObj(EPowerName _skill)
    {
        return PoolManager.GetObject(m_powerData[(int)_skill].PowerPrefab);
    }

    public void SetPowerData(List<PowerScriptable> _data)
    {
        m_powerData = new PowerScriptable[_data.Count];
        for(int i = 0; i<m_powerData.Length; i++) { m_powerData[i] = _data[i]; }
    }


    public static EPowerProperty[] String2PowerProps(string _data)
    {
        if (_data == "") { return new EPowerProperty[0]; }
        string[] datas = _data.Split('/');
        EPowerProperty[] props = new EPowerProperty[datas.Length];
        for (int i = 0; i<datas.Length; i++)
        {
            props[i] = String2PowerProp(datas[i]);
        }
        return props;
    }
    private static EPowerProperty String2PowerProp(string _data)
    {
        return _data switch
        {
            "Slash" => EPowerProperty.SLASH,
            "Hit" => EPowerProperty.HIT,
            "Explosion" => EPowerProperty.EXPLOSION,
            "Shockwave" => EPowerProperty.SHOCKWAVE,
            "Fog" => EPowerProperty.FOG,
            "Totem" => EPowerProperty.TOTEM,
            "Light" => EPowerProperty.LIGHT,
            "Soul" => EPowerProperty.SOUL,

            _ => EPowerProperty.LAST
        };
    }


    public static ECCType IDToCC(string _code)
    {
        char c2 = _code[1];
        if(c2 != ValueDefine.MELEE_CC_CODE && c2 != ValueDefine.RANGED_CC_CODE && c2 != ValueDefine.AROUND_CC_CODE)
        { return ECCType.NONE; }
        return (ECCType)(_code[2] - '1');
    }





    public void SetManager()
    {
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            m_powerInfo[i] = new PowerInfo(GameManager.GetPowerData((EPowerName)i));
        }
    }
}
