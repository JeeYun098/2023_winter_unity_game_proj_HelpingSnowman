using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject mainTitleScene;
    public SpriteRenderer TitleScene;
    public Sprite[] TitleAnimation;

    public GameObject walkSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        mainTitleScene.SetActive(false);
        StartCoroutine(PlayTitleAnim());
    }

    IEnumerator PlayTitleAnim()
    {
        TitleScene.sprite = TitleAnimation[0];
        yield return new WaitForSeconds(2.3f);

        walkSound.SetActive(true);

        for (int i = 1; i < 8; i++)
        {
            TitleScene.sprite = TitleAnimation[i];
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1.6f);
        TitleScene.sprite = TitleAnimation[8];
        yield return new WaitForSeconds(1.4f);
        for(int i = 9; i < 12; i++)
        {
            TitleScene.sprite = TitleAnimation[i];
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 12; i < 16; i++)
        {
            TitleScene.sprite = TitleAnimation[i];
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        TitleScene.sprite = TitleAnimation[17];

        yield return new WaitForSeconds(1f);
        mainTitleScene.SetActive(true);

    }
}
