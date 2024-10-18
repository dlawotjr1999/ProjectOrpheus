using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EAlarmType
{
    SIDE_TEXT,              // 화면 사이드 텍스트 알람
    LAST
}

public class UIManager : MonoBehaviour
{
    // UI와 알람 차이점 -> UI는 조작이나 상호작용 가능, 알람은 한번 띄우고 끝 (사실 아직 정확하지 않지만 종류가 많아지면 복잡해질 거 같아서 나눠둠)

    [SerializeField]
    private GameObject[] m_alarmPrefabs = new GameObject[(int)EAlarmType.LAST];                     // 알람 프리펍들
    public GameObject GetAlarmPrefab(EAlarmType _alarm) { return m_alarmPrefabs[(int)_alarm]; }


    public Sprite GetMonsterProfile(EMonsterName _monster)
    {
        return GameManager.GetMonsterData(_monster).MonsterProfile;
    }
    public Sprite GetMonsterBodyImg(EMonsterName _monster)
    {
        return GameManager.GetMonsterData(_monster).MonsterBodyImg;
    }

    public Sprite GetItemSprite(SItem _item)
    {
        return GameManager.GetItemData(_item).ItemImage;
    }

    public Sprite GetPowerSprite(EPowerName _skill)
    {
        return GameManager.GetPowerData(_skill).PowerIcon;
    }
}