using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterScript))]
public class MonsterLighter : HideScript            // 몬스터 전용 HideScript => 성불 기능까지 갖춤
{
    private MonsterScript m_monster;

    [SerializeField]
    private bool m_isHiding;

    public void HideMonster() { m_isHiding = true; }


    public override void GetLight()                         // 빛을 받았을 때
    {
        GettingLight = true;
        if (m_isHiding)
        {
            base.GetLight();
        }

    }
    public override void LoseLight()                       // 빛을 그만 받을 때
    {
        GettingLight = false;
        if (m_isHiding)
        {
            base.LoseLight();
        }
    }


    public override void SetComps()
    {
        base.SetComps();
        m_monster = GetComponent<MonsterScript>();
    }
}
