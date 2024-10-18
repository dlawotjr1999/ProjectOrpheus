using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellcatAnimationLink : MonoBehaviour
{
    private HellcatScript m_hellcat;


    public void JumpStartTiming()
    {
        m_hellcat.StartJump();
    }

    public void JumpStopTiming()
    {
        m_hellcat.StopJump();
    }


    private void Awake()
    {
        m_hellcat = GetComponentInParent<HellcatScript>();
    }
}
