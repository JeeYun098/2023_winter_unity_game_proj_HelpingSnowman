using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int life = 3;
    public Text txt_life;

    public GameObject[] Stages;
    public PlayerController player;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPlayerPosition(other.gameObject);
            life--;
        }
    }

    private void ResetPlayerPosition(GameObject player)
    {
        // 플레이어의 위치를 (0, 0, 0)으로 초기화
        player.transform.position = Vector3.zero;
    }
}
