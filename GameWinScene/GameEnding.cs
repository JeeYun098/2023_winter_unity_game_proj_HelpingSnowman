using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    public SpriteRenderer gameEnd;
    public Sprite[] end;

    private int endingIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(endingAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator endingAnim()
    {
        while (true)
        {
            if (endingIdx == 0)
            {
                gameEnd.sprite = end[0];
                endingIdx++;
            }
            else
            {
                gameEnd.sprite = end[1];
                endingIdx--;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
