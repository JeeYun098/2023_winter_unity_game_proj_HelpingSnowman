using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Honey : MonoBehaviour
{
    const float CREATE_INTERVAL = 0.18f;
    float mCreateTime = 0;
    float mTotalTime = 0;

    float mNextCreateInterval = CREATE_INTERVAL;

    int mphase;

    public GameObject honey;

    public Sprite[] honeySprites;
    public SpriteRenderer bossBear;

    void OnEnable()
    {
        mphase = 1;
        StartCoroutine(HoneyActive());
    }

    // Update is called once per frame
    void Update()
    {
        mTotalTime += Time.deltaTime;
        mCreateTime += Time.deltaTime;
        if (mCreateTime > mNextCreateInterval)
        {
            mCreateTime = 0;
            mNextCreateInterval = CREATE_INTERVAL - (0.005f * mTotalTime);
            if (mNextCreateInterval < 0.005f)
            {
                mNextCreateInterval = 0.005f;
            }

            for (int i = 0; i < mphase; i++)
            {
                createHoney(8f + i * 0.2f);
            }

        }

        if (mTotalTime >= 10f)
        {
            mTotalTime = 0;
            mphase++;
        }
    }

    IEnumerator HoneyActive()
    {
        for (int i = 1; i < 4; i++)
        {
            bossBear.sprite = honeySprites[i];
            yield return new WaitForSeconds(0.1f);
        }

        while (true)
        {
            for(int i = 2; i < 4; i++)
            {
                bossBear.sprite = honeySprites[i];
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void createHoney(float y)
    {
        float x = Random.Range(-8f, 8f);
        createObject(honey, new Vector3(x, y, 0), Quaternion.identity);
    }

    private GameObject createObject(GameObject original, Vector3 position, Quaternion rotation)
    {
        return (GameObject)Instantiate(original, position, rotation);
    }
}
