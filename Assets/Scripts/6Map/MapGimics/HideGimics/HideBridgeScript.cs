using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBridgeScript : HideGimicScript
{
    private Material m_bridgeMaterial;

    private float m_changeTime = 1;

    [SerializeField]
    private float m_soundDistance = 10;

    private string DissolveAmount { get { return ValueDefine.DISSOLVE_AMOUNT_NAME; } }

    public override void GetLight()
    {
        base.GetLight();
        StartCoroutine(DissolveCoroutine(-1));

        if (PlayerDistance < m_soundDistance)
        {
            GameManager.PlaySE(EEnvironmentSE.BRIDGE_APPEAR, transform.position);
        }
    }

    public override void LoseLight()
    {
        base.LoseLight();
        StartCoroutine(DissolveCoroutine(1));
    }

    private IEnumerator DissolveCoroutine(float _change)
    {
        while((_change > 0 && m_bridgeMaterial.GetFloat(DissolveAmount) < 1) ||
            (_change < 0 && m_bridgeMaterial.GetFloat(DissolveAmount) > 0))
        {
            float amount = m_bridgeMaterial.GetFloat(DissolveAmount);
            amount += _change / m_changeTime * Time.deltaTime;
            if(amount < 0) { amount = 0; } else if(amount > 1) { amount = 1; }
            m_bridgeMaterial.SetFloat(DissolveAmount, amount);
            yield return null;
        }

    }


    public override void SetComps()
    {
        base.SetComps();
        m_bridgeMaterial = GetComponent<MeshRenderer>().material;
        m_bridgeMaterial.SetFloat(DissolveAmount, 1);
        m_colliders = GetComponentsInChildren<Collider>();
    }
}
