using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Basic : MonoBehaviour
{
    public Sprite[] basicSprites;
    public SpriteRenderer bossBear;

    public float changeBasicActive;

    public int basicActiveIndex = 0;
    public int basicActiveFlag = 0;

    void OnEnable()
    {
        StartCoroutine(BasicActive());
    }

    IEnumerator BasicActive()
    {
        while (true)
        {
            bossBear.sprite = basicSprites[basicActiveIndex];

            // Thread.Sleep은 메인 스레드를 차단하므로 주의해서 사용
            // Thread.Sleep((int)(changeBasicActive * 1000));

            if (basicActiveFlag == 0)
            {
                basicActiveIndex = basicActiveIndex + 1;
                if (basicActiveIndex == 2) basicActiveFlag = 1;
            }

            else if (basicActiveFlag == 1)
            {
                basicActiveIndex = basicActiveIndex - 1;
                if (basicActiveIndex == 0) basicActiveFlag = 0;
            }

            yield return new WaitForSeconds(changeBasicActive);
        }
    }

}
