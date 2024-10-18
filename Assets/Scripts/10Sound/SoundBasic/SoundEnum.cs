using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBGM                // BGM 이름
{
    TITLE_BGM,
    FIELD_BGM,
    BOSS_BGM,

    LAST
}

public enum EPlayerSE
{
    STEP1,
    STEP2,
    SLSAH,
    BLOW,
    HEAL,
    CASTING,
    INTERACT,
    ITEM_GET,
    LIGHT_ON,
    LIGHT_OFF,

    LAST
}

public enum EPowerSE
{
    NONE,
    POWER_SLASH1,
    POWER_SLASH2,
    SLASH_HIT1,
    SLASH_HIT2,
    MELEE_CRASH1,
    MELEE_CRASH2,
    MELEE_FLARE,
    FLARE_HIT,
    PROJECTILE1,
    PROJECTILE2,
    PROJECTILE3,
    PROJECTILE_HIT1,
    PROJECTILE_HIT2,
    PROJECTILE_HIT3,
    PROJECTILE_HIT4,
    WIND_BLADE1,
    WIND_BLADE2,
    WIND_HIT,
    SOUL_FIRE,
    TOTEM_STRIKE,
    TOTEM_BUFF,
    TOTEM_CC,
    SHOCKWAVE,
    AROUND_FOG,
    HP_BUFF,
    DAMAGE_BUFF,
    WEAPON_CC,

    LAST
}

public enum EMonsterSE
{
    PURIFY,

    LAST
}

public enum EEnvironmentSE
{
    REST,
    TRANSPORT,
    REGION_ENTER,
    BRIDGE_APPEAR,

    LAST
}

public enum ESystemSE
{
    OPEN_UI,
    BTN_CLICK,
    EQUIP,
    QUEST_ACCEPT,
    QUEST_COMPLETE,
    QUEST_FINISH,
    GAME_OVER,

    LAST
}