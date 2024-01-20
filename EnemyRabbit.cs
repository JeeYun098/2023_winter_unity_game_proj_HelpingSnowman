using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyRabbit : MonoBehaviour
{
    public float xspeed = 2f;  //x축 움직일 속도
    public float yspeed = 2f;  //y축 움직일 속도
    public float height = 1f;  //y축 움직일 높이
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
                yspeed = -yspeed; // y축 방향 전환
            }
        }
        else
        {
            ChangeDirection = false;
            rb.velocity = new Vector2(-xspeed, yspeed); // x축,y축으로 이동
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerShrink(collision.gameObject); // 적에 부딪히면 플레이어 크기 감소
            Debug.Log("토끼 충돌, 플레이어 크기 20% 감소");
            Destroy(gameObject); // 현재 오브젝트 적 파괴
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.8f; // 플레이어의 크기를 0.8배로 줄임
        player.transform.localScale = currentScale;
        if (PlayerController.life > 0)
        {
            PlayerController.life--; //생명 감소
            Debug.Log(PlayerController.life);
        }

        /*
         void Attack()
        {
            if (attack)
            {
                Destroy(gameObject); // 현재 오브젝트 적 파괴
            }
        ]
         */
    }
}
