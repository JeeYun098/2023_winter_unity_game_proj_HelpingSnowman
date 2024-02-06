using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum E_STATE
    {
        NONE = 0,   
        START,
        PLAY,
        RETRY,
        GAMEOVER,
        END
    }

    E_STATE gamestate;           // 게임의 현재 상태

    public GameObject g_ui_GameStart;
    public GameObject g_ui_GameRetry;
    public GameObject g_ui_GameOver;
    public GameObject g_ui_GameEnd;

    //public int stageIndex;
    public int life = 3;
    public Text txt_life;

    //public GameObject[] Stages;
    public PlayerController_boss boss_player;
    public PlayerController player;

    public GameObject bgm;

    void Start()
    {
        gamestate = E_STATE.START;
    }

    void Update()
    {
        GameState();
    }

    void GameState()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
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

            case E_STATE.PLAY:
                GamePlay();
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

        bgm.SetActive(true);

        //player.Init();
        yield return new WaitForSeconds(1.5f);

        g_ui_GameStart.SetActive(false);

        //CreateEnemy(); //적 생성
        //CreateCoin(); //코인 생성

        gamestate = E_STATE.PLAY;
    }
    void GamePlay()
    {
        //플레이 멈추는 경우 생명이 모두 사라졌는지 체크하는 알고리즘
        GameOverCheck();

        txt_life.text = "life: " + life.ToString(); //플레이어 생명을 UI에 문자로 입력
    }
    void GameRetry()
    {
        g_ui_GameStart.SetActive(false);
        g_ui_GameOver.SetActive(false);
        g_ui_GameRetry.SetActive(true);
        g_ui_GameEnd.SetActive(false);

        bgm.SetActive(false);
    }
    void GameOver()
    {
        //g_ui_GameStart.SetActive(false);
        //g_ui_GameOver.SetActive(true);
        //g_ui_GameRetry.SetActive(true);
        //g_ui_GameEnd.SetActive(false);

        bgm.SetActive(false);
        DestroyAll();
        SceneManager.LoadScene("5_GameOver_game");
    }
    void GameEnd()
    {
        //g_ui_GameStart.SetActive(false);
        //g_ui_GameOver.SetActive(false);
        //g_ui_GameRetry.SetActive(true);
        //g_ui_GameEnd.SetActive(true);

        bgm.SetActive(false);
        DestroyAll();
        Application.Quit();
    }

    public void OnClickRetry()
    {
        g_ui_GameOver.SetActive(false);
        g_ui_GameRetry.SetActive(false);
        g_ui_GameEnd.SetActive(false);
        //gamestate = E_STATE.START;
        SceneManager.LoadScene("2_GameStage");
    }

    /*
    public void OnClickGameEnd()
    {
        gamestate = E_STATE.END;
    }

    public void Init()
    {
        player.transform.position = Vector3.zero;
        txt_life.text = life.ToString(); //생명 UI
        life = 3;
        player.speed = 12.0f;
        player.fireSpeedx = 100.0f;
        player.left = false;
        player.MovingAnim = 0;
        player.jumpForce = 2000f;
        player.dashForce = 1500f;
    }
    */

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
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit, Snow 오브젝트 파괴
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
        Destroy(GameObject.FindGameObjectWithTag("Snow"));
    }

    //화면을 벗어나면 원위치, 생명-1
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
