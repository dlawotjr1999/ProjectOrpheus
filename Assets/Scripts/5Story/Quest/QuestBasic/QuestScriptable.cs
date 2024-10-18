using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Id;                           // ����Ʈ ID
    public EQuestName Enum;
    public string Name;                         // ����Ʈ �̸�
    public QuestContent Content;                // ����Ʈ ����
    public float TimeLimit;                     // ���� �ð�
    public List<NPCDialogue> ResultDialogues;   // �Ϸ� �� ����
    public List<EQuestName> ResultQuests;       // �Ϸ� �� ����
    public string Description;                  // ����Ʈ ����
    public string CompleteInfo;                 // �Ϸ� �� ���̵�ٿ� ǥ��
    public QuestReward Reward;                  // ����

    public void AddResultQuest(EQuestName _quest)
    {
        ResultQuests.Add(_quest);
    }

    private QuestContent String2Content(string _type, string _detail, string _amount)
    {
        float.TryParse(_amount, out float amount);
        switch (_type)
        {
            case "FIND":
                SNPC npc = StoryManager.String2NPC(_detail);
                if(!npc.IsNull) { return new(EQuestType.FIND, npc); }
                EMonsterName monster = MonsterManager.ID2Monster(_detail);
                if(monster != EMonsterName.LAST) { return new(EQuestType.FIND, monster, 1); }
                break;
            case "TALK":
                return new(EQuestType.TALK, StoryManager.String2NPC(_detail));
            case "KILL":
                return new(EQuestType.KILL, MonsterManager.ID2Monster(_detail), amount);
            case "PURIFY":
                return new(EQuestType.PURIFY, MonsterManager.ID2Monster(_detail), amount);
            case "COLLECT":
                return new(EQuestType.COLLECT, ItemManager.ID2Item(_detail), amount);

        }
        Debug.LogError("����Ʈ ���� �߸� �Էµ�");
        return QuestContent.Null;
    }
    private QuestReward String2Reward(string _type, string _num, string _detail)
    {
        int.TryParse(_num, out int num);
        switch (_type)
        {
            case "SOUL":
                return new(ERewarTyoe.SOUL, num);
            case "PURIFIED":
                return new(ERewarTyoe.PURIFIED, num);
            case "STAT":
                EStatName stat = PlayerForceManager.String2Stat(_detail);
                if (stat == EStatName.LAST) { return new(ERewarTyoe.STAT, num); }
                else { return new(stat, num); }
            case "TRAIT":
                // �̱���
                break;
            case "ITEM":
                SItem item = ItemManager.ID2Item(_detail);
                return new(item, num);
        }
        Debug.LogError("���� Ÿ�� �߸� �Էµ�");
        return QuestReward.Null;
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx =               _idx;
        Id =                _data[(int)EQuestAttributes.ID];
        Enum =              (EQuestName)Idx;
        Name =              _data[(int)EQuestAttributes.NAME];
        Content =           String2Content(_data[(int)EQuestAttributes.QUEST_TYPE], _data[(int)EQuestAttributes.QUEST_DETAIL], _data[(int)EQuestAttributes.QUEST_AMOUNT]);
        float.TryParse(     _data[(int)EQuestAttributes.TIME_LIMIT], out TimeLimit);
        ResultDialogues =   StoryManager.String2Dialogues(_data[(int)EQuestAttributes.RESULT_DIALOGUES]);
        ResultQuests = new();
        Description =       _data[(int)EQuestAttributes.DESCRIPTION];
        CompleteInfo =      _data[(int)EQuestAttributes.COMPLETE_INFO];
        Reward =            String2Reward(_data[(int)EQuestAttributes.REWARD_TYPE], _data[(int)EQuestAttributes.REWARD_NUM], _data[(int)EQuestAttributes.REWARD_DETAIL]);
    }
}
