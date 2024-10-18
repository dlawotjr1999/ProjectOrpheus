using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField]
    private float m_mapScale = 8;
    public Image m_mapImg;

    protected OasisIconScript[] m_oasisPoints;
    protected AltarIconScript[] m_altarPoints;

    protected RectTransform MapRect { get { return m_mapImg.rectTransform; } }
    protected float MapImgHeight { get { return m_mapImg.sprite.rect.height; } }

    protected float MapHeight { get { return PlayManager.MapHeight; } }

    public float MapScale { get { return m_mapScale; } private set { m_mapScale = value; } }


    private void SetPosition()
    {
        Vector2 player = PlayManager.PlayerPos2;
        Vector2 playerOffset = player / MapHeight;

        Vector2 pivot = playerOffset * 0.5f + new Vector2(0.25f, 0.25f);
        MapRect.pivot = pivot;
    }

    private void SetRotation()
    {
        float player = PlayManager.CameraRotation;
        MapRect.localEulerAngles = new(0, 0, player);
    }

    private void SetMapGimicPosition()
    {
        m_oasisPoints = GetComponentsInChildren<OasisIconScript>();
        m_altarPoints = GetComponentsInChildren<AltarIconScript>();
        for (int i = 0; i < m_oasisPoints.Length; i++)
        {
            if (i >= (int)EOasisName.LAST) { m_oasisPoints[i].gameObject.SetActive(false); continue; }
            m_oasisPoints[i].SetParent(this);
            m_oasisPoints[i].SetComps((EOasisName)i, m_mapImg.rectTransform);
        }
        for (int i = 0; i < m_altarPoints.Length; i++)
        {
            if (i >= (int)EAltarName.LAST) { m_altarPoints[i].gameObject.SetActive(false); continue; }
            m_altarPoints[i].SetParent(this);
            m_altarPoints[i].SetComps((EAltarName)i, m_mapImg.rectTransform);
        }
    }


    public void SetScale(float _scale)
    {
        m_mapScale = _scale;
        float realScale = MapHeight / MapImgHeight * _scale;

        m_mapImg.rectTransform.localScale = new(realScale, realScale, 1);
        SetPosition();
    }

    private void InitSize()
    {
        MapRect.sizeDelta = new(MapImgHeight, MapImgHeight);
    }
    protected virtual void Start()
    {
        if (!GameManager.IsInGame) { return; }
        InitSize();
        SetMapGimicPosition();
        SetScale(MapScale);
        SetRotation();
    }
    protected virtual void Update()
    {
        if (!GameManager.IsInGame) { return; }
        SetPosition();
        SetRotation();
    }
}
