using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_beta : MonoBehaviour
{
    public float speed = 1.5f; // �̵� �ӵ�
    public GameManager gameManager;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // �������� �̵�
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                return;
            else
            {
                PlayerShrink(collision.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
                Debug.Log("�� �浹, �÷��̾� ũ�� 20% ����");
                Destroy(gameObject); // ���� ������Ʈ �� �ı�
            }
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // �÷��̾��� ũ�⸦ 0.8��� ����
        player.transform.localScale = currentScale;
        if (gameManager.life > 0)
        {
            gameManager.life--; //������
            Debug.Log(gameManager.life);
        }
    }
}
