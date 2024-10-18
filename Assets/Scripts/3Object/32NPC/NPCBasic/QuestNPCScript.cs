using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCScript : NPCScript
{
    public List<QuestScriptable> QuestList { get { return m_scriptable.QuestList; } }

}
