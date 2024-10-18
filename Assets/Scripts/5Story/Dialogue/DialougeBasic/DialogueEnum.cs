using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDialogueAttributes
{
    NPC,
    DIALOGUE_IDX,
    BRANCH_IDX,
    LINE_IDX,
    LINE_TEXT,
    RESULT_DIALOGUES,
    RESULT_QUEST,
    QUEST_FUNCTION,

    LAST
}

public enum EDialogueState
{
    LOCKED,             // 안보임
    UNLOCKED,           // 대화 가능
    WAITING,            // 대기 중(퀘스트 수락 후 대기)
    COMPLETE,           // 대화 완료

    LAST
}

public enum EDialogueName
{
    DIALOGUE1,
    DIALOGUE2,
    DIALOGUE3,

    LAST
}

public enum EDialQuestFunction
{
    START,              // 미션 제시
    COMPLETE,           // 미션 조건 달성
    FNIINSH,            // 미션 완료
    COMPLETE_FINISH,    // 조건 달성 동시에 완료

    LAST
}