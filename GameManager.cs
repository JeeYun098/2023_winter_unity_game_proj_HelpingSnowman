using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //textUI사용

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public int life = 3;
    public Text txt_life;

    void Update()
    {
        GameOverCheck();
        txt_life.text = life.ToString(); //생명 UI
    }

    public void LifeDown()
    {
        if (life > 0)
        {
            life--;
            Debug.Log(life);
        }
    }

    public void LifeUp()
    {
        if (life < 5)
        {
            life++;
            Debug.Log(life);
        }
    }

    void GameOverCheck()
    {
        if (life <= 0)
        {
            GameOver();
            return;
        }
    }

    void GameOver()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit 오브젝트 파괴
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
    }

}
