using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct FRange
{
    public float Min;
    public float Max;
    public float Num { get { return UnityEngine.Random.Range(Min, Max); } }
    public FRange(float _min, float _max) { Min = _min; Max = _max; }
}

public struct BufferObject
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Size;
}

public struct HitData
{
    public ObjectScript Attacker;
    public float Damage;
    public Vector3 Point;
    public float Impulse;
    public ECCType[] CCList;
    public EPowerProperty Property;
    public bool IsNull { get { return Attacker == null; } }
    public static HitData Null { get { return new(null, -1, Vector3.zero); } }
    public readonly bool HasImpulse { get { return Impulse > 0; } }

    public HitData(ObjectScript _attacker, float _damage, Vector3 _point) : this(_attacker, _damage, _point, ECCType.NONE) { }

    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, float _impulse) : this(_attacker, _damage, _point, _impulse, ECCType.NONE) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType _cc) : this(_attacker, _damage, _point, 0, _cc) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType[] _cc) : this(_attacker, _damage, _point, 0, _cc) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, EPowerProperty _prop) : this(_attacker, _damage, _point, 0, new ECCType[0], _prop) { }



    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, float _impulse, ECCType _cc)
        : this(_attacker, _damage, _point, _impulse, new ECCType[] { _cc }, EPowerProperty.LAST) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, float _impulse, ECCType[] _cc)
        : this(_attacker, _damage, _point, _impulse, _cc, EPowerProperty.LAST) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, float _impulse, EPowerProperty _prop)
        : this(_attacker, _damage, _point, _impulse, new ECCType[0], _prop) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType _cc, EPowerProperty _prop)
        : this(_attacker, _damage, _point, 0, new ECCType[] { _cc }, _prop) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType[] _cc, EPowerProperty _prop)
        : this(_attacker, _damage, _point, 0, _cc, _prop) { }


    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, float _impulse, ECCType[] _cc, EPowerProperty _prop)
    {
        Attacker = _attacker;
        Damage = _damage;
        Point = _point;
        Impulse = _impulse;
        CCList = new ECCType[_cc.Length];
        for (int i = 0; i<_cc.Length; i++) { CCList[i] = _cc[i]; }
        Property = _prop;
    }
}

public struct CCInfo
{
    public ECCType Type;
    public float Amount;
    public bool IsNull { get { return Type == ECCType.LAST; } }
    public static CCInfo Null { get { return new(ECCType.LAST, 0); } }
    public bool HasAmount { get { return Amount > ValueDefine.DEFAULT_CC[(int)Type]; } }
    public CCInfo(ECCType _type) { Type = _type; Amount = ValueDefine.DEFAULT_CC[(int)_type]; }
    public CCInfo(ECCType _type, float _amount) { Type = _type; Amount = _amount; }
}