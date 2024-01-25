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
        txt_life.text = life.ToString(); //���� UI
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
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit ������Ʈ �ı�
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
        // �÷��̾��� ��ġ�� (0, 0, 0)���� �ʱ�ȭ
        player.transform.position = Vector3.zero;
    }
}
