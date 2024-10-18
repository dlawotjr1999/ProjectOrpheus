using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatInfoUIScript : MonoBehaviour
{
    private StatBoxUIScript m_parent;
    public void SetParent(StatBoxUIScript _parent) { m_parent = _parent; SetComps(); }

    [SerializeField]
    private GameObject m_statNumObj;

    private TextMeshProUGUI[] m_statNums;

    private PlayerStatInfo PlayerStat { get; set; }

    public void InitStats(PlayerStatInfo _info)
    {
        PlayerStat = _info;
        SetStatNums();
    }


    public void UpdateUsingPoint(int[] _point)
    {
        PlayerStatInfo newInfo = new(PlayerStat, _point);
        SetStatNums(newInfo);
    }
    public void ResetUsingPoint()
    {
        SetStatNums();
    }


    private void SetStatNums()
    {
        PlayerCombatInfo info = new(PlayerStat);
        for (int i = 0; i<(int)ECombatInfoName.LAST; i++)
        {
            ECombatInfoName name = (ECombatInfoName)i;
            float stat = info.GetStat(name);
            string statInfo = stat.ToString();
            if (name == ECombatInfoName.ATTACK)
            {
                FRange attack = PlayManager.PlayerWeaponInfo.WeaponAttack;
                statInfo = $"{stat + attack.Min}-{stat + attack.Max}";
            }
            else if (name == ECombatInfoName.MAGIC)
            {
                FRange magic = PlayManager.PlayerWeaponInfo.WeaponMagic;
                statInfo = $"{stat + magic.Min}-{stat + magic.Max}";
            }
            m_statNums[i].text = statInfo;
        }
    }
    private void SetStatNums(PlayerStatInfo _using)
    {
        PlayerCombatInfo ori = new(PlayerStat);
        PlayerCombatInfo use = new(_using);
        for (int i = 0; i<(int)ECombatInfoName.LAST; i++)
        {
            ECombatInfoName name = (ECombatInfoName)i;
            float stat = use.GetStat(name);
            float gap = FunctionDefine.RoundF3(use.GetStat(name) - ori.GetStat(name));

            string statInfo = stat.ToString();
            if(name == ECombatInfoName.ATTACK) 
            {
                FRange attack = PlayManager.PlayerWeaponInfo.WeaponAttack;
                statInfo = $"{stat + attack.Min}-{stat + attack.Max}"; 
            }
            else if (name == ECombatInfoName.MAGIC)
            {
                FRange magic = PlayManager.PlayerWeaponInfo.WeaponMagic;
                statInfo = $"{stat + magic.Min}-{stat + magic.Max}";
            }
            string num;
            if (gap == 0) { num = statInfo.ToString(); }
            else { num = $"{statInfo} (+{gap})"; }
            m_statNums[i].text = num;
        }
    }


    private void SetComps()
    {
        m_statNums = m_statNumObj.GetComponentsInChildren<TextMeshProUGUI>();
    }
}
