using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyRabbit : MonoBehaviour
{
    public float xspeed = 2f;  //x�� ������ �ӵ�
    public float yspeed = 2f;  //y�� ������ �ӵ�
    public float height = 1f;  //y�� ������ ����
    public bool ChangeDirection = false;
    private float originalY;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (transform.position.y <= originalY || transform.position.y >= originalY + height)
        {
            if (ChangeDirection == false)
            {
                ChangeDirection = true;
                yspeed = -yspeed; // y�� ���� ��ȯ
            }
        }
        else
        {
            ChangeDirection = false;
            rb.velocity = new Vector2(-xspeed, yspeed); // x��,y������ �̵�
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerShrink(collision.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
            Debug.Log("�䳢 �浹, �÷��̾� ũ�� 20% ����");
            Destroy(gameObject); // ���� ������Ʈ �� �ı�
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // �÷��̾��� ũ�⸦ 0.8��� ����
        player.transform.localScale = currentScale;
        if (PlayerController.life > 0)
        {
            PlayerController.life--; //���� ����
            Debug.Log(PlayerController.life);
        }

        /*
         void Attack()
        {
            if (attack)
            {
                Destroy(gameObject); // ���� ������Ʈ �� �ı�
            }
        ]
         */
    }
}
