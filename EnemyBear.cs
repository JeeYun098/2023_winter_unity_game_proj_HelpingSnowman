using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBear : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision) //trigger체크, y축freeze필요
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerShrink(collision.gameObject); // 적에 부딪히면 플레이어 크기 감소
            Debug.Log("곰 충돌, 플레이어 크기 20% 감소");
            Destroy(gameObject); // 현재 오브젝트 적 파괴
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // 플레이어의 크기를 0.8배로 줄임
        player.transform.localScale = currentScale;
        gameManager.LifeDown(); //생명감소
    }
}
