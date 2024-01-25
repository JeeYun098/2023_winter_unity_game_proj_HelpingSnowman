using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnim : MonoBehaviour
{
    public Sprite[] bearSprites;
    public SpriteRenderer bear;
    public Sprite bearDeath;  // 적이 죽은 상태의 스프라이트
    public bool isBearDead = false;  // 적이 죽었는지 여부
    public float changeBearActive = 0.1f;

    public int bearActiveIndex = 0;
    public int bearActiveFlag = 0;

    void OnEnable()
    {
        StartCoroutine(BearMove());
    }

    IEnumerator BearMove()
    {
        while (!isBearDead)
        {
            bear.sprite = bearSprites[bearActiveIndex];

            if (bearActiveFlag == 0)
            {
                bearActiveIndex = bearActiveIndex + 1;
                if (bearActiveIndex == 2) bearActiveFlag = 1;
            }

            else if (bearActiveFlag == 1)
            {
                bearActiveIndex = bearActiveIndex - 1;
                if (bearActiveIndex == 0) bearActiveFlag = 0;
            }

            yield return new WaitForSeconds(changeBearActive);
        }
    }

    public void BearDie()
    {
        if (!isBearDead)
        {
            isBearDead = true;
            Debug.Log("곰 죽음");
            bear.sprite = bearDeath;
            StartCoroutine(DestroyAfterDelay(0.2f));  //delay후에 제거
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

