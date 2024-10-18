using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_bgmList; 
    [SerializeField]
    private AudioClip[] m_playerSEList;                         // 여기부터 개별 프리펍에 없는 소리 (아무거나 넣지 마셈)
    [SerializeField]
    private AudioClip[] m_powerSEList;
    [SerializeField]
    private AudioClip[] m_monsterSEList;
    [SerializeField]
    private AudioClip[] m_environmentSEList;
    [SerializeField]
    private AudioClip[] m_systemSE;                             // 여기까지


    [SerializeField]
    private AudioSource CurBGM;
    [SerializeField]
    private GameObject m_sePrefab;
    public GameObject SEPrefab { get { return m_sePrefab; } }

    [Range(0, 100)]
    private static int m_bgmVolume = 100;               // BGM 볼륨
    [Range(0, 100)]
    private static int m_seVolume = 80;                // 효과음 볼륨
    public static float BGMVolume { get { return m_bgmVolume * 0.01f; } }
    public static float SEVolume { get { return m_seVolume * 0.01f; } }
    public void SetBGMVolume(int _volume) { m_bgmVolume = _volume; if (CurBGM) { CurBGM.volume = _volume / 100f; } PlayerPrefs.SetInt(ValueDefine.BGMVolData, _volume); }
    public void SetSEVolume(int _volume) { m_seVolume = _volume; PlayerPrefs.SetInt(ValueDefine.SEVolData, _volume); }

    private Vector3 CameraPos { get { return Camera.main.transform.position; } }

    public void PlayBGM(EBGM _bgm)
    {
        if (CurBGM.isPlaying) { StopBGM(); }
        CurBGM.clip = m_bgmList[(int)_bgm];
        CurBGM.volume = BGMVolume;
        CurBGM.Play();
    }
    public void ChangeBGM(EBGM _bgm)
    {
        StartCoroutine(ChangingBGM(_bgm));
    }
    private IEnumerator ChangingBGM(EBGM _bgm)
    {
        while (CurBGM.volume > 0) { CurBGM.volume -= Time.deltaTime; yield return null; }
        CurBGM.clip = m_bgmList[(int)_bgm];
        while(CurBGM.volume < BGMVolume) { CurBGM.volume += Time.deltaTime; yield return null; }
    }
    public void StopBGM() 
    {
        if (!CurBGM.isPlaying) { return; }
        CurBGM.Stop();
        CurBGM.clip = null;
    }



    public void PlaySE(EPlayerSE _se) { PlaySE(_se, CameraPos); }
    public void PlaySE(EPlayerSE _se, Vector3 _point) { PlaySE(m_playerSEList[(int)_se], _point); }
    public void PlaySE(EMonsterSE _se) { PlaySE(_se, CameraPos); }
    public void PlaySE(EMonsterSE _se, Vector3 _point) { PlaySE(m_monsterSEList[(int)_se], _point); }
    public void PlaySE(EPowerSE _se) { PlaySE(_se, CameraPos); }
    public void PlaySE(EPowerSE _se, Vector3 _point) { PlaySE(m_powerSEList[(int)_se - 1], _point); }
    public void PlaySE(EEnvironmentSE _se) { PlaySE(_se, CameraPos); }
    public void PlaySE(EEnvironmentSE _se, Vector3 _point) { PlaySE(m_environmentSEList[(int)_se], _point); }
    public void PlaySE(ESystemSE _se) { PlaySE(m_systemSE[(int)_se], CameraPos); }

    public void PlaySE(AudioClip _clip, Vector3 _point)                        // 효과음 예시
    {
        GameObject se = PoolManager.GetObject(m_sePrefab);
        se.transform.position = _point;
        SEPrefab script = se.GetComponent<SEPrefab>();
        script.PlaySE(_clip, SEVolume);
    }



    private void CheckSoundIntregrity()
    {
        if (m_bgmList.Length != (int)EBGM.LAST) { Debug.Log("BGM 개수 틀림"); return; }
        if (m_playerSEList.Length != (int)EPlayerSE.LAST) { Debug.Log("효과음 리스트1 개수 틀림"); return; }
        if (m_powerSEList.Length != (int)EBGM.LAST) { Debug.Log("효과음 리스트2 개수 틀림"); return; }
        if (m_monsterSEList.Length != (int)EBGM.LAST) { Debug.Log("효과음 리스트3 개수 틀림"); return; }
        if (m_environmentSEList.Length != (int)EBGM.LAST) { Debug.Log("효과음 리스트4 개수 틀림"); return; }
        if (m_systemSE.Length != (int)EBGM.LAST) { Debug.Log("효과음 리스트5 개수 틀림"); return; }
    }

    public void SetManager()
    {
        m_bgmVolume = PlayerPrefs.GetInt(ValueDefine.BGMVolData, 100);
        m_seVolume = PlayerPrefs.GetInt(ValueDefine.SEVolData, 100);
        CheckSoundIntregrity();
    }
}
