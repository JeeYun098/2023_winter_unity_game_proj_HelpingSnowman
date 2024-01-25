using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Sprite[] playerSprites;
    public SpriteRenderer player;
    public float changePlayerActive = 0.1f;

    public int playerActiveIndex = 0;
    public int playerActiveFlag = 0;

    void OnEnable()
    {
        StartCoroutine(PlayerMove());
    }

    IEnumerator PlayerMove()
    {
        while (true)
        {
            player.sprite = playerSprites[playerActiveIndex];

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
