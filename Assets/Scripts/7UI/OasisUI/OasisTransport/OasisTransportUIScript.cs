using UnityEngine;
using UnityEngine.UI;

public class OasisTransportUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;

    private bool IsCompsSet { get; set; }

    public void OpenUI(OasisUIScript _parent) 
    {
        gameObject.SetActive(true);
        if (!IsCompsSet)
        {
            m_parent = _parent;
            SetComps();
        }
    }

    [SerializeField]
    private RectTransform m_mapImg;
    [SerializeField]
    private Button m_transportBtn;
    [SerializeField]
    private Button m_cancelBtn;
    private OasisPointUIScript[] m_oasisPoints;

    public EOasisName CurOasisName { get { return m_parent.Oasis.PointName; } }
    private EOasisName CurDestination { get; set; } = EOasisName.LAST;


    public void SetDestination(EOasisName _point)
    {
        if(_point == m_parent.Oasis.PointName) { return; }


        if(CurDestination != EOasisName.LAST)
        {
            m_oasisPoints[(int)CurDestination].ResetDestination();
        }
        else
        {
            m_transportBtn.interactable = true;
        }
        CurDestination = _point;
    }
    
    private void TransportTo()
    {
        if(CurDestination == EOasisName.LAST) { return; }
        CloseUI();
        MoveToOasis(CurDestination);
    }

    public void MoveToOasis(EOasisName _point)
    {
        if (CurOasisName == _point)
        {
            Debug.Log("현재 위치한 오아시스입니다!");
            return;
        }
        else
        {
            PlayManager.TransportToOasis(_point);
            CloseUI();
        }
    }

    private void CancelUI()
    {
        CloseUI();
    }

    public void CloseUI()
    {
        m_parent.FunctionDone();
        gameObject.SetActive(false);
        m_parent.CloseUI();
    }
    

    private void SetBtns()
    {
        m_transportBtn.onClick.AddListener(TransportTo);
        m_cancelBtn.onClick.AddListener(CancelUI);
    }
    private void SetComps()
    {
        m_oasisPoints = GetComponentsInChildren<OasisPointUIScript>();
        SetBtns();
        for (int i = 0; i<m_oasisPoints.Length; i++)
        {
            if (i >= (int)EOasisName.LAST) { m_oasisPoints[i].gameObject.SetActive(false); continue; }
            m_oasisPoints[i].SetParent(this);
            m_oasisPoints[i].SetComps((EOasisName)i, m_mapImg);
        }
        IsCompsSet = true;
    }
}
