using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERegion
{
    START,
    LIFE,
    GOD,
    WATER,
    CRYSTAL,

    LAST
}

public class RegionMarker : MonoBehaviour
{
    [SerializeField]
    private ERegion m_region;
    public ERegion Region { get { return m_region; } }
}
