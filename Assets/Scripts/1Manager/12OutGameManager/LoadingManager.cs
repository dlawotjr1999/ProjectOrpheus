using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private float m_minLoadingTime = 3;
    [SerializeField]
    private Slider m_loadingBar;
    [SerializeField]
    private GameObject m_clickTxt;

    private PlayerInput m_offgameInput;
    private bool Anykey { get { return m_offgameInput.actions["AnyKey"].triggered; } }

    private void LoadPlayScene()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(ValueDefine.HELL_SCENE_IDX);
        oper.allowSceneActivation = false;
        StartCoroutine(LoadingCoroutine(oper));
    }
    private IEnumerator LoadingCoroutine(AsyncOperation _oper)
    {
        float time = m_minLoadingTime;
        while (_oper.progress < 0.9f || time > m_minLoadingTime * 0.9f)
        {
            time -= Time.deltaTime;
            SetLoadingBar(FunctionDefine.Min(_oper.progress, (m_minLoadingTime - time) / m_minLoadingTime));
            yield return null;
        }
        while (time > 0)
        {
            time -= Time.deltaTime;
            SetLoadingBar((m_minLoadingTime - time) / m_minLoadingTime);
            yield return null;
        }
        m_clickTxt.GetComponent<Animator>().SetTrigger("ON");
        while (!Anykey)
        {
            yield return null;
        }
        _oper.allowSceneActivation = true;
    }

    private void SetLoadingBar(float _prog)
    {
        m_loadingBar.value = _prog;
    }


    private void Awake()
    {
        m_offgameInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        LoadPlayScene();
    }
}
