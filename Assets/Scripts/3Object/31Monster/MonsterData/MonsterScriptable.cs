using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MonsterScriptable : ScriptableObject
{
    public uint             Idx;
    public EMonsterName     MonsterEnum;
    public string           ID;
    public EMonsterType     MonsterType;            // 종류 (보통, 엘리트, 보스)
    public string           MonsterName;
    public int              MaxHP;
    public float            Attack;
    public float            MoveSpeed;
    public float            ApproachSpeed;          // 접근 시 속도
    public float            ViewAngle;              // 시야각
    public float            ViewRange;              // 시야 범위
    public float            EngageRange;            // 뭐였지
    public float            ReturnRange;            // 원래 위치로 돌아가는 범위
    public float            AttackRange;
    public float            AttackSpeed;
    public float            ApproachDelay;          // 접근 딜레이
    public float            FenceRange;             // 돌아다니는 범위 returnRange랑 뭐가 다름?
    [TextArea]
    public string           Description;
    public int              FirstKillStat = 1;      // 최초 처치 시 스탯 양
    public DropInfo         DropInfo;
    public GameObject       MonsterPrefab;
    public Sprite           MonsterProfile;
    public Sprite           MonsterBodyImg;

    private EMonsterType String2Type(string _data)
    {
        return _data switch
        {
            "NORMAL" => EMonsterType.NORMAL,
            "ELITE" => EMonsterType.ELITE,
            "BOSS" => EMonsterType.BOSS,

            _ => EMonsterType.LAST
        };
    }

    public void SetMonsterScriptable(uint _idx, string[] _data, DropInfo _drop, GameObject _prefab, Sprite _profile, Sprite _body)
    {
        Idx =           _idx;
        MonsterEnum =   (EMonsterName)_idx;
        ID =            _data[(int)EMonsterAttribue.ID];
        MonsterType =   String2Type(_data[(int)EMonsterAttribue.TYPE]);
        MonsterName =   _data[(int)EMonsterAttribue.NAME];
        int.TryParse(   _data[(int)EMonsterAttribue.MAX_HP],            out MaxHP);
        float.TryParse( _data[(int)EMonsterAttribue.DAMAGE],            out Attack);
        float.TryParse( _data[(int)EMonsterAttribue.ROAMING_SPEED],     out MoveSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.APPROACH_SPEED],    out ApproachSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.VIEW_ANGLE],        out ViewAngle);
        float.TryParse( _data[(int)EMonsterAttribue.VIEW_RANGE],        out ViewRange);
        float.TryParse( _data[(int)EMonsterAttribue.ENGAGE_RANGE],      out EngageRange);
        float.TryParse( _data[(int)EMonsterAttribue.RETURN_RANGE],      out ReturnRange);
        float.TryParse( _data[(int)EMonsterAttribue.ATTACK_RANGE],      out AttackRange);
        float.TryParse( _data[(int)EMonsterAttribue.ATTACK_SPEED],      out AttackSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.APPROACH_DELAY],    out ApproachDelay);
        float.TryParse( _data[(int)EMonsterAttribue.FENCE_RANGE],       out FenceRange);
        Description =   _data[(int)EMonsterAttribue.DESCRIPTION];
        DropInfo =      _drop;
        MonsterPrefab = _prefab;
        if (_profile != null) { MonsterProfile = _profile; }
        if (_body != null) { MonsterBodyImg = _body; }
    }
}
