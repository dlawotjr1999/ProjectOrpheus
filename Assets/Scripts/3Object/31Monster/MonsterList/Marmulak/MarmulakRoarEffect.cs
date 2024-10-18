using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MarmulakRoarEffect : ObjectAttackScript
{
    [SerializeField]
    private VisualEffect m_roarEffect;
    [SerializeField]
    private VisualEffect m_roarDistortion;

    public void Play() { PlayRoarEffect(); PlayDistortion(); }
    public void Stop() { StopDistortion(); }

    private void PlayRoarEffect() { m_roarEffect.Play(); }
    private void PlayDistortion() { m_roarDistortion.Play(); }
    private void StopDistortion() { m_roarDistortion.Stop(); }
}
