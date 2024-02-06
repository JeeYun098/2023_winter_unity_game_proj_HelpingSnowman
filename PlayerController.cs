using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rb;
    //float axisH = 0.0f;
    public float speed = 12.0f;   // 이동 속도

    public LayerMask groundLayer;  // 착지 레이어     

    public GameObject objPrefab;   // 발사체
    public float maxShotDelay = 1.0f;
    public float curShotDelay;
    public float fireSpeedx = 100.0f;
    bool zero = true;

    private SpriteRenderer spriteRenderer;
    //public string targetSceneName = "Stage1";

    public bool left = false;

    public int MovingAnim = 0;
    public GameObject PlayerMoving;
    private bool IsMoving = false;

    private int jumpcount = 0;
    public float jumpForce = 2000f;
    public float dashForce = 1500f;

    public GameObject hitEffectSound;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   // rigidbody2D 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        get_input();
        SnowAttack();
        Reload();
        JumpControl();
        DashControl();
        PlayerMovingCheck();
    }

    void get_input()
    {
        float x = Input.GetAxisRaw("Horizontal");
        IsMoving = false;
        MovingAnim = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            IsMoving = true;
            zero = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
            IsMoving = true;
            zero = false;
        }

        // stop  // 추가
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.000001f, rb.velocity.y);
        }

        if (IsMoving)
        {
            MovingAnim++;
            Vector3 moveVelocity = new Vector3(x, 0, 0) * speed * Time.deltaTime;
            rb.transform.position += moveVelocity;
        }

        // stop
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.000001f, rb.velocity.y);
        }
    }



    void PlayerMovingCheck()
    {
        if (MovingAnim == 1) PlayerMoving.SetActive(true);
        else if (MovingAnim == 0) PlayerMoving.SetActive(false);
    }

    void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            jumpcount++;
            if (jumpcount <= 1)
            {
                rb.AddForce(Vector3.up * jumpForce);
            }

        }
    }

    void DashControl()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float x = Input.GetAxisRaw("Horizontal");

            Vector2 dashDirection = new Vector2(x, 0).normalized;
            rb.AddForce(dashDirection * dashForce);
        }
    }

    public void SnowAttack()
    {
        if (Input.GetKey(KeyCode.Z) && curShotDelay >= maxShotDelay)
        {
            curShotDelay = 0;
            Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
            GameObject snow = Instantiate(objPrefab, pos, transform.rotation);
            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();
            //axisH = Input.GetAxisRaw("Horizontal");
            //if (axisH > 0.0f)
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                //zero = true;
            }
            //else if (axisH < 0.0f)
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
                //zero = false;
            }
            else
            {
                if (zero)
                {
                    rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                }
                else
                {
                    rigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
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
            Debug.Log($"Player Velocity: {rb.velocity.y}");
            // 낙하 중이고 enemy 보다 위에 있을 때
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                Debug.Log("밟기공격");
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
            /*gameObject.transform.position = Vector3.zero; //장애물에 찔리면 원위치*/
            Debug.Log("데미지");
            // damage
            OnDamaged(collision.transform.position);
        }
        if (collision.gameObject.tag == "Ground")
        {
            jumpcount = 0;
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.9f; // 플레이어의 크기를 0.9배로 줄임
        player.transform.localScale = currentScale;
    }

    public void OnAttack(Transform enemy)
    {
        // reaction
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // enemy die
        Destroy(enemy.gameObject);
    }

    public void OnDamaged(Vector2 targetPos)
    {
        hitEffectSound.SetActive(true);
        Debug.Log("생명감소");
        // health down
        gameManager.LifeDown();
        Debug.Log("크기감소");
        // shrink
        PlayerShrink(gameObject); //플레이어크기감소
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
        hitEffectSound.SetActive(false);
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
            //SceneManager.LoadScene(targetSceneName);
            SceneManager.LoadScene("3_Boss");
        }
    }
}

