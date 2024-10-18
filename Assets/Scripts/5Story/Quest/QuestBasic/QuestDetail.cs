using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class QuestInfo          // 퀘스트 현황
{
    public EQuestName QuestName;        // Enum
    public EQuestState State;           // 상태
    public float QuestProgress;         // 진행도
    public float QuestTimeCount;        // 남은 시간
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
public struct QuestContent      // 퀘스트 내용
{
    public EQuestType Type;             // 종류
    public float Amount;                // 양
    public EMonsterName Monster;        // 몬스터
    public SItem Item;                  // 아이템
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
public struct QuestReward       // 퀘스트 보상
{
    public ERewarTyoe Type;             // 종류
    public int Amount;                  // 양
    public EStatName Stat;              // 스탯
    public SItem Item;                  // 아이템
    public bool IsStatPoint { get { return Type == ERewarTyoe.STAT && Stat == EStatName.LAST; } }
    public bool IsNull { get { return Type == ERewarTyoe.LAST; } }
    public static QuestReward Null { get { return new(ERewarTyoe.LAST, -1); } }
    public QuestReward(ERewarTyoe _type, int _amount) { Type = _type; Amount = _amount; Stat = EStatName.LAST; Item = SItem.Empty; }
    public QuestReward(EStatName _stat, int _amount) { Type = ERewarTyoe.STAT; Amount = _amount; Stat = _stat; Item = SItem.Empty; }
    public QuestReward(SItem _item, int _amount) { Type = ERewarTyoe.ITEM; Amount = _amount; Stat = EStatName.LAST; Item = _item; }
}