using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterAttribue
{
    ID,                         // ID
    TYPE,                       // 타입
    NAME,                       // 이름
    MAX_HP,                     // 최대 체력
    DAMAGE,                     // 데미지
    ROAMING_SPEED,              // 로밍 속도
    APPROACH_SPEED,             // 접근 속도
    VIEW_ANGLE,                 // 시야각
    VIEW_RANGE,                 // 시야 범위
    ENGAGE_RANGE,               // 전투 범위
    RETURN_RANGE,               // 복귀 범위
    ATTACK_RANGE,               // 공격 범위
    ATTACK_SPEED,               // 공격 속도
    APPROACH_DELAY,             // 접근 딜레이
    FENCE_RANGE,                // 거리 유지 범위
    DESCRIPTION,                // 설명
    LAST
}

public enum EMonsterType
{
    NORMAL,
    ELITE,
    BOSS,
    LAST
}

public enum EMonsterName            // 몬스터 이름
{
    STARVED_HHM,        // 굶주린 흠
    WOLF,               // 늑대
    BLOKAN,             // 블로칸

    MISERABLE_HHM,      // 비참한 흠
    FROMETZ,            // 프로메츠
    MARMULAK,           // 마르물락 파수꾼

    ARROGANT_HHM,       // 거만한 흠
    BLINK_BEAK,         // 깜빡부리
    UUM,                // 움

    SKURRABY_LIFE,      // 생명 해골딱지
    SKURRABY_WATER,     // 물 해골딱지
    SKURRABY_CRYSTAL,   // 광물 해골딱지
    QUEEN_LIFE,         // 생명 해골딱지 여왕
    QUEEN_WATER,        // 물 해골딱지 여왕
    QUEEN_CRYSTAL,      // 광물 해골딱지 여왕
    HELLCAT_LIFE,       // 생명 땅고양이
    HELLCAT_WATER,      // 물 땅고양이
    HELLCAT_CRYSTAL,    // 광물 땅고양이
    SPIRIT1_LIFE,       // 생명 정령1
    SPIRIT1_WATER,      // 물 정령1
    SPIRIT1_CRYSTAL,    // 광물 정령1
    SPIRIT2_LIFE,       // 생명 정령2
    SPIRIT2_WATER,      // 물 정령2
    SPIRIT2_CRYSTAL,    // 광물 정령2
    MINIUUM_LIFE,       // 생명 미니움
    MINIUUM_WATER,      // 물 미니움
    MINIUUM_CRYSTAL,    // 광물 미니움
    BLO,                // 블로
    LIFE_GUARDIAN,      // 생명 문지기
    WATER_GUARDIAN,     // 물 문지기
    CRYSTAL_GUARDIAN,   // 광물 문지기

    LAST
}
