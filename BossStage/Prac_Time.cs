using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTime
{
    public string name;
    public int priority;
    public float coolTime;
    public float curTime;
    public float storeTime;

    public CoolTime(string name, int priority, float coolTime, float curTime, float storeTime)
    {
        this.name = name;
        this.priority = priority;
        this.coolTime = coolTime;
        this.curTime = curTime;
        this.storeTime = storeTime;
    }

    public bool IsReady()
    {
        return curTime >= coolTime;
    }
}

public class Prac_Time : MonoBehaviour
{
    public List<CoolTime> skills = new List<CoolTime>()
    {
        new CoolTime("Time1", 1, 5f, 0f, 0f),
        new CoolTime("Time2", 2, 10f, 0f, 0f),
        new CoolTime("Time3", 3, 15f, 0f, 0f)
    };

    // Update is called once per frame
    void Update()
    {
        CoolTime nextSkill = GetNextSkill();
        if (nextSkill != null)
        {
            Debug.Log($"Using skill: {nextSkill.name}      {nextSkill.curTime}");
            nextSkill.storeTime = Time.time;
            nextSkill.curTime = 0f;
        }

        foreach (CoolTime t in skills)
        {
            t.curTime = Time.time - t.storeTime;
            if (t.curTime >= t.coolTime)
            {
                t.storeTime = Time.time;
            }
        }
    }

    CoolTime GetNextSkill()
    {
        CoolTime nextSkill = null;
        int highestPriority = int.MaxValue;

        foreach (CoolTime skill in skills)
        {
            if (skill.IsReady() && skill.priority < highestPriority)
            {
                highestPriority = skill.priority;
                nextSkill = skill;
            }
        }

        return nextSkill;
    }
}
