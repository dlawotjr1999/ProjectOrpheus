using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltarIconScript : MonoBehaviour
{
    private MinimapScript m_parent;

    private RectTransform m_rect;
    private Image m_img;

    [SerializeField]
    private EAltarName m_pointName;
    private EAltarName PointName { get { return m_pointName; } set { m_pointName = value; } }

    public void SetParent(MinimapScript _parent) { m_parent = _parent; }
    private void SetPosition(RectTransform _rect)
    {
        float width = PlayManager.MapWidth, height = PlayManager.MapHeight;

        Vector2 pos = PlayManager.AltarList[(int)PointName].Position2;
        Vector2 mapSize = _rect.sizeDelta;

        m_rect.anchoredPosition = new(mapSize.x * pos.x / width, mapSize.y * pos.y / height);
    }

    public void SetComps(EAltarName _altar, RectTransform _rect)
    {
        m_rect = GetComponent<RectTransform>();
        m_img = GetComponent<Image>();
        PointName = _altar;
        SetPosition(_rect);
    }
}

