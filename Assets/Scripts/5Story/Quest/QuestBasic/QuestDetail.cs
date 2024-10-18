using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class QuestInfo          // ����Ʈ ��Ȳ
{
    public EQuestName QuestName;        // Enum
    public EQuestState State;           // ����
    public float QuestProgress;         // ���൵
    public float QuestTimeCount;        // ���� �ð�
    public void SetQuestStatus(EQuestState _status) { State = _status; }
    public void SetQuestProgress(float _prog) { QuestProgress = _prog; }
    public QuestScriptable QuestData { get { return GameManager.GetQeustData(QuestName); } }
    public QuestContent QuestContent { get { return QuestData.Content; } }
    public float QuestTimeLimit { get { return QuestData.TimeLimit; } }
    private void SetInfo(EQuestName _name, EQuestState _status, float _progress, float _time)
    {
        QuestName = _name;
        SetQuestStatus(_status);
        SetQuestProgress(_progress);
        QuestTimeCount = _time;
    }
    public QuestInfo() { }
    public QuestInfo(EQuestName _name) { SetInfo(_name, EQuestState.LOCKED, 0, 0); }
    public QuestInfo(QuestInfo _other) { SetInfo(_other.QuestName, _other.State, _other.QuestProgress, 0); }
}

[Serializable]
public struct QuestContent      // ����Ʈ ����
{
    public EQuestType Type;             // ����
    public float Amount;                // ��
    public EMonsterName Monster;        // ����
    public SItem Item;                  // ������
    public SNPC NPC;                    // NPC
    public bool IsNull { get { return Type == EQuestType.LAST; } }
    public static QuestContent Null { get { return new(EQuestType.LAST, SNPC.Null); } }
    public QuestContent(EQuestType _type, EMonsterName _monster, float _amount)
    { Type = _type; Amount = _amount; Monster = _monster; Item = SItem.Empty; NPC = SNPC.Null; }
    public QuestContent(EQuestType _type, SItem _item, float _amount)
    { Type = _type; Amount = _amount; Monster = EMonsterName.LAST; Item = _item; NPC = SNPC.Null; }
    public QuestContent(EQuestType _type, SNPC _npc)
    { Type = _type; Amount = 1; Monster = EMonsterName.LAST; Item = SItem.Empty; NPC = _npc; }
}

[Serializable]
public struct QuestReward       // ����Ʈ ����
{
    public ERewarTyoe Type;             // ����
    public int Amount;                  // ��
    public EStatName Stat;              // ����
    public SItem Item;                  // ������
    public bool IsStatPoint { get { return Type == ERewarTyoe.STAT && Stat == EStatName.LAST; } }
    public bool IsNull { get { return Type == ERewarTyoe.LAST; } }
    public static QuestReward Null { get { return new(ERewarTyoe.LAST, -1); } }
    public QuestReward(ERewarTyoe _type, int _amount) { Type = _type; Amount = _amount; Stat = EStatName.LAST; Item = SItem.Empty; }
    public QuestReward(EStatName _stat, int _amount) { Type = ERewarTyoe.STAT; Amount = _amount; Stat = _stat; Item = SItem.Empty; }
    public QuestReward(SItem _item, int _amount) { Type = ERewarTyoe.ITEM; Amount = _amount; Stat = EStatName.LAST; Item = _item; }
}