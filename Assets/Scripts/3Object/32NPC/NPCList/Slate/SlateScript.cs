using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlateScript : NPCScript
{
    private Animator m_anim;

    [SerializeField]
    private ESlateName m_slateName;

    [SerializeField]
    private TextMeshProUGUI m_slateUI;

    [SerializeField]
    private float m_displayTime = 5;
    public float DisplayTime { get { return m_displayTime; } }

    public string SlateText { get { return m_scriptable.DefaultLine; } }

    private bool ShowingSlateText { get; set; }

    public override string InfoTxt => "읽기";

    public void SetSlate(uint _idx, NPCScriptable _scriptable)
    {
        m_slateName = (ESlateName)_idx;
        SetScriptable(_scriptable);
    }

    public override bool CanInteract => !ShowingSlateText;

    public override void StartInteract()
    {
        PlayManager.StopPlayerInteract();
        if(m_scriptable == null || ShowingSlateText) { return; }
        ShowingSlateText = true;
        m_anim.SetBool("IS_SHOWING", true);
        StartCoroutine(DisplayCoroutine());
    }
    private IEnumerator DisplayCoroutine()
    {
        yield return new WaitForSeconds(DisplayTime);
        m_anim.SetBool("IS_SHOWING", false);
    }
    public void DisplayDone()
    {
        PlayManager.StopPlayerInteract(InteractManager);
        ShowingSlateText = false;
    }


    public override void LoadData() { }
    public override void SaveData() { }

    private void SetComps()
    {
        m_anim = GetComponent<Animator>();
        m_slateUI.text = SlateText;
    }
    private void Awake()
    {
        SetComps();
    }
}
