using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum E_STATE
    {
        NONE = 0,   
        START,
        RETRY,
        GAMEOVER,
        END
    }

    E_STATE gamestate;           // 게임의 현재 상태

    public GameObject g_ui_GameStart;
    public GameObject g_ui_GameRetry;
    public GameObject g_ui_GameOver;
    public GameObject g_ui_GameEnd;

    public int stageIndex;
    public int life = 3;
    public Text txt_life;

    public GameObject[] Stages;
    public PlayerController_boss boss_player;
    public PlayerController player;

    void Start()
    {
        gamestate = E_STATE.START;
    }

    void Update()
    {
        GameState();
        GameOverCheck();
        txt_life.text = life.ToString(); //생명 UI
    }

    void GameState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            gamestate = E_STATE.END;
        if (Input.GetKeyDown(KeyCode.R))
            gamestate = E_STATE.RETRY;
        switch (gamestate)
        {
            case E_STATE.NONE:
                break;

            case E_STATE.START:
                gamestate = E_STATE.NONE;
                StartCoroutine(GameStart());
                break;

            case E_STATE.RETRY:
                gamestate = E_STATE.NONE;
                GameRetry();
                break;

            case E_STATE.GAMEOVER:
                gamestate = E_STATE.NONE;
                GameOver();
                break;

            case E_STATE.END:
                gamestate = E_STATE.NONE;
                GameEnd();
                break;
        }
    }

    IEnumerator GameStart()
    {
        g_ui_GameStart.SetActive(true);
        g_ui_GameOver.SetActive(false);
        g_ui_GameRetry.SetActive(false);
        g_ui_GameEnd.SetActive(false);

        //Init();
        yield return new WaitForSeconds(1.5f);

        g_ui_GameStart.SetActive(false);
    }
    void GameRetry()
    {
        g_ui_GameStart.SetActive(false);
        g_ui_GameOver.SetActive(false);
        g_ui_GameRetry.SetActive(true);
        g_ui_GameEnd.SetActive(false);
    }
    void GameOver()
    {
        g_ui_GameStart.SetActive(false);
        g_ui_GameOver.SetActive(true);
        g_ui_GameRetry.SetActive(true);
        g_ui_GameEnd.SetActive(false);

        DestroyAll();
    }
    void GameEnd()
    {
        g_ui_GameStart.SetActive(false);
        g_ui_GameOver.SetActive(false);
        g_ui_GameRetry.SetActive(true);
        g_ui_GameEnd.SetActive(true);

        DestroyAll();
    }

    public void OnClickStart()
    {
        gamestate = E_STATE.START;
    }

    public void OnClickGameEnd()
    {
        gamestate = E_STATE.END;
    }

    public void Init()
    {
        player.transform.position = Vector3.zero;
        life = 3;
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
            DestroyAll();
            gamestate = E_STATE.GAMEOVER;
            return;
        }
    }

    void DestroyAll()
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
