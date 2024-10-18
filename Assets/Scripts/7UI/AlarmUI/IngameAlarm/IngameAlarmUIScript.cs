using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class IngameAlarmUIScript : MonoBehaviour
{
    private readonly int MaxAlarm = 16;

    private ObjectPool<GameObject> m_alarmPool;
    private GameObject OnPoolCreate()
    {
        GameObject item = Instantiate(m_alarmElm);
        item.GetComponent<IPoolable>().SetPool(m_alarmPool);
        item.GetComponent<IngameAlarmElmScript>().SetParent(this);
        return item;
    }
    private void OnPoolGet(GameObject _item)
    {
        _item.SetActive(true);
        _item.transform.SetParent(null);
        _item.GetComponent<IPoolable>().OnPoolGet();
    }
    private void OnPoolRelease(GameObject _item)
    {
        _item.transform.SetParent(m_listTrans);
        _item.SetActive(false);
    }
    private void OnPoolDestroy(GameObject _item) { Destroy(_item); }


    [SerializeField]
    private GameObject m_alarmElm;
    [SerializeField]
    private Transform m_boxTrans;
    [SerializeField]
    private Transform m_listTrans;

    public readonly static float ElmHeight = 72;

    private readonly List<IngameAlarmElmScript> m_alarms = new();

    public void AddAlarm(string _alarm)
    {
        foreach (IngameAlarmElmScript showing in m_alarms) { showing.PlusIdx(); }
        GameObject alarm = m_alarmPool.Get();
        alarm.transform.SetParent(m_boxTrans);
        IngameAlarmElmScript elm = alarm.GetComponent<IngameAlarmElmScript>();
        elm.AlarmOn(_alarm);
        m_alarms.Add(elm);
    }

    public void AlarmDestroyed(IngameAlarmElmScript _alarm)
    {
        m_alarms.Remove(_alarm);
    }


    private void SetComps()
    {
        m_alarmPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, MaxAlarm, MaxAlarm);
        for (int i = 0; i<MaxAlarm; i++)
        {
            GameObject newItem = OnPoolCreate();
            newItem.GetComponent<IPoolable>().OriginalPool.Release(newItem);
        }
    }
    private void Awake()
    {
        SetComps();
    }
}
