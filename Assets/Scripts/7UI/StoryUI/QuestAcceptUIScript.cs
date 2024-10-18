using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAcceptUIScript : BaseUI
{
    // 퀘스트 정보
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    [SerializeField]
    private Button m_acceptFinishBtn;
    // private Button m_btn2;

    private TextMeshProUGUI m_btn1Txt;       // 수락, 완료 버튼
    // private TextMeshProUGUI m_btn2Txt;      // 거절, 확인 버튼

    private QuestScriptable CurQuest { get; set; }
    private bool IsStart { get; set; }
    private FPointer ConfirmFunction { get; set; }

    public void ShowNPCQuestUI(EQuestName _quest, bool _isStart, FPointer _confirm)
    {
        CurQuest = GameManager.GetQeustData(_quest);
        IsStart = _isStart;
        ConfirmFunction = _confirm;
        base.OpenUI();
    }

    public override void UpdateUI()
    {
        m_questTitle.text = CurQuest.Name;
        m_questDescription.text = CurQuest.Description;
        m_questRewards.text = $"보상 {CurQuest.Reward.Type} {CurQuest.Reward.Amount}개";
        SetBtnsTxts(IsStart);
    }


    public void SetBtnsTxts(bool _isStart)
    {
        m_btn1Txt.text = _isStart ? "수락" : "완료";
        // m_btn2Txt.text = _isStart ? "거절" : "확인";
    }

    private void AcceptOrFinish()
    {
        if (IsStart)
        {
            AcceptQuest();
        }
        else
        {
            FinishQuest();
        }
        ConfirmFunction();
        GameManager.PlaySE(ESystemSE.BTN_CLICK);
        CloseUI();
    }
    private void AcceptQuest()
    {
        PlayManager.SetQuestStatus(CurQuest.Enum, EQuestState.ACCEPTED);
        PlayManager.UpdateQuestSidebar();
    }
    private void FinishQuest()
    {
        PlayManager.SetQuestStatus(CurQuest.Enum, EQuestState.FINISH);
        PlayManager.UpdateQuestSidebar();
    }

/*    private void Btn2Function()
    {
        ConfirmFunction();
        GameManager.PlaySE(ESystemSE.BTN_CLICK);
        CloseUI();
    }*/


    private void SetBtns()
    {
        m_acceptFinishBtn.onClick.AddListener(AcceptOrFinish);       // 수락, 완료 버튼
        // m_btn2.onClick.AddListener(Btn2Function);       // 거절, 확인 버튼
    }

    public override void SetComps()
    {
        base.SetComps();
        m_btn1Txt = m_acceptFinishBtn.GetComponentInChildren<TextMeshProUGUI>();
        // m_btn2Txt = m_btn2.GetComponentInChildren<TextMeshProUGUI>();
        SetBtns();
    }
}
