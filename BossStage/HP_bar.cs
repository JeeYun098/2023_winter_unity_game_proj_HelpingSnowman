using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HP_bar : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;

    public GameObject BossSkill;
    public GameObject BossAttack;

    public SpriteRenderer bossBear;
    public Sprite[] deadBoss;
    public float closeEyes = 0.4f;
    public float fallDown = 0.2f;

    public Slider HpBar;
    // Start is called before the first frame update
    void Start()
    {
        SetHp(1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

    public void CheckHp()
    {
        if (HpBar != null)
        {
            HpBar.value = curHealth / maxHealth;
        }

    }

    public void Damage(float damage)
    {
        if (maxHealth == 0 || curHealth <= 0) return;
        curHealth -= damage;
        CheckHp();

        if (curHealth <= 0)
        {
            Debug.Log("º¸½º »ç¸Á");
            BossAttack.SetActive(false);
            BossSkill.SetActive(false);
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            bossBear.sprite = deadBoss[i];
            yield return new WaitForSeconds(closeEyes);
        }
        for (int i = 3; i < 7; i++)
        {
            bossBear.sprite = deadBoss[i];
            yield return new WaitForSeconds(fallDown);
        }

        SceneManager.LoadScene("4_GameEnd");

    }
}
