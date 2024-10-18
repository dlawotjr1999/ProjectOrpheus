using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour, IHaveData
{
    // 퀘스트 목록
    private QuestInfo[] m_questInfoList;
    public List<QuestInfo> QuestInfoList { get { return m_questInfoList.ToList(); } }

    private void InitQuestInfos()
    {
        m_questInfoList  = new QuestInfo[(int)EQuestName.LAST];
        for(int i=0;i<(int)EQuestName.LAST;i++)
        {
            m_questInfoList[i] = new((EQuestName)i);
        }
    }

    public void SetQuestStatus(EQuestName _quest, EQuestState _status) 
    {
        m_questInfoList[(int)_quest].SetQuestStatus(_status);
        if(_status == EQuestState.ACCEPTED) { GameManager.PlaySE(ESystemSE.QUEST_ACCEPT); }
        if(_status == EQuestState.COMPLETE) { CompleteQuest(_quest); GameManager.PlaySE(ESystemSE.QUEST_COMPLETE); }
        if(_status == EQuestState.FINISH) { FinishQuest(_quest); GameManager.PlaySE(ESystemSE.QUEST_FINISH); }
        PlayManager.UpdateQuestSidebar();
        // PlayManager.UPdateQuestUI();
    }

    public void SetQuestProgress(EQuestName _quest, float _prog)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestProgress(_prog);
        if(_prog == info.QuestContent.Amount) { SetQuestStatus(_quest, EQuestState.COMPLETE); return; }
        PlayManager.UpdateQuestSidebar();
        // PlayManager.UPdateQuestUI();
    }
    public void GiveUpQuest(EQuestName _quest)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestProgress(0);
        info.SetQuestStatus(EQuestState.UNLOCKED);
        PlayManager.UpdateQuestSidebar();
    }
    private void CompleteQuest(EQuestName _quest)
    {
        QuestScriptable data = GameManager.GetQeustData(_quest);
        GameManager.PlaySE(ESystemSE.QUEST_COMPLETE);
        foreach (NPCDialogue dial in data.ResultDialogues)
        {
            PlayManager.UnlockDialogue(dial);
        }
    }
    private void FinishQuest(EQuestName _quest)
    {
        QuestScriptable data = GameManager.GetQeustData(_quest);
        QuestReward reward = data.Reward;
        GetReward(reward);
    }

    private void GetReward(QuestReward _reward)
    {
        int amount = _reward.Amount;
        switch(_reward.Type)
        {
            case ERewarTyoe.SOUL:
                PlayManager.AddSoul(_reward.Amount);
                break;
            case ERewarTyoe.PURIFIED:
                PlayManager.AddPurified(_reward.Amount);
                break;
            case ERewarTyoe.STAT:
                if (_reward.IsStatPoint) { PlayManager.AddStatPoint(_reward.Amount); }
                else { PlayManager.UpgradeStat(_reward.Stat, amount); }
                break;
            case ERewarTyoe.ITEM:
                SItem item = _reward.Item;
                PlayManager.AddInventoryItem(item, amount);
                break;
            default:
                break;
        }
    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { InitQuestInfos(); return; }

        SaveData data = PlayManager.CurSaveData;

        m_questInfoList = new QuestInfo[(int)EQuestName.LAST];
        for (int i = 0; i<(int)EQuestName.LAST; i++)
        {
            m_questInfoList[i] = new(data.QuestInfos[i]);
        }
    }

    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i<(int)EQuestName.LAST; i++)
        {
            data.QuestInfos[i] = new(m_questInfoList[i]);
        }
    }


    public static EQuestName String2Enum(string _id)
    {
        if(_id == "") { return EQuestName.LAST; }
        int.TryParse(_id[1..], out int idx);
        return (EQuestName)idx;
    }


    public void SetManager()
    {
        LoadData();
    }
}
