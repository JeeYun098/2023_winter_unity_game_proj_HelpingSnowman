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
    public float speed = 3.0f;   // �̵� �ӵ�
    public float jump = 9.0f;    // ������
    public float dash = 20.0f;    // �뽬 �ӵ�
    private float defaultspeed;

    //������ �κ� (���� ���̾� ���� ����)
    //public LayerMask groundLayer;  // ���� ���̾�
    bool goJump = false;           // ���� ���� �÷���
    bool onGround = false;         // ���� �÷���

    bool goDash = false;           // �뽬 �÷���
    public float defaultTime = 0.3f;
    private float dashTime;        // �뽬 �ð�

    public GameObject objPrefab;   // �߻�ü
    public float maxShotDelay = 1.0f;
    public float curShotDelay;
    public float fireSpeedx = 4.0f;
    bool zero = true;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   // rigidbody2D ��������
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
        axisH = Input.GetAxisRaw("Horizontal");   // ���� ���� �Է�

        // right
        if (axisH > 0.0f)
        {
            Debug.Log("������ �̵�");
            //������ �κ� (���� ũ�⿡ ���� �¿� �̵�)
            //transform.localScale = new Vector2(1, 1);
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        }
        // left
        else if (axisH < 0.0f)
        {
            Debug.Log("���� �̵�");
            //������ �κ� (���� ũ�⿡ ���� �¿� �̵�)
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
        //������ �κ� (�±׺񱳸� ���� ���� ���� OnCollisionEnter�Ʒ��� �߰���)
        // ���� ����
        //onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        // ���� ���ų� �ӵ��� 0�� �ƴ� ��
        if (onGround || axisH != 0)
        {
            // �ӵ� ����
            rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        }

        // ���� ������ ������ ������ ��
        if (onGround && goJump)
        {
            Debug.Log("����");
            Vector2 jumpPw = new Vector2(0, jump);     // ������ ���� ����
            rb.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;                            // ���� �÷��� ����
        }

        // �뽬�� ������ ��
        if (goDash)
        {
            if (dashTime <= 0)   // �뽬 �ð��� ������
            {
                dashTime = defaultTime;
                speed = defaultspeed;                                       // ���� �ӵ��� ���ư�
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                goDash = false;                                             // �뽬 �÷��� ����
            }
            else                // �뽬 �ð� ���� ��
            {
                speed = dash;                                               // �뽬 �ӵ�
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                dashTime -= Time.deltaTime;                                 // �뽬 ���� �ð� ����
            }
        }
    }

    // jump fuction
    public void Jump()
    {
        goJump = true;
        Debug.Log("���� ��ư ����");
    }

    // dash fuction
    public void Dash()
    {
        goDash = true;
        Debug.Log("�뽬 ��ư ����");
    }

    //������ �κ� (�±׺񱳸� ���� ���� ����)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            onGround = true;
        }
        /*
        //���
        if (collision.gameObject.tag == "Enemy")
        {
            // ���� ���̰� enemy ���� ���� ���� ��
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

        // ��� 3�ʵ��� ����
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
        // stage1 �±׿� �ε����� ���� stage�� �Ѿ
        if (collision.gameObject.tag == "stage1")
        {
            //gameManager.NextStage();
        }
    }

    public void VelocityZero()
    {
        /*rb.velocity = Vector2.zero;*/    // ���� ��ġ�� 0���� �ϴ� ��.
    }
}