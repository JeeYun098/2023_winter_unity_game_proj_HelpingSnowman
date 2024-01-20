using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    //������ �κ� (�����߰�)
    public static int life = 3;          // ����

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
        //������ �κ� (���ӿ���üũ)
        GameOverCheck();
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
    }

    //������ �κ� (���ӿ���üũ)
    void GameOverCheck()
    {
        if (life <= 0)
        {
            GameOver();
            return;
        }
    }

    //������ �κ� (���ӿ���)
    void GameOver()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player")); // Player, Bear, Rabbit ������Ʈ �ı�
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
        //gameover=true;

        /*�������϶�?
         GameObject[] bears = GameObject.FindGameObjectsWithTag("Bear");
            foreach (GameObject bear in bears)
            {
                Destroy(bear); // Bear ������Ʈ �ı�
            }
         */
    }
}
