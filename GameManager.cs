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

    E_STATE gamestate;           // ������ ���� ����

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

        //CreateEnemy(); //�� ����
        //CreateCoin(); //���� ����

        gamestate = E_STATE.PLAY;
    }
    void GamePlay()
    {
        //�÷��� ���ߴ� ��� ������ ��� ��������� üũ�ϴ� �˰���
        GameOverCheck();

        txt_life.text = "life: " + life.ToString(); //�÷��̾� ������ UI�� ���ڷ� �Է�
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
        txt_life.text = life.ToString(); //���� UI
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
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit, Snow ������Ʈ �ı�
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
        Destroy(GameObject.FindGameObjectWithTag("Snow"));
    }

    //ȭ���� ����� ����ġ, ����-1
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
