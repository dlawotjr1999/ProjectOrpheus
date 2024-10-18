using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button m_newBtn;
    [SerializeField]
    private Button m_loadBtn;
    [SerializeField]
    private Button m_exitBtn;
    [SerializeField]
    private LoadListScript m_loadingList;


    private void StartGame()
    {
        GameManager.StartGame();
    }

    private void OpenLoad()
    {
        m_loadingList.OpenUI();
    }

    private void SetLoadData()
    {
        List<SaveData> data = GameManager.GameData;
        bool hasData = data.Count > 0;
        m_loadBtn.interactable = hasData;
    }

    private void ExitGame()
    {
        Application.Quit();
    }


    private void SetBtns()
    {
        m_newBtn.onClick.AddListener(StartGame);
        m_loadBtn.onClick.AddListener(OpenLoad);
        m_exitBtn.onClick.AddListener(ExitGame);
    }

    private void SetComps()
    {
        SetBtns();
    }

    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        GameManager.PlayBGM(EBGM.TITLE_BGM);
        SetLoadData();
    }
}
