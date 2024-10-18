using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private string DataPath { get; set; }
    private readonly string[] m_xmlPaths = new string[ValueDefine.MAX_SAVE];

    private readonly LinkedList<SaveData> m_gameData = new();

    public List<SaveData> GameData { get { return m_gameData.ToList(); } }
    private SaveData CurData { get { return PlayManager.CurSaveData; } set { PlayManager.SetCurData(value); } }

    private void LoadGameFile()
    {
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
        {
            if (!File.Exists(m_xmlPaths[i])) { continue; }
            var xmlSerializer = new XmlSerializer(typeof(SaveData));

            using FileStream stream = File.OpenRead(m_xmlPaths[i]);
            var saveData = (SaveData)xmlSerializer.Deserialize(stream);
            stream.Close();

            AddGameData(saveData);
        }
    }
    private void SaveGameFile()
    {
        for (int i = 0; i<GameData.Count; i++)
        {
            SaveData data = GameData[i];
            var xmlSerializer = new XmlSerializer(typeof(SaveData));

            using FileStream stream = File.Open(m_xmlPaths[i], FileMode.OpenOrCreate);
            xmlSerializer.Serialize(stream, data);
            stream.Close();
        }
    }

    public void AddGameData(SaveData _data)
    {
        m_gameData.AddFirst(_data);
        if (m_gameData.Count >= ValueDefine.MAX_SAVE) { m_gameData.RemoveLast(); }
        SaveGameFile();
    }
    public void SaveCurData(EOasisName _oasis)
    {
        CurData = new(CurData)
        {
            OasisPoint = _oasis
        };
        CurData.MonsterData.Clear();
        foreach (IHaveData data in m_dataList)
        {
            data.SaveData();
        }

        AddGameData(CurData);
    }
    public void ClearAllData()
    {
        m_dataList.Clear();
    }

    private readonly List<IHaveData> m_dataList = new();
    public void RegisterData(IHaveData _data)
    {
        m_dataList.Add(_data);
    }


    // NPC 데이터
    //public NPCScriptable GetNPCData(EnpcName _npc) { return m_dataList.GetNPCData(_npc); }



    public static EWeaponType IDToWeaponType(string _id)
    {
        return _id[1] switch
        {
            ValueDefine.BLADE_CODE => EWeaponType.BLADE,
            ValueDefine.SWORD_CODE => EWeaponType.SWORD,
            ValueDefine.SCEPTER_CODE => EWeaponType.SCEPTER,
            _ => EWeaponType.LAST
        };
    }
    public static EAdjType String2Adj(string _data)
    {
        return _data switch
        {
            "Damage" => EAdjType.DAMAGE,
            "Attack" => EAdjType.ATTACK,
            "Magic" => EAdjType.MAGIC,
            "MoveSpeed" => EAdjType.MOVE_SPEED,
            "MaxHP" => EAdjType.MAX_HP,
            "WeaponCC" => EAdjType.WEAPON_CC,

            _ => EAdjType.LAST
        };
    }
    public static ECCType String2CC(string _data)
    {
        return _data switch
        {
            "Fatigue" => ECCType.FATIGUE,
            "Melancholy" => ECCType.MELANCHOLY,
            "Extortion" => ECCType.EXTORTION,
            "Airborne" => ECCType.AIRBORNE,
            "Knockback" => ECCType.KNOCKBACK,
            "Weakness" => ECCType.WEAKNESS,
            "Bind" => ECCType.BIND,
            "Void" => ECCType.VOID,
            "Oblivion" => ECCType.OBLIVION,
            "Blind" => ECCType.BLIND,

            _ => ECCType.LAST
        };
    }


    private void InitData()
    {
        DataPath = Application.persistentDataPath + "/GameData/";
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
            m_xmlPaths[i] = $"{DataPath}SaveData{i}.xml";

        if (!Directory.Exists(DataPath)) { Directory.CreateDirectory(DataPath); }
    }
    public void SetManager()
    {
        InitData();
        LoadGameFile();
    }
}