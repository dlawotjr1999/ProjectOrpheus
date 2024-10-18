using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerUIScript : MonoBehaviour
{
    private RectTransform m_rect;
    private Canvas m_uiCanvas;

    private PlayerImgUIScript m_imgUI;                      // 플레이어 이미지 UI
    private PlayerInfoUIScript m_infoUI;                    // 플레이어 정보(아이템, 특성 등) UI
    private PlayerMaterialUIScript m_materialUI;            // 플레이어 재료 UI
    [SerializeField]
    private ItemInfoUIScript m_itemInfoUI;                  // 아이템 정보 팝업 UI

    private bool Opened { get; set; }                       // 열린 적 있는지 (처음 열리는지 확인용)

    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        if(!Opened) { SetComps(); }
        m_imgUI.OpenUI();
        m_infoUI.OpenUI();
        m_materialUI.OpenUI();
        GameManager.PlaySE(ESystemSE.OPEN_UI);
    }

    public void UpdateInfoUI()
    {
        m_imgUI.UpdateUI();
        m_infoUI.UpdateUI();
    }
    public void InfoBoxSet(EPlayerInfoType _type)
    {
        bool show = _type != EPlayerInfoType.MONSTER;
        if (m_imgUI.gameObject.activeSelf != show) { m_imgUI.gameObject.SetActive(show); }
    }

    public void UpdateMaterials()                           // 재화 업데이트
    {
        if(!Opened) { SetComps(); }
        m_materialUI.UpdateMaterials();
    }

    public void UpdatePlayerModelWeapon()
    {
        m_imgUI.UpdatePlayerWeapon(PlayManager.CurWeapon);
    }


    public void ShowItemInfoUI(SItem _item)
    {
        m_itemInfoUI.ShowUI(_item);
    }
    public void SetItemInfoUIPos(Vector2 _pos)
    {
        Vector2 pos = _pos - new Vector2(DisplayManager.CANVAS_WIDTH * 0.5f, DisplayManager.CANVAS_HEIGHT * 0.5f);
        m_itemInfoUI.SetPos(pos);
    }
    public void HideItemInfoUI()
    {
        m_itemInfoUI.HideUI();
    }
    public void CloseUI() { GameManager.SetControlMode(EControlMode.THIRD_PERSON); gameObject.SetActive(false); }      // 닫기


    private void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        m_uiCanvas = GetComponentInParent<Canvas>();

        m_imgUI = GetComponentInChildren<PlayerImgUIScript>();
        m_infoUI = GetComponentInChildren<PlayerInfoUIScript>();
        m_materialUI = GetComponentInChildren<PlayerMaterialUIScript>();
        m_materialUI.SetComps();

        m_imgUI.SetParent(this);
        m_imgUI.SetComps();
        m_infoUI.SetParent(this);
        m_infoUI.SetComps();

        Opened = true;
    }

    private void Awake()
    {
        GameManager.UIControlInputs.ClosePlayerUI.started += delegate { CloseUI(); };
    }

    private void Start()
    {
        if(!Opened) { SetComps(); OpenUI(); }
    }
}
