using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rb;
    float axisH = 0.0f;
    public float speed = 3.0f;   // 이동 속도
    public float jump = 9.0f;    // 점프력
    public float dash = 20.0f;    // 대쉬 속도
    private float defaultspeed;

    //수정된 부분 (착지 레이어 변수 삭제)
    //public LayerMask groundLayer;  // 착지 레이어
    bool goJump = false;           // 점프 시작 플래그
    bool onGround = false;         // 지면 플래그

    bool goDash = false;           // 대쉬 플래그
    public float defaultTime = 0.3f;
    private float dashTime;        // 대쉬 시간

    public GameObject objPrefab;   // 발사체
    public float maxShotDelay = 1.0f;
    public float curShotDelay;
    public float fireSpeedx = 4.0f;
    bool zero = true;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   // rigidbody2D 가져오기
        dashTime = defaultTime;
        defaultspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        get_input();
        SnowAttack();
        Reload();
    }

    void get_input()
    {
        axisH = Input.GetAxisRaw("Horizontal");   // 수평 방향 입력

        // right
        if (axisH > 0.0f)
        {
            Debug.Log("오른쪽 이동");
            //수정된 부분 (현재 크기에 맞춰 좌우 이동)
            //transform.localScale = new Vector2(1, 1);
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        }
        // left
        else if (axisH < 0.0f)
        {
            Debug.Log("왼쪽 이동");
            //수정된 부분 (현재 크기에 맞춰 좌우 이동)
            //transform.localScale = new Vector2(-1, 1);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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
        //수정된 부분 (태그비교를 통해 착지 판정 OnCollisionEnter아래에 추가함)
        // 착지 판정
        //onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        // 지면 위거나 속도가 0이 아닐 때
        if (onGround || axisH != 0)
        {
            // 속도 갱신
            rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        }

        // 지면 위에서 점프를 눌렀을 때
        if (onGround && goJump)
        {
            Debug.Log("점프");
            Vector2 jumpPw = new Vector2(0, jump);     // 점프를 위한 벡터
            rb.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;                            // 점프 플래그 끄기
        }

        // 대쉬를 눌렀을 때
        if (goDash)
        {
            if (dashTime <= 0)   // 대쉬 시간이 끝나면
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

    // jump fuction
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

    //수정된 부분 (태그비교를 통해 착지 판정)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            onGround = true;
        }
        /*
        //밟기
        if (collision.gameObject.tag == "Enemy")
        {
            // 낙하 중이고 enemy 보다 위에 있을 때
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                // attack
                OnAttack(collision.transform);
            }
            else
            {
                // damage
                OnDamaged(collision.transform.position);
            }
        }
        */
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

    public void OnAttack(Transform enemy)
    {
        // reaction
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // enemy die
        /*EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();*/
    }

    public void OnDamaged(Vector2 targetPos)
    {
        // health down
        //gameManager.HealthDown();

        // chage layer
        gameObject.layer = 11;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // reaction forc
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        // 충격 3초동안 유지
        Invoke("OffDamaged", 3);
    }

    public void OffDamaged()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // stage1 태그에 부딪히면 다음 stage로 넘어감
        if (collision.gameObject.tag == "stage1")
        {
            //gameManager.NextStage();
        }
    }

    public void VelocityZero()
    {
        /*rb.velocity = Vector2.zero;*/    // 시작 위치를 0으로 하는 것.
    }
}