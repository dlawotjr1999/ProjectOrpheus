using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUIScript : MonoBehaviour
{
    private StateUIScript m_stateUI;
    private RaycastUIScript m_raycastUI;

    public void SetStaminaRate(float _rate)
    {
        m_stateUI.SetStaminaRate(_rate);
    }
    public void SetLightRate(float _rate)
    {
        m_stateUI.SetLightRate(_rate);
    }
    public void SetLightState(bool _on)
    {
        m_stateUI.SetLightState(_on);
    }


    public void ShowAimUI()
    {
        m_raycastUI.ShowAim();
        SetAimUI(false);
    }
    public void SetAimUI(bool _on)
    {
        m_raycastUI.SetAimState(_on);
    }
    public void HideAimUI()
    {
        m_raycastUI.HideAim();
    }


    private void SetComps()
    {
        m_stateUI = GetComponentInChildren<StateUIScript>();
        m_stateUI.SetComps();
        m_raycastUI = GetComponentInChildren<RaycastUIScript>();
        m_raycastUI.SetComps();
    }

    private void Awake()
    {
        SetComps();
    }
}
