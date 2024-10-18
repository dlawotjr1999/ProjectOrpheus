using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadListElm : MonoBehaviour
{
    private LoadListScript m_parent;
    public void SetParent(LoadListScript _parent) { m_parent = _parent; }

    private Button m_btn;
    private Image m_img;
    private TextMeshProUGUI m_slotTxt;

    private readonly Color SlotColor = new(212/255f, 238/255f, 236/255f);
    private readonly Color EmptyColor = new(241/255f, 226/255f, 149/255f);

    private int LoadingIdx { get; set; }


    public void LoadData(int _idx, SaveData _data)
    {
        LoadingIdx = _idx;
        m_btn.interactable = true;
        m_img.color = SlotColor;
        m_slotTxt.text = _data.SavedTime;
    }
    public void EmptySlot(int _idx)
    {
        LoadingIdx = _idx;
        m_btn.interactable = false;
        m_img.color = EmptyColor;
        m_slotTxt.text = "ºó ½½·Ô";
    }
    private void LoadGame()
    {
        if(LoadingIdx == -1) { return; }

        m_parent.LoadGame(LoadingIdx);
    }


    private void SetBtns()
    {
        m_btn.onClick.AddListener(LoadGame);
    }

    public void SetComps()
    {
        m_btn = GetComponent<Button>();
        m_img = GetComponent<Image>();
        m_slotTxt = GetComponentInChildren<TextMeshProUGUI>();
        SetBtns();
    }
}
