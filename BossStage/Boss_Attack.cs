using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public string name;
    public int priority;
    public float coolTime;
    public float curTime;
    public float storeTime;

    public Skill(string name, int priority, float coolTime, float curTime, float storeTime)
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
public class Boss_Attack : MonoBehaviour
{
    public bool SkillIsRunning = false;

    public GameObject Strike;
    public GameObject Honey;
    public GameObject Shout;
    public GameObject Basic;

    public SpriteRenderer bossBear;

    public Queue<string> attackReady = new Queue<string>();

    int attackReady_num = 0;
    public string[] attackReady_Arr;

    public List<Skill> skills = new List<Skill>()
    {
        new Skill("Strike", 2, 4f, 3f, 0f),
        new Skill("Shout", 3, 7.5f, 7.3f, 0f),
        new Skill("Honey", 1, 9f, 4f, 0f)
};
    // Start is called before the first frame update
    void Start()
    {
        Basic.SetActive(true);
        foreach (Skill skill in skills)
        {
            skill.storeTime = Time.time;
        }
    }

    void Update()
    {
        Skill nextSkill = GetNextSkill();
        if (nextSkill != null)
        {
            ReadySkill(nextSkill);
            nextSkill.storeTime = Time.time;
            nextSkill.curTime = 0f;
        }

        foreach (Skill skill in skills)
        {
            skill.curTime = Time.time - skill.storeTime;
            if (skill.coolTime <= skill.curTime)
            {
                skill.storeTime += skill.coolTime;
            }
        }
    }

    Skill GetNextSkill()
    {
        Skill nextSkill = null;
        int highestPriority = int.MaxValue;

        foreach (Skill skill in skills)
        {
            if (skill.IsReady() && skill.priority < highestPriority)
            {
                highestPriority = skill.priority;
                nextSkill = skill;
            }
        }

        return nextSkill;
    }

    void ReadySkill(Skill skill)
    {
        int ReadySkillCount = attackReady.Count;

        if(ReadySkillCount >= 4)
        {
            string firstReadySkill = attackReady.Peek();
            ExecuteSkill(firstReadySkill);
            attackReady.Dequeue();
        }
        else
        {
            if (attackReady_num > 9) attackReady_num = 0;
            attackReady_Arr[attackReady_num] = skill.name;
            attackReady_num++;

            attackReady.Enqueue(skill.name);
        }

    }

    void ExecuteSkill(string skill)
    {
        Basic.SetActive(false);
        // Debug.Log($"attack srart {skill}");
        switch (skill)
        {
            case "Strike":
                if(!SkillIsRunning)
                    StartCoroutine(StrikeSkill());
                break;

            case "Honey":
                if (!SkillIsRunning) 
                    StartCoroutine(HoneySkill());
                break;

            case "Shout":
                if (!SkillIsRunning) 
                    StartCoroutine(ShoutSkill());
                break;
        }
    }

    IEnumerator StrikeSkill()
    {
        SkillIsRunning = true;
        Strike.SetActive(true);

        yield return new WaitForSeconds(5f);

        Strike.SetActive(false);
        SkillIsRunning = false;
        Basic.SetActive(true);

    }

    IEnumerator HoneySkill()
    {
        SkillIsRunning = true;
        Honey.SetActive(true);

        yield return new WaitForSeconds(5f);

        Honey.SetActive(false);
        SkillIsRunning = false;
        Basic.SetActive(true);
    }

    IEnumerator ShoutSkill()
    {
        SkillIsRunning = true;
        Shout.SetActive(true);
        yield return new WaitForSeconds(3.5f);

        Shout.SetActive(false);
        SkillIsRunning = false;
        Basic.SetActive(true);
    }


}

