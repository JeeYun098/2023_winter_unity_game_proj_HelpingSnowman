using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAnim : MonoBehaviour
{
    public Sprite[] rabbitSprites;
    public SpriteRenderer rabbit;
    public Sprite rabbitDeath;  // 적이 죽은 상태의 스프라이트
    public bool isRabbitDead = false;  // 적이 죽었는지 여부
    public float changeRabbitActive = 0.1f;

    public int rabbitActiveIndex = 0;
    public int rabbitActiveFlag = 0;

    void OnEnable()
    {
        StartCoroutine(RabbitMove());
    }

    IEnumerator RabbitMove()
    {
        while (!isRabbitDead)
        {
            rabbit.sprite = rabbitSprites[rabbitActiveIndex];

            if (rabbitActiveFlag == 0)
            {
                rabbitActiveIndex = rabbitActiveIndex + 1;
                if (rabbitActiveIndex == 3) rabbitActiveFlag = 1;
            }

            else if (rabbitActiveFlag == 1)
            {
                rabbitActiveIndex = rabbitActiveIndex - 1;
                if (rabbitActiveIndex == 0) rabbitActiveFlag = 0;
            }

            yield return new WaitForSeconds(changeRabbitActive);
        }
    }

    public void RabbitDie()
    {
        if (!isRabbitDead)
        {
            isRabbitDead = true;
            Debug.Log("토끼 죽음");
            rabbit.sprite = rabbitDeath;
            StartCoroutine(DestroyAfterDelay(0.2f));  //delay후에 제거
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

