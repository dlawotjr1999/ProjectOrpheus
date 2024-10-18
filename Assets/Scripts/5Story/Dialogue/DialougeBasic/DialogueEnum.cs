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
    LOCKED,             // �Ⱥ���
    UNLOCKED,           // ��ȭ ����
    WAITING,            // ��� ��(����Ʈ ���� �� ���)
    COMPLETE,           // ��ȭ �Ϸ�

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
    START,              // �̼� ����
    COMPLETE,           // �̼� ���� �޼�
    FNIINSH,            // �̼� �Ϸ�
    COMPLETE_FINISH,    // ���� �޼� ���ÿ� �Ϸ�

    LAST
}