using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogueUI : BaseUI
{
    [SerializeField]
    private TextMeshProUGUI m_nameText;
    [SerializeField]
    private TextMeshProUGUI m_typingText;
    [SerializeField]
    private float m_textSpeed = 0.1f;

    private Button m_btn;

    private NPCScript CurNPC { get; set; }
    private string CurNPCName { get { return CurNPC.NPCName; } }
    private DialogueScriptable CurDialogue { get; set; }
    private DialLine[] CurLines { get { if (CurDialogue == null) { return CurNPC.DefaultLine; } return CurDialogue.Lines; } }
    private int DialogueCount { get; set; } = 0;

    private bool IsQuestConfirmed { get; set; }
    private bool IsButtonClicked { get; set; }


    public override void OpenUI()
    {
        base.OpenUI();
        IsButtonClicked = false;
        DialogueCount = 0;
    }

    public void OpenUI(NPCScript _npc, int _idx)
    {
        OpenUI();
        SetNPC(_npc);
        CurDialogue = _idx >= 0 ? _npc.DialogueList[_idx] : null;
        StartCoroutine(ProcLine(CurLines[DialogueCount]));
    }

    private void SetNPC(NPCScript _npc)
    {
        CurNPC = _npc;

        m_nameText.text = CurNPCName;
    }


    IEnumerator ProcLine(DialLine _line)
    {
        m_typingText.text = "";

        string txt = _line.Text;

        for (int i = 0; i < txt.Length; i++)
        {
            m_typingText.text += txt[i];
            yield return new WaitForSeconds(m_textSpeed);

            if (IsButtonClicked)                            // 좌클릭하면 전체 내용 한 번에 출력
            {
                m_typingText.text = txt;
                IsButtonClicked = false;
                break;
            }
        }
        yield return new WaitForSeconds(0.1f);

        IsButtonClicked = _line.OpenQuestUI;
        LineResult(_line);

        IsQuestConfirmed = !_line.OpenQuestUI;
        while (!IsQuestConfirmed)                                   // 퀘스트 UI 조작 완료 대기
        {
            yield return null;
        }
        while (!IsButtonClicked) { yield return null; }
        NextDialogue();
    }
    private void LineResult(DialLine _line)
    {
        List<NPCDialogue> dials = _line.ResultDialogues;
        foreach (NPCDialogue dial in dials)
        {
            PlayManager.UnlockDialogue(dial);
        }

        if (_line.HasQuest)                                 // 대사별 퀘스트 진행
        {
            DialQuest quest = _line.ResultQuest;
            switch (quest.Function)
            {
                case EDialQuestFunction.START:
                    PlayManager.ShowNPCQuestUI(quest.Quest, true, ConfirmQuest);
                    break;
                case EDialQuestFunction.COMPLETE:
                    CheckQuestComplete(CurNPC.NPC);
                    break;
                case EDialQuestFunction.FNIINSH:
                    PlayManager.ShowNPCQuestUI(quest.Quest, false, ConfirmQuest);
                    break;
                case EDialQuestFunction.COMPLETE_FINISH:
                    CheckQuestComplete(CurNPC.NPC);
                    PlayManager.ShowNPCQuestUI(quest.Quest, false, ConfirmQuest);
                    break;
            }
        }
    }
    private void CheckQuestComplete(SNPC _npc)                      // 대화하기 퀘스트 확인
    {
        List<QuestInfo> infos = PlayManager.QuestInfoList;

        EQuestType questType = EQuestType.TALK;
        if (questType == EQuestType.LAST) { return; }

        foreach (QuestInfo quest in infos)
        {
            if (quest.State != EQuestState.ACCEPTED || quest.QuestContent.Type != questType
                || quest.QuestContent.NPC != _npc) { continue; }

            PlayManager.SetQuestProgress(quest.QuestName, 1);
        }
    }
    private void ConfirmQuest()
    {
        IsQuestConfirmed = true;
    }

    public void ShowAllDialogue()
    {
        IsButtonClicked = true;
    }

    private void NextDialogue()
    {
        DialogueCount++;
        if (DialogueCount == CurLines.Length) { CloseUI(); return; }

        IsButtonClicked = false;
        StartCoroutine(ProcLine(CurLines[DialogueCount]));
    }

    public override void CloseUI()
    {
        CurNPC.StopInteract();
        base.CloseUI();
    }


    public override void SetComps()
    {
        base.SetComps();
        m_btn = GetComponentInChildren<Button>();
        m_btn.onClick.AddListener(ShowAllDialogue);
    }
}