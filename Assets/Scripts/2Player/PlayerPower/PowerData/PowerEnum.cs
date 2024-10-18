using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EPowerAttribute
{
    ID,                         // ID
    CAST_TYPE,                  // 캐스팅 타입
    PROPERTY,                   // 스킬 속성
    NAME,                       // 이름
    ATTACK,                     // 물리 공격 계수
    MAGIC,                      // 주술 공격 계수
    MOVE_SPEED,                 // 이동 속도?
    CASTING_RANGE,              // 사거리
    HIT_RADIUS,                 // 공격하는 범위
    PRE_DELAY,                  // 선딜레이
    TOTAL_DELAY,                // 총 딜레이
    COOLTIME,                   // 쿨타임
    STAMINA_COST,               // 스테미나 사용량
    ADJ_TYPE,                   // 스탯 변동 (버프 or 디버프)
    ADJ_AMOUNT,                 // ㄴ변동량
    ADJ_TIME,                   // ㄴ지속 시간
    DESCRIPTION,                // 설명
    PRICE,                      // 가격

    LAST
}

public enum ECastType
{
    MELEE,
    MELEE_CC,
    RANGED,
    RANGED_CC,
    SUMMON,
    AROUND,
    AROUND_CC,
    BUFF,

    LAST
}

public enum EPowerProperty
{
    SLASH,
    HIT,
    EXPLOSION,
    SHOCKWAVE,
    FOG,
    TOTEM,
    LIGHT,
    SOUL,

    LAST
}

public enum EPowerName
{
    MELEE_SLASH,
    MELEE_SOUL,
    MELEE_KNOCKBACK,
    MELEE_MELANCHOLY,
    MELEE_EXTORTION,
    MELEE_AIRBORNE,
    RANGED_PROJ1,
    RANGED_PROJ2,
    RANGED_PROJ3,
    RANGED_PARAB1,
    RANGED_PARAB2,
    RANGED_PARAB3,
    RANGED_PARAB4,
    RANGED_SLASH1,
    RANGED_SLASH2,
    RANGED_FATIGUE1,
    RANGED_FATIGUE2,
    RANGED_MELANCHOLY,
    RANGED_KNOCKBACK1,
    RANGED_KNOCKBACK2,
    CREATION_SOUL,
    CREATION_TOTEM1,
    CREATION_TOTEM2,
    CREATION_TOTEM3,
    CREATION_TOTEM4,
    CREATION_TOTEM5,
    AROUND_SHOCKWAVE,
    AROUND_SLASH,
    AROUND_FOG,
    AROUND_FATIGUE,
    AROUND_AIRBORNE,
    AROUND_BIND,
    AROUND_VOID,
    BUFF_MAXHP,
    BUFF_COMBAT,
    BUFF_FATIGUE,
    BUFF_KNOCKBACK,
    BUFF_MELANCHOLY,
    BUFF_EXTORTION,

    LAST
}