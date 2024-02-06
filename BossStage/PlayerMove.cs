using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Sprite[] snowmanSprites;
    public SpriteRenderer snowman;

    public float changePlayerActive;

    public int playerActiveIndex = 0;
    public int playerActiveFlag = 0;

    void OnEnable()
    {
        StartCoroutine(PlayerMoving());
    }

    IEnumerator PlayerMoving()
    {
        while (true)
        {
            snowman.sprite = snowmanSprites[playerActiveIndex];

            if (playerActiveFlag == 0)
            {
                playerActiveIndex = playerActiveIndex + 1;
                if (playerActiveIndex == 2) playerActiveFlag = 1;
            }

            else if (playerActiveFlag == 1)
            {
                playerActiveIndex = playerActiveIndex - 1;
                if (playerActiveIndex == 0) playerActiveFlag = 0;
            }

            yield return new WaitForSeconds(changePlayerActive);
        }


    }

}
