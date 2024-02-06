using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Shout : MonoBehaviour
{
    public GameObject shoutSpacePreview;
    public GameObject shoutSpace;

    public Sprite[] shoutSprites;
    public SpriteRenderer bossBear;

    public float changeShoutActive;


    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(ShoutActive());
    }

    IEnumerator ShoutActive()
    {
        bossBear.sprite = shoutSprites[0];
        yield return new WaitForSeconds(1.5f);

        shoutSpacePreview.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        shoutSpacePreview.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        for (int i = 1; i < 4; i++)
        {
            bossBear.sprite = shoutSprites[i];
            yield return new WaitForSeconds(changeShoutActive);
        }

        shoutSpace.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        shoutSpace.SetActive(false);
    }
}
