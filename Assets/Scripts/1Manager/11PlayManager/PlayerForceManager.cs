using UnityEngine;

public class PlayerForceManager : MonoBehaviour, IHaveData
{
    // 플레이어 능력치
    private PlayerStatInfo PlayerStatInfo { get { return PlayManager.PlayerStatInfo; } }

    public readonly static int InitialStatPoint = 2;
    public int LeftStatPoint { get; private set; }
    public int UsedStatPoint { get; private set; }


    public void AddStatPoint(int _add)
    {
        LeftStatPoint += _add;
        PlayManager.UpdateInfoUI();
    }


    public void UpgradeStat(int[] _point)                   // 포인트 투자로 인한 업그레이드
    {
        for (int i = 0; i<(int)EStatName.LAST; i++)
        {
            if (_point[i] > 0)
            {
                UpgradeStat((EStatName)i, _point[i], false);
                LeftStatPoint -= _point[i];
                UsedStatPoint += _point[i];
            }
        }
        PlayManager.ApplyPlayerStat();
        if (LeftStatPoint < 0)
            Debug.LogError("스탯 오버 사용");
    }
    public void UpgradeStat(EStatName _stat, int _amount, bool _isReward)
    {
        PlayerStatInfo.UpgradeStat(_stat, _amount);
        if (_isReward) { PlayManager.AddIngameAlarm($"{Stat2String(_stat)} {_amount} 증가"); }
    }

    public void ResetStat()
    {
        LeftStatPoint += UsedStatPoint;
        UsedStatPoint = 0;

        PlayManager.ApplyStatReset();
    }


    // 플레이어 권능
    private readonly bool[] m_powerObtained = new bool[(int)EPowerName.LAST];
    public bool[] PowerObtained { get { return m_powerObtained; } }


    private readonly EPowerName[] m_powerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT] { EPowerName.LAST, EPowerName.LAST, EPowerName.LAST };
    public EPowerName[] PowerSlot { get { return m_powerSlot; } }

    public void RegisterPowerSlot(EPowerName _skill, int _idx) { m_powerSlot[_idx] = _skill; }
    public void ObtainPower(EPowerName _skill)
    {
        int idx = (int)_skill;
        if (PowerObtained[idx]) { Debug.Log("이미 획득한 스킬"); return; }
        PowerObtained[idx] = true;
    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { LeftStatPoint = InitialStatPoint; return; }

        SaveData data = PlayManager.CurSaveData;

        LeftStatPoint = data.LeftStatPoint;
        UsedStatPoint = data.UsedStatPoint;

        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            m_powerSlot[i] = data.PowerSlot[i];
        }
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            m_powerObtained[i] = data.PowerObtained[i];
        }
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.LeftStatPoint = LeftStatPoint;
        data.UsedStatPoint = UsedStatPoint;

        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            data.PowerSlot[i] = m_powerSlot[i];
        }
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            data.PowerObtained[i] = m_powerObtained[i];
        }
    }

    private readonly static string[] StatString = new string[(int)EStatName.LAST]
        { "HEALTH", "ENDURE", "STRENGTH", "INTELLECT", "RAPID", "MENTAL" };

    public static EStatName String2Stat(string _data)
    {
        for(int i = 0; i<(int)EStatName.LAST; i++) { if(_data == StatString[i]) { return (EStatName)i; } }
        return EStatName.LAST;
    }
    public static string Stat2String(EStatName _stat)
    {
        return StatString[(int)_stat];
    }


    public void SetManager()
    {
        LoadData();
    }
}
