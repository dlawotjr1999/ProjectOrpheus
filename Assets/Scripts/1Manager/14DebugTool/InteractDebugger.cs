#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDebugger : MonoBehaviour
{
    [SerializeField]
    private NPCScript[] m_tempNPCs;










    private void Start()
    {
        PlayManager.TempSetNPCs(m_tempNPCs);
    }
}
#endif