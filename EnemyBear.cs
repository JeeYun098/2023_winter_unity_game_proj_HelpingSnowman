using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBear : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision) //triggerüũ, y��freeze�ʿ�
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerShrink(collision.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
            Debug.Log("�� �浹, �÷��̾� ũ�� 20% ����");
            Destroy(gameObject); // ���� ������Ʈ �� �ı�
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // �÷��̾��� ũ�⸦ 0.8��� ����
        player.transform.localScale = currentScale;
        gameManager.LifeDown(); //������
    }
}
