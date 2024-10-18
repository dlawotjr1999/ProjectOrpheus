using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoxElmScript : MonoBehaviour
{
    private PowerBoxUIScript m_parent;
    public void SetParent(PowerBoxUIScript _parent) { m_parent = _parent; }
    public PowerBoxUIScript Box { get { return m_parent; } }

    private readonly Color MissingColor = new(87/255f, 87/255f, 87/255f);

    private Image m_skillImg;
    private TextMeshProUGUI m_skillName;

    private PowerBoxDragScript m_drag;

    public EPowerName CurPower { get; private set; }
    public bool IsObtained { get; private set; }
    public bool IsEquipped { get; private set; }

    public void SetSkillInfo(EPowerName _skill, bool _equipped)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
        CurPower = _skill;
        Sprite img = GameManager.GetPowerSprite(_skill);
        PowerInfo info = GameManager.GetPowerInfo(_skill);
        IsObtained = PlayManager.PowerObtained[(int)_skill];
        IsEquipped = _equipped;

        m_skillImg.sprite = img;
        m_skillImg.color = IsObtained ? Color.white : MissingColor;
        m_skillName.text = info.PowerName;
        if(IsEquipped) { m_skillName.color = Color.red; }
        else { m_skillName.color = Color.black; }
    }


    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    private void SetComps()
    {
        m_skillImg = GetComponentsInChildren<Image>()[1];
        m_skillName = GetComponentInChildren<TextMeshProUGUI>();
        m_drag = GetComponentInChildren<PowerBoxDragScript>();
        m_drag.SetParent(this);
    }

    private void Awake()
    {
        SetComps();
    }
}
