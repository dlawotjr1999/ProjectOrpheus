using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterScript))]
public class MonsterLighter : HideScript            // ���� ���� HideScript => ���� ��ɱ��� ����
{
    private MonsterScript m_monster;

    [SerializeField]
    private bool m_isHiding;

    public void HideMonster() { m_isHiding = true; }


    public override void GetLight()                         // ���� �޾��� ��
    {
        GettingLight = true;
        if (m_isHiding)
        {
            base.GetLight();
        }

    }
    public override void LoseLight()                       // ���� �׸� ���� ��
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
