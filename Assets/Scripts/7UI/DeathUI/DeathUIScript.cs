using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIScript : BaseUI
{
    [SerializeField]
    private Button m_restartBtn;

    public override void OpenUI()
    {
        base.OpenUI();
        StartCoroutine(PlayDeathSound());
    }
    private IEnumerator PlayDeathSound()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.PlaySE(ESystemSE.GAME_OVER);
    }
    private void RestartGame()
    {
        PlayManager.RestartGame();
    }


    private void SetBtns()
    {
        m_restartBtn.onClick.AddListener(RestartGame);
    }
    public override void SetComps()
    {
        base.SetComps();
        SetBtns();
    }
}
