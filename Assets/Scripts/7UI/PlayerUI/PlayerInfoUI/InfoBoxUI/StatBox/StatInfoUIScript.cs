using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EStatBtnState
{
    IDLE,
    ABLE,
    USED
}


public class StatInfoUIScript : MonoBehaviour
{
    private StatBoxUIScript m_parent;
    public void SetParent(StatBoxUIScript _parent) { m_parent = _parent; SetComps(); }

    private StatCtrlBtnScript[] m_statBtns;
    private TextMeshProUGUI[] m_statNums;
    [SerializeField]
    private GameObject m_leftPointObj;

    private TextMeshProUGUI m_leftPointTxt;
    private Image m_leftPointImg;
    private Button m_confirmBtn;
    private Button m_cancelBtn;

    private PlayerStatInfo PlayerStat { get; set; }
    private int StatPoint { get; set; }

    private readonly int[] m_usingPoints = new int[(int)EStatName.LAST];
    private int UsingPoint { get { int point = 0; for (int i = 0; i<(int)EStatName.LAST; i++) { point += m_usingPoints[i]; } return point; } }

    public void InitStats(PlayerStatInfo _info)
    {
        PlayerStat = _info;
        SetStatNums(PlayerStat);
        ClearUsingPoint();
    }


    private void SetCurStat()
    {
        SetStatNums(PlayerStat, m_usingPoints);
    }


    private void SetPointUsing(int _use)
    {
        StatPoint = PlayManager.LeftStatPoint - _use;
        m_leftPointTxt.text = StatPoint.ToString();
        if (StatPoint > 0) { AblePointUse(); }
        else { DisablePointUse(); }
        if (UsingPoint > 0) { PointUsing(); }
        else { CancelUsing(); }
    }

    private void AblePointUse()
    {
        m_leftPointImg.color = Color.green;
    }
    private void DisablePointUse()
    {
        m_leftPointImg.color = Color.white;
    }

    private void PointUsing()
    {
        m_confirmBtn.interactable = true;
        m_cancelBtn.interactable = true;
    }
    private void CancelUsing()
    {
        m_confirmBtn.interactable = false;
        m_cancelBtn.interactable = false;
    }

    private void ConfirmStatUse()
    {
        PlayManager.UpgradeStat(m_usingPoints);
        m_parent.InitUI();
    }
    private void ResetPoint()
    {
        ClearUsingPoint();
        SetStatNums(PlayerStat);
        m_parent.ResetStatUse();
    }
    private void ClearUsingPoint()
    {
        for (int i = 0; i<(int)EStatName.LAST; i++) { m_usingPoints[i] = 0; }
        SetPointUsing(0);
    }


    public void SetStatNums(PlayerStatInfo _info)
    {
        int[] noUse = new int[(int)EStatName.LAST];
        SetStatNums(_info, noUse);
    }
    public void SetStatNums(PlayerStatInfo _info, int[] _use)
    {
        for (int i = 0; i<(int)EStatName.LAST; i++)
        {
            string num;
            float stat = _info.GetStat((EStatName)i);
            if (_use[i] == 0)
            {
                num = stat.ToString();
            }
            else
            {
                num = $"{stat + _use[i]} (+{_use[i]})";
            }
            m_statNums[i].text = num;
        }
    }

    public void ChkNUpgradeStat(EStatName _stat, bool _up)
    {
        if (_up)
        {
            if (StatPoint <= 0) { return; }
            m_usingPoints[(int)_stat]++;
            UpdateStatUse();
        }
        else
        {
            if (m_usingPoints[(int)_stat] <= 0) { return; }
            m_usingPoints[(int)_stat]--;
            UpdateStatUse();
        }
    }

    private void UpdateStatUse()
    {
        SetCurStat();
        m_parent.UpdateStatUse(m_usingPoints);
        SetPointUsing(UsingPoint);
    }


    private void SetBtns()
    {
        m_confirmBtn.onClick.AddListener(ConfirmStatUse);
        m_cancelBtn.onClick.AddListener(ResetPoint);
    }
    private void SetComps()
    {
        m_statBtns = GetComponentsInChildren<StatCtrlBtnScript>();
        int len = m_statBtns.Length;
        if(len != (int)EStatName.LAST) { Debug.LogError("버튼 개수 다름"); return; }
        m_statNums = new TextMeshProUGUI[len];
        for(int i=0;i<len;i++) 
        {
            m_statNums[i] = m_statBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            m_statBtns[i].SetComps(this, (EStatName)i);
        }

        m_leftPointTxt = m_leftPointObj.GetComponentInChildren<TextMeshProUGUI>();
        m_leftPointImg = m_leftPointObj.GetComponent<Image>();

        Button[] btns = GetComponentsInChildren<Button>();
        m_confirmBtn = btns[0];
        m_cancelBtn = btns[1];
        SetBtns();
    }
}
