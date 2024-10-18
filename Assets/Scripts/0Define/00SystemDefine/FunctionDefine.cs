using UnityEngine;
using UnityEngine.EventSystems;

public delegate void FPointer();                                    // 함수 포인터
public delegate void EventPointer(PointerEventData _data);          // 마우스 정보 갖는 함수 포인터

public static class FunctionDefine
{
    public static int Abs(int _n) { return (int)Abs((float)_n); }
    public static float Abs(float _n) { if (_n >=0) { return _n; } return -_n; }
    public static int Min(int _n1, int _n2) { return (int)Min((float)_n1, _n2); }
    public static float Min(float _n1, float _n2) { if (_n1 <= _n2) { return _n1; } return _n2; }
    public static int Max(int _n1, int _n2) { return (int)Max((float)_n1, _n2); }
    public static float Max(float _n1, float _n2) { if (_n1 >= _n2) { return _n1; } return _n2; }

    public static float Sin(float _angle) { return Mathf.Sin(_angle * Mathf.Deg2Rad); }                 // 사인 (각도로 반환)
    public static float Cos(float _angle) { return Mathf.Cos(_angle * Mathf.Deg2Rad); }                 // 코사인 (각도로 반환)

    public static float Round(float _num)                                                             // 반올림
    {
        float unit = (float)Mathf.Round(_num);

        if (_num - unit < 0.000001f && _num - unit > -0.000001f) return unit;
        else return _num;
    }
    public static float RoundF1(float _num)
    {
        int n1 = Mathf.RoundToInt(_num * 10);
        return n1 * 0.1f;
    }
    public static float RoundF3(float _num)
    {
        int n3 = Mathf.RoundToInt(_num * 1000);
        return n3 * 0.001f;
    }

    public static T2[] GetRow<T1, T2>(T1[,] _array, int _row) where T2 : T1
    {
        int cols = _array.GetLength(1);
        T2[] row = new T2[cols];

        for (int i = 0; i < cols; i++)
        {
            row[i] = (T2)_array[_row, i];
        }

        return row;
    }


    public static bool CheckAnimParameter(Animator _anim, string _name)                                             // 애니메이터에 특정 변수명 있는지 확인
    {
        return CheckAnimParameter(_anim, _name, 0);
    }
    public static bool CheckAnimParameter(Animator _anim, string _name, AnimatorControllerParameterType _type)      // 종류까지 확인
    {
        foreach (AnimatorControllerParameter parameter in _anim.parameters)
        {
            if ((_type == 0 || parameter.type == _type) && parameter.name == _name)
            {
                return true;
            }
        }
        return false;
    }
    public static void AddEvent(EventTrigger _trigger, EventTriggerType _type, EventPointer _function)  // 이벤트 트리거에 이벤트 추가
    {
        EventTrigger.Entry entry = new() { eventID = _type };
        entry.callback.AddListener(data => { _function((PointerEventData)data); });
        _trigger.triggers.Add(entry);
    }

    public static float VecToDeg(Vector2 _vec)              // 벡터=>각도 변환
    {
        float deg = 90 - Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;
        return deg;
    }
    public static Vector2 DegToVec(float _deg)              // 각도=>벡터 변환
    {
        if (_deg == 0) return Vector2.up;
        else if (_deg < 180)
        {
            float tan = Mathf.Tan((90-_deg) * Mathf.Deg2Rad);
            return new Vector2(1, tan).normalized;
        }
        else if (_deg == 180) return Vector2.down;
        else
        {
            float tan = Mathf.Tan((90-_deg) * Mathf.Deg2Rad);
            return new Vector2(-1, -tan).normalized;
        }
    }

    public static Vector3 AngleToDir(float _angle)
    {
        float radian = _angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }


    public static Vector2 RotateVector2(Vector2 _vec, float _deg)       // 벡터 회전
    {
        float deg = -_deg;
        float x = _vec.x;
        float y = _vec.y;
        return new(x * Cos(deg) - y * Sin(deg), x * Sin(deg) + y * Cos(deg));
    }

    public static void SetFriction(Collider _collider, float _frictionWall, bool _isMinimum)
    {
        _collider.material.dynamicFriction = 0.6f * _frictionWall;
        _collider.material.staticFriction = 0.6f * _frictionWall;

        if (_isMinimum) _collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        else _collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
    }


    public static bool IsTerrain(GameObject _obj)
    {
        return _obj.tag switch
        {
            ValueDefine.TERRAIN_TAG => true,

            _ => false
        };
    }

    public static bool CheckCurAnimation(Animator _anim, int _layer, string _name)        // 현재 애니메이션 확인
    {
        return _anim.GetCurrentAnimatorStateInfo(_layer).IsName(_name);
    }




    public static void SetTransparent(Material _mat)                    // 메테리얼 Transparent 설정 (투명 가능)
    {
        _mat.SetFloat("_Surface", 1f);
        _mat.SetFloat("_Blend", 0f);

        _mat.SetOverrideTag("RenderType", "Transparent");
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _mat.SetInt("_ZWrite", 0);
        _mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        _mat.SetShaderPassEnabled("ShadowCaster", false);
    }
    public static void SetObaque(Material _mat)                         // 메테리얼 Obaque 설정 (투명 불가능)
    {
        _mat.SetFloat("_Surface", 0f);

        _mat.SetOverrideTag("RenderType", "");
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        _mat.SetInt("_ZWrite", 1);
        _mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _mat.renderQueue = -1;
        _mat.SetShaderPassEnabled("ShadowCaster", true);
    }
}
