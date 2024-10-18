using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQuestAttributes
{
    ID,
    NAME,
    QUEST_TYPE,
    QUEST_DETAIL,
    QUEST_AMOUNT,
    TIME_LIMIT,
    RESULT_DIALOGUES,
    DESCRIPTION,
    COMPLETE_INFO,
    REWARD_TYPE,
    REWARD_NUM,
    REWARD_DETAIL,
    LAST
}

public enum EQuestType
{
    FIND,
    TALK,
    KILL,
    PURIFY,
    COLLECT,


    LAST
}

public enum EQuestState
{
    LOCKED,
    UNLOCKED,
    ACCEPTED,
    COMPLETE,
    FAIL,
    FINISH,

    LAST
}

public enum EQuestName
{
    KILL_YUM,
    PURIFY_YUM,
    KILL_BLO,
    KILL_UUM,
    TALK_ALTAR02_0,
    TALK_ALTAR02_2,
    KILL_HELLCAT,
    KILL_HUNGRY,
    TALK_ALTAR04_0,
    TALK_ALTAR04_2,
    PURIFY_BLOKAN,
    KILL_LIFE_GUARDIAN,
    KILL_MINIUUM_BLUE,
    KILL_SKURRABY_WATER,
    KILL_GLOOMY,
    PURIFY_MISERABLE,
    TALK_ALTAR06_0,
    PURIFY_MARMULAK,
    KILL_FROMETZ,
    KILL_WATER_GUARDIAN,
    KILL_COCKY,
    PURIFY_BLINKBEAK,
    PURIFY_HELLCAT,
    TALK_ALTAR08_0,
    PURIFY_COCKY,
    PURIFY_ARROGANT,
    KILL_QUEEN_CRYSTAL,
    KILL_CRYSTAL_GUARDIAN,

    LAST
}

public enum ERewarTyoe
{
    SOUL,
    PURIFIED,
    STAT,
    TRAIT,
    ITEM,

    LAST
}