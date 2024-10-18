using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCScriptable : ScriptableObject
{
    public uint Idx;
    public SNPC NPC;
    public string NPCName;
    public List<DialogueScriptable> DialogueList;
    public List<QuestScriptable> QuestList;
    [TextArea]
    public string DefaultLine;


    public void AddDialogue(DialogueScriptable _dial) { DialogueList.Add(_dial); }
    public void AddQuest(QuestScriptable _quest) { QuestList.Add(_quest); }

    public virtual void SetNPCScriptable(uint _idx, string[] _data)
    {
        Idx =           _idx;     
        NPC =           StoryManager.String2NPC(_data[(int)ENPCAttribute.SNPC]);
        NPCName =       _data[(int)ENPCAttribute.NAME];

        DialogueList = new();
        QuestList = new();
        DefaultLine =   _data[(int)ENPCAttribute.DEFAULT_LINE];
    }
}
