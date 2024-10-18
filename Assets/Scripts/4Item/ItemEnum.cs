using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemAttribute
{
    ID,                     // ID
    TYPE,                   // 타입
    NAME,                   // 이름
    DESCRIPTION,            // 설명
    DROP_RATE,              // 드랍율
    PRICE,                  // 가격
    MIN_ATTACK,             // 최소 물리
    MAX_ATTACK,             // 최대 물리
    MIN_MAGIC,              // 최소 주술
    MAX_MAGIC,              // 최대 주술
    ATTACK_SPEED,           // 무기 속도
    HEAL_AMOUNT,            // 회복량
    BUFF_TIME,              // 버프 시간
    THROW_DAMAGE,           // 투척 아이템 데미지
    THROW_SPEED,            // 투척 속도
    EXPLODE_TIME,           // 폭발 딜레이
    LAST
}

public enum EItemType
{
    WEAPON,                 // 무기
    PATTERN,                // 문양
    THROW,                  // 투척
    OTHERS,                 // 기타
    LAST
}

public enum EWeaponType
{
    BLADE,                  // 대검
    SWORD,                  // 세검
    SCEPTER,                // 홀

    LAST,
}


public enum EWeaponName
{
    BASIC_BLADE,            // 기본 대검
    BASIC_SWORD,            // 기본 세검
    BASIC_SCEPTER,          // 기본 홀
    GOBLIN_SCEPTER,         // 고블린 홀

    LAST
}

public enum EPatternName
{
    LIFE,
    LIFE_UP,
    WATER,
    WATER_UP,
    GROUND,
    GROUND_UP,
    LAST
}

public enum EThrowItemName
{
    STONE,
    BOMB,
    SLOW,
    ENDER,
    PURIFY,
    LAST
}

public enum EOtherItemName
{
    O101,
    O102,
    O103,
    O201,
    O202,
    O203,
    O301,
    O302,
    O303,
    O501,
    O502,
    O503,
    O504,
    O505,
    O506,
    O507,
    O508,
    O509,
    O510,
    O511,
    O512,
    O513,
    O514,
    O515,
    O516,
    O517,
    O518,
    O519,

    LAST
}

public enum EItemName       // 무기 제외 아이템 이름
{
    ITEM1,
    ITEM2,
    ITEM3,
    ITEM4,

    LAST
}

public enum EDropAttribute
{
    MONSTER,
    ITEM1,
    RATE1,
    ITEM2,
    RATE2,
    ITEM3,
    RATE3,
    ITEM4,
    RATE4,
    STAT,
    SOUL,
    PURIFIED,

    LAST
}