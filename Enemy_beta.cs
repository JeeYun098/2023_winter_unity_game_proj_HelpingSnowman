using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_beta : MonoBehaviour
{
    public float speed = 1.5f; // 이동 속도
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
        // 왼쪽으로 이동
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("충돌 발생: " + collision.gameObject.name);
            Debug.Log($"Player Position: {collision.transform.position}");
            Debug.Log($"Enemy Position: {transform.position}");

            float relativeY = collision.transform.position.y - transform.position.y;

            if (relativeY > 0 && rb.velocity.y < 0)
            {
                Debug.Log("밟기");
                Destroy(gameObject); // 현재 오브젝트 적 파괴
            }
            else
            {
                PlayerShrink(collision.gameObject); // 적에 부딪히면 플레이어 크기 감소
                Debug.Log("적 충돌, 플레이어 크기 20% 감소");
                Destroy(gameObject); // 현재 오브젝트 적 파괴
            }
            
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // 플레이어의 크기를 0.8배로 줄임
        player.transform.localScale = currentScale;
        if (gameManager.life > 0)
        {
            gameManager.life--; //생명감소
            Debug.Log(gameManager.life);
        }
    }
}
