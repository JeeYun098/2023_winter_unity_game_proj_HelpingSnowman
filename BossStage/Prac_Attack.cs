using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class prac_Skill
{
    public string name;
    public float cooldown;
    public float currentCooldown;
    public float curTime;
    public int priority;

    public prac_Skill(string name, float cooldown, int priority, float curTime, float currentCooldown)
    {
        this.name = name;
        this.cooldown = cooldown;
        this.priority = priority;
        this.curTime = curTime;
        this.currentCooldown = currentCooldown;
    }

    public bool IsReady()
    {
        return currentCooldown <= 0;
    }
}
public class Prac_Attack : MonoBehaviour
{
    public GameObject Strike;
    public GameObject Honey;
    public GameObject Shout;
    public GameObject Basic;

    public List<prac_Skill> skills = new List<prac_Skill>()
    {
        new prac_Skill("Strike",5f, 1, 0f, 0f),
        new prac_Skill("Honey", 15f, 2, 0f, 0f),
        new prac_Skill("Shout", 25f, 3, 0f, 0f)
};
    // Start is called before the first frame update
    void Start()
    {
        Basic.SetActive(true);
        foreach (prac_Skill skill in skills)
        {
            skill.curTime = Time.time;
        }
    }

    void Update()
    {
        // Update skill cooldowns
        foreach (prac_Skill skill in skills)
        {
            skill.currentCooldown = Time.time - skill.curTime;
            if(skill.currentCooldown >= skill.cooldown)
            {
                skill.currentCooldown = 0f;
                skill.curTime += skill.cooldown;
            }
        }

        // Find the highest priority skill with cooldown at 0
        prac_Skill nextSkill = null;
        int highestPriority = int.MaxValue;
        foreach (prac_Skill skill in skills)
        {
            if (skill.IsReady() && skill.priority < highestPriority)
            {
                highestPriority = skill.priority;
                nextSkill = skill;
            }
        }

        // Execute the selected skill and reset its cooldown
        if (nextSkill != null)
        {
            ExecuteSkill(nextSkill);
            nextSkill.currentCooldown = nextSkill.cooldown;
        }
    }

    IEnumerator ResetTime()
    {
        foreach (prac_Skill skill in skills)
        {
            yield return new WaitForSeconds(skill.cooldown);

        }
    }

    void ExecuteSkill(prac_Skill skill)
    {
        switch (skill.name)
        {
            case "Strike":
                Debug.Log($"Executing skill: {skill.name}");
                StartCoroutine(StrikeSkill());
                break;

            case "Honey":
                Debug.Log($"Executing skill: {skill.name}");
                StartCoroutine(HoneySkill());
                break;

            case "Shout":
                Debug.Log($"Executing skill: {skill.name}");
                StartCoroutine(ShoutSkill());
                break;
        }
    }

    IEnumerator StrikeSkill()
    {
        Basic.SetActive(false);
        Strike.SetActive(true);
        yield return new WaitForSeconds(5f);
        Strike.SetActive(false);
        Basic.SetActive(true);
    }

    IEnumerator HoneySkill()
    {
        Basic.SetActive(false);
        Honey.SetActive(true);
        yield return new WaitForSeconds(5f);
        Honey.SetActive(false);
        Basic.SetActive(true);
    }

    IEnumerator ShoutSkill()
    {
        Basic.SetActive(false);
        Shout.SetActive(true);
        yield return new WaitForSeconds(5f);
        Shout.SetActive(false);
        Basic.SetActive(true);
    }


}