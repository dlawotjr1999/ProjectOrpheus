using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLightScript : MonoBehaviour
{
    protected float DistToPlayer { get { return (PlayManager.PlayerPos-transform.position).magnitude; } }   // 플레이어와 거리
    public bool GettingLight { get; protected set; }                                                        // 빛 받는 상태인지

    public virtual void GetLight()                         // 빛을 받았을 때
    {
        GettingLight = true;
    }
    public virtual void LoseLight()                       // 빛을 그만 받을 때
    {
        GettingLight = false;
    }

    private void CheckLight()                      // 프레임마다 빛 변화 감지
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
