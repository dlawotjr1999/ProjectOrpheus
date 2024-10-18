using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PowerEffect : CombinedEffect
{
    [SerializeField]
    private float m_lastTime = -1;
    [SerializeField]
    private float m_returnDelay = 2;

    private Transform ReturnTrans { get; set; }
    private Vector3 OriginalPosition { get; set; }

    public void EffectOn(Transform _returnTrans)
    {
        ReturnTrans = _returnTrans;
        OriginalPosition = transform.localPosition;
        transform.SetParent(null);
        base.EffectOn();
        if(m_lastTime > 0) { StartCoroutine(WaitDone(m_lastTime)); }
    }
    public void EffectOn(Transform _returnTrans, float _time)
    {
        EffectOn(_returnTrans);
        LeaveEffect(_returnTrans, _time);
    }
    private IEnumerator WaitDone(float _time)
    {
        yield return new WaitForSeconds(_time);
        EffectOff();
    }
    public void LeaveEffect(Transform _transform, float _delay)
    {
        transform.SetParent(null);
        ReturnTrans = _transform;
        StartCoroutine(WaitDone(_delay));
    }
    public override void EffectOff()
    {
        base.EffectOff();
        StartCoroutine(ReturnDelay());
    }
    private IEnumerator ReturnDelay()
    {
        yield return new WaitForSeconds(m_returnDelay);
        transform.SetParent(ReturnTrans);
        transform.localPosition = OriginalPosition;
    }
}
