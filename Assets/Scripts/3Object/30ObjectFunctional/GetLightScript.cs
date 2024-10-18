using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLightScript : MonoBehaviour
{
    protected float DistToPlayer { get { return (PlayManager.PlayerPos-transform.position).magnitude; } }   // �÷��̾�� �Ÿ�
    public bool GettingLight { get; protected set; }                                                        // �� �޴� ��������

    public virtual void GetLight()                         // ���� �޾��� ��
    {
        GettingLight = true;
    }
    public virtual void LoseLight()                       // ���� �׸� ���� ��
    {
        GettingLight = false;
    }

    private void CheckLight()                      // �����Ӹ��� �� ��ȭ ����
    {
        ELightState state = PlayerLightScript.CurState;
        float size = PlayerLightScript.CurSize;
        if (!GettingLight)
        {
            if (state == ELightState.ON ||
                (state == ELightState.CHANGE && size >= DistToPlayer))
            {
                GetLight();
            }
        }
        else
        {
            if (state == ELightState.OFF ||
                state == ELightState.CHANGE && size <= DistToPlayer)
            {
                LoseLight();
            }
        }
    }


    public virtual void SetComps()
    {

    }

    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        LoseLight();
    }
    private void Update()
    {
        CheckLight();
    }
}
