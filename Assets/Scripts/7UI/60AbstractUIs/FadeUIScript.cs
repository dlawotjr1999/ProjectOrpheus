using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class FadeUIScript : MonoBehaviour              // Fade ����� �ִ� UI
{
    private Image[] m_imgs;                     // ��ü �̹���
    private TextMeshProUGUI[] m_txts;           // ��ü �ؽ�Ʈ
    private Color[] m_imgColors;                // �̹��� ���� ����
    private Color[] m_txtColors;                // �ؽ�Ʈ ���� ����

    [SerializeField]
    private bool m_fadeOnCreate = true;         // ���� ���ڸ��� Fade�� ��������
    [SerializeField]
    protected bool m_ableFade = true;            // Fade ��� on/off
    [SerializeField]
    private float m_fadeInTime = 0.5f;          // Fade In�� �ɸ��� �ð�
    [SerializeField]
    private float m_fadeOutTime = 0.5f;         // Fade Out�� �ɸ��� �ð�

    private float FadeTimeCount { get; set; }       // �ð� ī��Ʈ
    protected bool Fading { get; private set; }


    private void SetUIAlpha(float _alpha)       // UI ��ҵ� ���İ� ����
    {
        for (int i = 0; i<m_imgs.Length; i++) { Color color = m_imgColors[i]; color.a *= _alpha; m_imgs[i].color = color; }
        for (int i = 0; i<m_txts.Length; i++) { Color color = m_txtColors[i]; color.a *= _alpha; m_txts[i].color = color; }
    }


    public virtual void DestroyUI()
    {
        Destroy(gameObject);
    }


    public void FadeInStart()                   // Fade In ����
    {
        if(!m_ableFade) { FadeInDone(); return; }
        Fading = true;
        SetUIAlpha(0);
        FadeTimeCount = m_fadeInTime;
        StartCoroutine(FadingProcess(1));
    }
    public virtual void FadeInDone()            // Fade In ����
    {
        Fading = false;
        SetUIAlpha(1);
    }
    public virtual void FadeOutStart()          // Fade out ����
    {
        if (!m_ableFade) { FadeOutDone(); return; }
        Fading = true;
        SetUIAlpha(1);
        FadeTimeCount = m_fadeOutTime;
        StartCoroutine(FadingProcess(-1));
    }
    public virtual void FadeOutDone()           // Fade Out ����
    {
        Fading = false;
        SetUIAlpha(0);
    }

    private IEnumerator FadingProcess(int _change)      // change -> FadeIn = 1, FadeOut = -1
    {
        while (FadeTimeCount > 0)
        {
            FadeTimeCount -= Time.deltaTime;
            if (FadeTimeCount < 0) { FadeTimeCount = 0; }

            if(_change >= 0) { SetUIAlpha((m_fadeInTime-FadeTimeCount) / m_fadeInTime); }
            else { SetUIAlpha(FadeTimeCount / m_fadeOutTime); }
            yield return null;
        }
        if(_change >= 0) { FadeInDone(); }
        else { FadeOutDone(); }
    }


    public virtual void SetComps()
    {
        m_imgs = GetComponentsInChildren<Image>();
        m_txts = GetComponentsInChildren<TextMeshProUGUI>();
    }
    public virtual void SetAlphaInfo()
    {
        m_imgColors = new Color[m_imgs.Length];
        m_txtColors = new Color[m_txts.Length];
        for(int i=0;i<m_imgs.Length;i++) { m_imgColors[i] = m_imgs[i].color; }
        for(int i=0;i<m_txts.Length;i++) { m_txtColors[i] = m_txts[i].color; }
    }
    public virtual void Awake()
    {
        SetComps();
        SetAlphaInfo();
    }
    public virtual void Start()
    {
        if (m_fadeOnCreate)
            FadeInStart();
    }
}
