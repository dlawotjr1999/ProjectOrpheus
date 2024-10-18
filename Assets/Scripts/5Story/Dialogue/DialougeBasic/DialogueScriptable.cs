using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScriptable : ScriptableObject
{
    public uint Idx;
    public SNPC NPC;
    public int DialogueIdx;
    public int BranchIdx;
    public DialLine[] Lines;
    public bool IsOpenAtFirst;

    public void DisableFirstOpen() { IsOpenAtFirst = false; }

    public NPCDialogue NPCDial { get { return new(NPC, DialogueIdx); } }

    private DialQuest String2Quest(string _id, string _func)
    {
        EQuestName quest = StoryManager.ID2Quest(_id);
        if(quest == EQuestName.LAST) { return DialQuest.Null; }
        EDialQuestFunction func = String2Func(_func);
        if(func == EDialQuestFunction.LAST) { return DialQuest.Null; }
        return new(quest, func);

    }
    private EDialQuestFunction String2Func(string _func)
    {
        return _func switch
        {
            "START" => EDialQuestFunction.START,
            "COMPLETE" => EDialQuestFunction.COMPLETE,
            "FINISH" => EDialQuestFunction.FNIINSH,
            "COMPLETE_FINISH" => EDialQuestFunction.COMPLETE_FINISH,

            _ => EDialQuestFunction.LAST
        };
    }

    public void SetScriptable(uint _idx, string _npc, List<string[]> _data)
    {
        Idx = _idx;
        NPC = StoryManager.String2NPC(_npc);
        int cnt = _data.Count;
        Lines = new DialLine[cnt];
        for (int i = 0; i<cnt; i++)
        {
            string[] data = _data[i];
            string line = data[(int)EDialogueAttributes.LINE_TEXT];
            List<NPCDialogue> dials = StoryManager.String2Dialogues(data[(int)EDialogueAttributes.RESULT_DIALOGUES]);
            DialQuest quest = String2Quest(data[(int)EDialogueAttributes.RESULT_QUEST], data[(int)EDialogueAttributes.QUEST_FUNCTION]);
            Lines[i] = new(line, dials, quest);
        }
        string[] firstData = _data[0];
        int.TryParse(firstData[(int)EDialogueAttributes.DIALOGUE_IDX], out DialogueIdx);
        int.TryParse(firstData[(int)EDialogueAttributes.BRANCH_IDX], out BranchIdx);
        IsOpenAtFirst = true;
    }
}
