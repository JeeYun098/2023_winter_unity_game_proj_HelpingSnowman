using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ItemSnow : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerExpand(other.gameObject); // 적에 부딪히면 플레이어 크기 감소
            Destroy(gameObject); // 현재 오브젝트 적 파괴
        }
    }

    void PlayerExpand(GameObject player)
    {
        if (gameManager.life < 5) //최대 생명 5개
        {
            Vector3 currentScale = player.transform.localScale;
            currentScale *= 1.1f; // 플레이어의 크기를 1.1배로 늘림
            player.transform.localScale = currentScale;
            Debug.Log("아이템 눈 획득, 플레이어 크기 20% 증가");
            gameManager.LifeUp(); //생명증가
        }
        else if (gameManager.life >= 5) //생명이 6개보다 많아지면 더이상 증가하지 않음
        {
            Debug.Log("최대크기도달, 더이상 증가하지 않음");
            Debug.Log(gameManager.life);
        }
    }
}

