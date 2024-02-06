using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Active_Strike : MonoBehaviour
{
    public GameObject LSpacePreview;
    public GameObject RSpacePreview;
    public GameObject LSpace;
    public GameObject RSpace;

    public SpriteRenderer bossBear;
    public Sprite[] strikePreviews;
    public Sprite[] strikeLeftBear;
    public Sprite[] strikeRightBear;

    public float changeImageInterval = 0.1f;
    public float changeRangePreview = 0.2f;
    public float changeStrikeRange = 0.7f;
    public float changeStrikeAttack = 0.5f;

    public int[] LRStrikes;

    void OnEnable()
    {
        StartCoroutine(StrikePreviewBear());
        StartCoroutine(PreviewAndStrike());

    }

    IEnumerator PreviewAndStrike()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            int randomNum = Random.Range(0, 2);
            LRStrikes[i] = randomNum;

            if (randomNum == 0)
            {
                LSpacePreview.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                LSpacePreview.SetActive(false);
            }
            else if (randomNum == 1)
            {
                RSpacePreview.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                RSpacePreview.SetActive(false);
            }

            yield return new WaitForSeconds(changeRangePreview);
        }

        for(int i = 0; i < 3; i++)
        {
            if (LRStrikes[i] == 0) 
            { 
            
                StartCoroutine(StrikeLeft());
            }
            if (LRStrikes[i] == 1)
            {
                StartCoroutine(StrikeRight());
            }
            yield return new WaitForSeconds(changeStrikeAttack);

        }

    }

    IEnumerator StrikePreviewBear()
    {
        for (int i = 0; i < strikePreviews.Length; i++)
        {
            bossBear.sprite = strikePreviews[i];

            yield return new WaitForSeconds(changeImageInterval);
        }
        yield return new WaitForSeconds(1f);
    }

    IEnumerator StrikeLeft()
    {
        for (int i = 0; i < strikePreviews.Length; i++)
        {
            bossBear.sprite = strikeLeftBear[i];

            yield return new WaitForSeconds(changeImageInterval);
        }
        LSpace.SetActive(true);
        yield return new WaitForSeconds(changeStrikeRange);
        LSpace.SetActive(false);

    }

    IEnumerator StrikeRight()
    {
        for (int i = 0; i < strikePreviews.Length; i++)
        {
            bossBear.sprite = strikeRightBear[i];

            yield return new WaitForSeconds(changeImageInterval);
        }
        RSpace.SetActive(true);
        yield return new WaitForSeconds(changeStrikeRange);
        RSpace.SetActive(false);
    }

}
