using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBoxElmScript : MonoBehaviour
{
    private WeaponBoxUIScript m_parent;
    public void SetParent(WeaponBoxUIScript _parent) { m_parent = _parent; }
    private Transform ParentTrans { get { return m_parent.ParentTrans; } }

    private WeaponImgScript m_img;

    private EWeaponName ElmWeapon { get; set; }     // 할당된 무기
    private bool IsCurWeapon { get; set; }          // 플레이어가 장착 중인 무기인지

    private Image m_weaponImg;                      // 무기 이미지      
    private Button m_equipBtn;                      // 장착 버튼

    private TextMeshProUGUI m_weaponNameTxt;         // 무기 이름

    public void SetWeaponInfo(int _weapon)          // 정보 설정
    {
        EWeaponName weapon = (EWeaponName)_weapon;
        ElmWeapon = weapon;

        SItem item = new(EItemType.WEAPON, _weapon);

        Sprite img = GameManager.GetItemSprite(item);
        ItemInfo info = GameManager.GetWeaponInfo(weapon);

        m_weaponImg.sprite = img;
        m_weaponNameTxt.text = info.ItemName;
        bool obtained = info.Obtained;
        IsCurWeapon = weapon == PlayManager.CurWeapon;
        SetBtnState(obtained);
    }

    private void SetBtnState(bool _obtained)        // 버튼 상태 설정
    {
        if (IsCurWeapon) { SetBtnTxt("장착 중"); }
        else { SetBtnTxt("장착"); }

        if (_obtained) { m_equipBtn.interactable = true; }
        else { m_equipBtn.interactable = false; }
    }
    private void SetBtnTxt(string _txt)             // 버튼 텍스트 설정
    {
        m_equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = _txt;
    }

    public void HideElm()                           // 숨기기
    {
        gameObject.SetActive(false);
    }


    public void ShowInfo()
    {
        m_parent.ShowInfoUI(ElmWeapon);
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoUIPos(_pos);
    }
    public void HideInfo()
    {
        m_parent.HideInfoUI();
    }


    private void EquipWeapon()                      // 할당된 무기 장착
    {
        if(IsCurWeapon) { return; }
        m_parent.EquipWeapon(ElmWeapon);
    }


    private void SetBtn()
    {
        m_equipBtn.onClick.AddListener(EquipWeapon);
    }

    private void SetComps()
    {
        m_weaponImg = GetComponentsInChildren<Image>()[1];
        m_weaponNameTxt = GetComponentsInChildren<TextMeshProUGUI>()[0];
        m_equipBtn = GetComponentInChildren<Button>();
        m_img = GetComponentInChildren<WeaponImgScript>();
        m_img.SetParent(this);
        m_img.SetComps();
        SetBtn();
    }

    private void Awake()
    {
        SetComps();
    }
}
