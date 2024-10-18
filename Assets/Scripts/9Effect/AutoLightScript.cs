using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLightScript : MonoBehaviour
{
    [SerializeField]
    private float m_lastTime = 5;
    [SerializeField]
    private float m_maxLight = 3;
    private Light m_light;



    private IEnumerator LightChange(float _delay, float _change)
    {
        bool isOn = _change > 0;
        yield return new WaitForSeconds(_delay);
        while ((isOn && m_light.intensity < m_maxLight) || 
            (!isOn && m_light.intensity > 0))
        {
            m_light.intensity += _change * Time.deltaTime;
            yield return null;
        }
        if (isOn) { m_light.intensity = m_maxLight; }
        else { m_light.intensity = 0; }
    }


    private void OnEnable()
    {
        m_light.intensity = 0;
        float change = m_maxLight / (m_lastTime / 4);
        float delay = m_lastTime * 3 / 4;
        StartCoroutine(LightChange(0, change));
        StartCoroutine(LightChange(delay, -change));
    }
    private void Awake()
    {
        m_light = GetComponentInChildren<Light>();
    }
}
