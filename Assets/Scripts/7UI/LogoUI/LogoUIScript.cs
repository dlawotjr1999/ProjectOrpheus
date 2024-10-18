using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoUIScript : MonoBehaviour
{
    private bool IsLogoDone { get; set; }

    private void ToTitle()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(ValueDefine.TITLE_SCENE_IDX);
        oper.allowSceneActivation = false;
        StartCoroutine(WaitLogo(oper));
    }
    private IEnumerator WaitLogo(AsyncOperation _oper)
    {
        while (!IsLogoDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _oper.allowSceneActivation = true;
    }
    private void DoneLogo()
    {
        IsLogoDone = true;
    }

    private void Start()
    {
        ToTitle();
    }
}
