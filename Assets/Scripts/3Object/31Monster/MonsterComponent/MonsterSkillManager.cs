using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillManager : MonoBehaviour
{
    private MonsterScript m_monster;

    public int SkillNum { get; private set; }

    [SerializeField]
    private ObjectAttackScript[] m_skillList;
    [SerializeField]
    private float[] m_skillDamages;
    [SerializeField]
    private float[] m_skillCooltimes;
    [SerializeField]
    private float m_anySkillCooltime = 8;

    public int CurSkillIdx { get; private set; }
    public int NextSkillIdx { get; private set; }
    public bool CanSkill { get { return AnySkillTimeCheck && SkillTimeCount[NextSkillIdx] <= 0; } }
    public bool AnySkillTimeCheck { get { return AnySkillTimeCount <= 0; } }
    public bool SkillTimeCheck { get { foreach(float time in SkillCooltimes) { if(time <= 0) { return true; } } return false; } }

    public ObjectAttackScript[] SkillList { get { return m_skillList; } }
    public float[] SkillDamages { get { return m_skillDamages; } }
    public float[] SkillCooltimes { get { return m_skillCooltimes; } }

    public float[] SkillTimeCount { get; protected set; }
    private float AnySkillTimeCount { get; set; }


    public void StartSkill()
    {
        CurSkillIdx = NextSkillIdx;
    }
    public void SkillUsed(int _idx)
    {
        AnySkillTimeCount += m_anySkillCooltime;
        SkillTimeCount[_idx] += SkillCooltimes[_idx];
        SetNextSkill();
    }
    private void SetNextSkill()
    {
        if(SkillNum == 1) { NextSkillIdx = 0; return; }
        int minIdx = 0;
        float minTime = SkillTimeCount[minIdx];
        for (int i = 1; i<SkillNum; i++)
        {
            float curTime = SkillTimeCount[i];
            if (curTime < minTime) { minIdx = i; }
            else if (curTime == minTime) { minIdx = Random.Range(0, 2) == 0 ? i : minIdx; }
        }
        NextSkillIdx = minIdx;
    }

    public void SetManager(MonsterScript _monster)
    {
        m_monster = _monster;
        SkillNum = m_skillList.Length;
        if (m_skillDamages.Length != SkillNum || m_skillCooltimes.Length != SkillNum)
        {
            // Debug.LogError($"{m_monster} 스킬 개수 설정 잘못됨"); return;
        }
        SkillTimeCount = new float[SkillNum];
        NextSkillIdx = Random.Range(0, SkillNum);
    }

    private void Update()
    {
        if (AnySkillTimeCount > 0) { AnySkillTimeCount -= Time.deltaTime; }
        for (int i = 0; i<SkillNum; i++)
        {
            if (SkillTimeCount[i] > 0) { SkillTimeCount[i] -= Time.deltaTime; }
        }
    }
}
