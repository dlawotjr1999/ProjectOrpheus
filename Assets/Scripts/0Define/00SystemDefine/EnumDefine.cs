using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatName
{
    HEALTH,     // 체력
    ENDURE,     // 지구력
    STRENGTH,   // 근력
    INTELLECT,  // 지력
    RAPID,      // 순발력
    MENTAL,     // 정신력

    LAST
}
public enum ECombatInfoName
{
    MAX_HP,         // 최대 HP
    MAX_STAMINA,    // 최대 스테미나
    ATTACK,         // 물리 공격력
    MAGIC,          // 마법 공격력
    DEFENSE,        // 방어력
    OVERDRIVE,    // 치명타 데미지
    TOLERANCE,      // 내성
    LAST
}

public enum EProperty
{
    WATER = 0,
    LIFE = 1,
    CRYSTAL = 2,

    LAST,
}

public enum EInteractType
{
    NPC,
    OASIS,
    ITEM,
    LAST
} 

public enum ECCType
{
    NONE,               // 일반(없음)
    FATIGUE,            // 피로
    MELANCHOLY,         // 우울
    EXTORTION,          // 갈취
    AIRBORNE,           // 띄움
    KNOCKBACK,          // 밀림
    WEAKNESS,           // 나약
    BIND,               // 속박
    VOID,               // 상실
    OBLIVION,           // 망각
    BLIND,              // 실명

    LAST
}
