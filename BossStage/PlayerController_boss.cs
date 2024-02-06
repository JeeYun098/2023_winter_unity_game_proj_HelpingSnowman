using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public enum PlayerState {run, jump, dash }

public class PlayerController_boss : MonoBehaviour
{
    public GameObject BossAttacking;
    public GameObject BossMoving;

    PlayerState pstate = PlayerState.run;
    Rigidbody2D player_rig;

    public bool isMoving = false;
    public int MovingAnim = 0;

    int jumpcount = 0;
    public float moveSpeed;
    public float dashForce;
    public float jumpForce;

    public GameObject hpBar;
    public float head_damageAmount;
    public float body_damageAmount;

    public Text hit;
    private double hitNum = 0;

    public PolygonCollider2D RightCol;
    public PolygonCollider2D LeftCol;

    public SpriteRenderer snowman;
    public GameObject PlayerMoving;

    private bool alreadyProcessed = false;
    private bool isDamaged = false;

    private SpriteRenderer spriteRenderer;       // 추가
    //public string targetSceneName = "BossStage"; // 추가

    public GameObject objPrefab;
    private int playerDireaction;
    public float maxShotDelay;
    public float curShotDelay;
    bool Zero = true;

    public GameObject hitEffectSound;
    public GameObject bgm;
    public GameObject GameRetry;

    // Start is called before the first frame update
    void Start()
    {
        player_rig = this.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   // 추가

        RightCol.enabled = true;
        LeftCol.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        player_rig.WakeUp(); // 매 프레임마다 물리력이 비활성화된 것을 활성화 시킴

        MoveControl();
        JumpControl();
        DashControl();
        PlayerMovingCheck();
        Reload();
        SnowAttack();
        GameOverCheck();
        CheckQuit();
        CheckRetry();
    }

    void PlayerMovingCheck()
    {
        if (MovingAnim == 1) PlayerMoving.SetActive(true);
        else if (MovingAnim == 0) PlayerMoving.SetActive(false);
    }
  

    void MoveControl()
    {
        float x = Input.GetAxisRaw("Horizontal");
        isMoving = false;
        MovingAnim = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerDireaction = 1;
            LeftCol.enabled = false;
            RightCol.enabled = true;

            snowman.flipX = false;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerDireaction = 0;
            LeftCol.enabled = true;
            RightCol.enabled = false;

            snowman.flipX = true;
            isMoving = true;
        }

        // stop  // 추가
        if (Input.GetButtonUp("Horizontal"))
        {
            player_rig.velocity = new Vector2(player_rig.velocity.normalized.x * 0.000001f, player_rig.velocity.y);
        }

        if (isMoving)
        {
            print("현재 상태: " + pstate);
            MovingAnim++;
            pstate = PlayerState.run;
            Vector3 moveVelocity = new Vector3(x, 0, 0) * moveSpeed * Time.deltaTime;
            player_rig.transform.position += moveVelocity;
        }
    }


    void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            jumpcount++;
            if (jumpcount <= 2)
            {
                print("현재 상태: " + pstate);
                pstate = PlayerState.jump;
                player_rig.AddForce(Vector3.up * jumpForce);
            }

        }
    }

    void DashControl()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            print("현재 상태: " + pstate);
            pstate = PlayerState.dash;
            float x = Input.GetAxisRaw("Horizontal");

            Vector2 dashDirection = new Vector2(x, 0).normalized; 
            player_rig.AddForce(dashDirection * dashForce);
        }
    }

    void CheckQuit()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            bgm.SetActive(false);
            Destroy(gameObject);
            Application.Quit();
        }
    }

    void CheckRetry()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bgm.SetActive(false);
            GameRetry.SetActive(true);
        }
    }

    public void OnClickRetry()
    {
        GameRetry.SetActive(false);
        SceneManager.LoadScene("3_Boss");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Enemy")
        {
            // 낙하 중이고 enemy 보다 위에 있을 때
            if (player_rig.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                // attack
                OnAttack(collision.transform);
            }
            else
            {
                Debug.Log("데미지");
                // damage
                OnDamaged(collision.transform.position);
            }
        }
        if (collision.gameObject.tag == "Dead")
        {
            Debug.Log("데미지");
            // damage
            OnDamaged(collision.transform.position);
        }
        */
        if (collision.gameObject.tag == "Land" || collision.gameObject.tag == "Honey_Collider")
        {
            pstate = PlayerState.run;
            //Debug.Log("Land 충돌");
            jumpcount = 0;
        }
    }

    public void OnAttack(Transform enemy)
    {
        // reaction
        player_rig.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // enemy die
    }

    public void OnDamaged(Vector2 targetPos)
    {
        isDamaged = true; //데미지상태on
        // health down
        Debug.Log("무적상태");
        // change layer
        gameObject.layer = 11;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        player_rig.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        // 충격 3초동안 유지
        Invoke("OffDamaged", 3);
    }

    public void SnowAttack()
    {
        if (Input.GetKey(KeyCode.Z) && curShotDelay >= maxShotDelay)
        {
            Debug.Log("공격중");
            curShotDelay = 0;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject snow = Instantiate(objPrefab, pos, transform.rotation);
            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();

            if (playerDireaction == 1)
            {
                Debug.Log("right Snowball instantiated");
                rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                Zero = true;
            }
            else if (playerDireaction == 0)
            {
                Debug.Log("left snowball instantiated");
                rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                Zero = false;
            }
        }
    }

    public void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OffDamaged()
    {
        Debug.Log("원래로");
        // chage layer
        gameObject.layer = 10;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isDamaged = false; //데미지상태 off
        hitEffectSound.SetActive(false);
    }    // 여기까지


    private void OnTriggerEnter2D(Collider2D other)
    {
        alreadyProcessed = false;

        if (other.gameObject.tag == "Strike" || other.gameObject.tag == "Shout" || other.gameObject.tag == "Honey")
        {
            if (!alreadyProcessed)
            {
                if (!isDamaged)
                {
                    Hitting();
                    Debug.Log(hitNum);
                    if (hitNum < 5)
                    {
                        // damage
                        OnDamaged(gameObject.transform.position);
                        Debug.Log("데미지");
                    }
                }
            }
        }

        /*
        // change scene       // 추가
        if (other.CompareTag("stage")) // 충돌한 객체가 "stage" 태그를 가진 경우
        {
            Debug.Log("보스 스테이지");
            SceneManager.LoadScene(targetSceneName);  // BossStage 씬으로 바뀜
        }
        */
    }
    public void Hitting()
    {
        hitNum++;
        hitEffectSound.SetActive(true);
        hit.text = "hit: " + hitNum.ToString();

        alreadyProcessed = true;
    }

    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Head")
        {
            //Debug.Log("머리");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("머리 공격 중");
                hpBar.GetComponent<HP_bar>().Damage(body_damageAmount);

            }
        }
        else if (other.gameObject.tag == "Body")
        {
            //Debug.Log("몸통");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("몸통 공격 중");
                hpBar.GetComponent<HP_bar>().Damage(body_damageAmount);

            }
        }
    }
    */

    public void GameOverCheck()
    {
        if (hitNum >= 5)
        {
            GameOver();
            return;
        }
    }

    public void GameOver()
    {
        BossAttacking.SetActive(false);
        BossMoving.SetActive(false);
        SceneManager.LoadScene("6_GameOver_boss");
    }

}
