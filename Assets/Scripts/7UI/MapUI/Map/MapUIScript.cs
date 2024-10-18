using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StylizedWater2;
using System;
using Pathfinding;

public class MapUIScript : MinimapScript
{
    [SerializeField]
    private RectTransform m_mapImage;

    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        SetComps();
    }

    public void CloseUI() 
    { 
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        gameObject.SetActive(false);
    }

    private void SetComps()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
