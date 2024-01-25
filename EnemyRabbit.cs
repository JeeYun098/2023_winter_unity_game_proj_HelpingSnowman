using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRabbit : MonoBehaviour
{
    public float speed = 4f; // 이동 속도
    public GameManager gameManager;
    private Rigidbody2D rb;
    private bool movingRight = true;
    public SpriteRenderer rabbit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (movingRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                return;
            else
            {
                PlayerShrink(collision.gameObject); // 적에 부딪히면 플레이어 크기 감소
                Debug.Log("적 충돌, 플레이어 크기 20% 감소");
                Destroy(gameObject); // 현재 오브젝트 적 파괴
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            movingRight = !movingRight;
            if (rabbit.flipX == true)
            {
                rabbit.flipX = false;
            }
            else if (rabbit.flipX == false)
            {
                rabbit.flipX = true;
            }
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.9f; // 플레이어의 크기를 0.9배로 줄임
        player.transform.localScale = currentScale;
    }
}
