using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    //수정된 부분 (생명추가)
    public static int life = 3;          // 생명

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
        //수정된 부분 (게임오버체크)
        GameOverCheck();
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
    }

    //수정된 부분 (게임오버체크)
    void GameOverCheck()
    {
        if (life <= 0)
        {
            GameOver();
            return;
        }
    }

    //수정된 부분 (게임오버)
    void GameOver()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit 오브젝트 파괴
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
        //gameover=true;

        /*여러개일때?
         GameObject[] bears = GameObject.FindGameObjectsWithTag("Bear");
            foreach (GameObject bear in bears)
            {
                Destroy(bear); // Bear 오브젝트 파괴
            }
         */
    }
}
