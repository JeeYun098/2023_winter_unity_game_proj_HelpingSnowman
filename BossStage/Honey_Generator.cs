using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey_Generator : MonoBehaviour
{
    public GameObject honey;

    const float CREATE_INTERVAL = 0.18f;
    float mCreateTime = 0;
    float mTotalTime = 0;

    float mNextCreateInterval = CREATE_INTERVAL;

    int mphase;

    // Start is called before the first frame update
    void Start()
    {
        mphase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Honey Active");
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

        if (mTotalTime >= 5f)
        {
            mTotalTime = 0;
            mphase++;
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
