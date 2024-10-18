using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattler : MonoBehaviour
{
    private MonsterScript m_monster;

    [SerializeField]
    private bool m_againstMonster = false;
    public bool AgainstMonster { get { return m_againstMonster; } set { m_againstMonster = value; } }






    private void Awake()
    {
        m_monster = GetComponentInParent<MonsterScript>();
    }
}
