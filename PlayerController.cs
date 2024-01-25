using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rb;
    float axisH = 0.0f;
    public float speed = 4.0f;   // 이동 속도
    public float jump = 9.0f;    // 점프력
    public float dash = 20.0f;    // 대쉬 속도 
    private float defaultspeed;

    public LayerMask groundLayer;  // 착지 레이어
    bool goJump = false;           // 점프 시작 플래그
    bool onGround = false;         // 지면 플래그

    bool goDash = false;           // 대쉬 플래그
    public float defaultTime = 0.3f;      
    private float dashTime;        // 대쉬 시간

    public GameObject objPrefab;   // 발사체
    public float maxShotDelay;
    public float curShotDelay;
    public float fireSpeedx = 30.0f;
    bool zero = true;

    private SpriteRenderer spriteRenderer;
    public string targetSceneName = "Stage1";

    public bool left = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   // rigidbody2D 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();  
        dashTime = defaultTime;
        defaultspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        get_input();
        SnowAttack();
        Reload();
        //MoveCheck();
    }

    void get_input()
    {
        axisH = Input.GetAxisRaw("Horizontal");   // 수평 방향 입력

        // right
        if (axisH > 0.0f)
        {
            if (!left) //방향전환x 오른쪽
            {
                Debug.Log("오른쪽 이동");
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                left = false;
            }
            else if (left) //방향전환o 오른쪽
            {
                Debug.Log("오른쪽 이동");
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                left = false;
            }
        }
        // left
        else if (axisH < 0.0f)
        {
            if (!left) //방향전환o 왼쪽
            {
                Debug.Log("왼쪽 이동");
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                left = true;
            }
            else if (left) //방향전환x 왼쪽
            {
                Debug.Log("왼쪽 이동");
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                left = true;
            }
            
        }
        // stop
        if(Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.000001f, rb.velocity.y);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        // dash
        if (Input.GetKeyDown(KeyCode.X))
        {
            Dash();
        }
    }

    void FixedUpdate()
    {
        // 착지 판정
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        // 지면 위거나 속도가 0이 아닐 때
        if (onGround || axisH != 0)
        {
            // 속도 갱신
            rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        } 

        // 지면 위에서 점프를 눌렀을 때
        if(onGround && goJump)
        {
            Debug.Log("점프");
            Vector2 jumpPw = new Vector2(0, jump);     // 점프를 위한 벡터
            rb.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;                            // 점프 플래그 끄기
        }

        // 대쉬를 눌렀을 때
        if(goDash)
        {
            if(dashTime <= 0)   // 대쉬 시간이 끝나면
            {
                dashTime = defaultTime;
                speed = defaultspeed;                                       // 기존 속도로 돌아감
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                goDash = false;                                             // 대쉬 플래그 끄기
            }
            else                // 대쉬 시간 지속 중
            {
                speed = dash;                                               // 대쉬 속도
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                dashTime -= Time.deltaTime;                                 // 대쉬 유지 시간 감소
            }
        }
    }

    // jump function
    public void Jump()
    {
        goJump = true;
        Debug.Log("점프 버튼 눌림");
    }

    // dash fuction
    public void Dash()
    {
        goDash = true;
        Debug.Log("대쉬 버튼 눌림");
    }

    public void SnowAttack()
    {
        if (Input.GetKey(KeyCode.Z) && curShotDelay >= maxShotDelay)
        {
            curShotDelay = 0;
            Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
            GameObject snow = Instantiate(objPrefab, pos, transform.rotation);
            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();
            axisH = Input.GetAxisRaw("Horizontal");
            if (axisH > 0.0f)
            {
                rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                zero = true;
            }
            else if (axisH < 0.0f)
            {
                rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                zero = false;
            }
            else
            {
                if (zero)
                {
                    rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                }
                else
                {
                    rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    // 밟기
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bear"|| collision.gameObject.tag == "Rabbit")
        {
            // 낙하 중이고 enemy 보다 위에 있을 때
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
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
        if (collision.gameObject.tag == "Spike")
        {
            gameObject.transform.position = Vector3.zero; //장애물에 찔리면 원위치
            Debug.Log("데미지");
            // damage
            OnDamaged(collision.transform.position);
        }
    }

    public void OnAttack(Transform enemy)
    {
        // reaction
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // enemy die
    }

    public void OnDamaged(Vector2 targetPos)
    {
        // health down
        gameManager.LifeDown();
        Debug.Log("무적상태");
        // change layer
        gameObject.layer = 11;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        // 충격 3초동안 유지
        Invoke("OffDamaged", 3);
    }

    public void OffDamaged()
    {
        Debug.Log("원래로");
        // chage layer
        gameObject.layer = 10;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        /*// sprite alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // sprite filp Y
        spriteRenderer.filpY = true;
        // collider disable
        capsuleCollier.enabled = false;
        // die effect jump
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);*/

    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stage")) // 충돌한 객체가 "Player" 태그를 가진 경우
        {
            Debug.Log("씬 바뀜");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}

