using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NPCDialogue
{
    public SNPC NPC;
    public int Idx;
    public static NPCDialogue Null { get { return new(SNPC.Null, -1); } }
    public bool IsNull { get { return NPC.IsNull; } }
    public static bool operator ==(NPCDialogue _d1, NPCDialogue _d2) { return _d1.NPC == _d2.NPC && _d1.Idx == _d2.Idx; }
    public static bool operator !=(NPCDialogue _d1, NPCDialogue _d2) { return !(_d1 == _d2); }
    public NPCDialogue(SNPC _npc, int _idx) { NPC = _npc; Idx = _idx; }

    public readonly override bool Equals(object _obj)
    {
        return _obj is NPCDialogue dialogue&&
               EqualityComparer<SNPC>.Default.Equals(NPC, dialogue.NPC)&&
               Idx==dialogue.Idx;
    }
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(NPC, Idx);
    }
}

[Serializable]
public struct DialLine
{
    [TextArea]
    public string Text;
    public List<NPCDialogue> ResultDialogues;
    public DialQuest ResultQuest;
    public bool HasQuest { get { return !ResultQuest.IsNull; } }
    public bool OpenQuestUI { get { return HasQuest && ResultQuest.Function != EDialQuestFunction.COMPLETE; } }
    public DialLine(string _line) : this(_line, new(), DialQuest.Null) { }
    public DialLine(string _line, List<NPCDialogue> _dials) : this(_line, _dials, DialQuest.Null) { }
    public DialLine(string _line, DialQuest _quest) : this(_line, new(), _quest) { }
    public DialLine(string _line, List<NPCDialogue> _dials, DialQuest _quest)
    {
        Text = _line;
        ResultDialogues = new();
        foreach (NPCDialogue dial in _dials) { ResultDialogues.Add(dial); }
        ResultQuest = _quest;
    }
}

[Serializable]
public struct DialQuest
{
    public EQuestName Quest;
    public EDialQuestFunction Function;
    public readonly bool IsNull { get { return Quest == EQuestName.LAST; } }
    public static DialQuest Null { get { return new(EQuestName.LAST, EDialQuestFunction.LAST); } }
    public DialQuest(EQuestName _quest, EDialQuestFunction _func) { Quest = _quest; Function = _func; }
}
