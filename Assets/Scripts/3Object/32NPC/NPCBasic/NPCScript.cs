using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueInfo
{
    public bool IsUnlocked;
    public bool IsDone;
    public bool CanProcess { get { return IsUnlocked && !IsDone; } }
    public static DialogueInfo Init { get { return new(false, false); } }
    public DialogueInfo(bool _unlokced, bool _done) { IsUnlocked = _unlokced; IsDone = _done; }
    public DialogueInfo(DialogueInfo _other) { IsUnlocked = _other.IsUnlocked; IsDone = _other.IsDone; }
}

public class NPCScript : MonoBehaviour, IInteractable, IHaveData
{
    [SerializeField]
    protected NPCScriptable m_scriptable;
    public void SetScriptable(NPCScriptable _scriptable) { m_scriptable = _scriptable; }

    public InteractScript InteractManager { get; private set; }
    public void SetInteractScript(InteractScript _script) { InteractManager = _script; }

    public Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }

    public SNPC NPC { get { if (m_scriptable == null) { return SNPC.Null; } return m_scriptable.NPC; } }
    public string NPCName { get { return m_scriptable.NPCName; } }
    public DialLine[] DefaultLine { get { return new DialLine[1] { new(m_scriptable.DefaultLine) }; } }

    public List<DialogueScriptable> DialogueList { get { return m_scriptable.DialogueList; } }
    private int DialCount { get { if (m_scriptable == null) { return 0; } return DialogueList.Count; } }
    protected DialogueInfo[] m_dialInfos;

    private int AbleDialIdx { get { for (int i = 0; i<DialCount; i++) { if (m_dialInfos[i].CanProcess) return i; } return -1; } }

    public virtual bool CanInteract { get { return true; } }

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public virtual string InfoTxt { get { return "대화"; } }            // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 


    public void UnlockDialogue(int _idx)
    {
        m_dialInfos[_idx].IsUnlocked = true;
        Debug.Log($"{NPCName} {_idx+1}번째 대화 잠금 해제!");
    }
    public virtual void StartDialogue()
    {
        PlayManager.OpenDialogueUI(this, AbleDialIdx);
        if (AbleDialIdx < 0) { return; }
        m_dialInfos[AbleDialIdx].IsDone = true; ;
    }


    public virtual void StartInteract()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        NPCInteraction();
    }
    public virtual void NPCInteraction()
    {
        StartDialogue();
    }

    public virtual void StopInteract()
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }


    public virtual void ApplyLoadedData(NPCSaveData _save)
    {
        DialogueInfo[] states = _save.DialInfo;
        for (int i = 0; i<states.Length; i++) { m_dialInfos[i] = new(states[i]); }
    }
    public virtual NPCSaveData ModifiedData()
    {
        return new(NPC, m_dialInfos);
    }

    public virtual void InitNPCData()
    {
        m_dialInfos = new DialogueInfo[DialCount];
    }

    // 기본
    public virtual void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { InitNPCData(); InitDialInfos(); return; }

        SaveData data = PlayManager.CurSaveData;

        foreach (NPCSaveData save in data.NPCData)
        {
            if (save.NPC != NPC) { continue; }
            ApplyLoadedData(save);
            return;
        }
    }

    public virtual void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        foreach (NPCSaveData save in data.NPCData)
        {
            if (save.NPC != NPC) { continue; }
            NPCSaveData modified = ModifiedData();
            data.NPCData.Add(modified);
            return;
        }

        NPCSaveData newSave = new(NPC, m_dialInfos);
        data.NPCData.Add(newSave);
    }


    private void InitDialInfos()
    {
        for (int i = 0; i<DialCount; i++)
        {
            m_dialInfos[i].IsUnlocked = DialogueList[i].IsOpenAtFirst;
        }
    }

    private void SetComps()
    {
        m_dialInfos = new DialogueInfo[DialCount];
        for (int i = 0; i<DialCount; i++)
        {
            m_dialInfos[i] = new(DialogueList[i].IsOpenAtFirst, false);
        }
    }

    private void Awake()
    {
        SetComps();
    }

    private void Start()
    {
        LoadData();
    }
}
