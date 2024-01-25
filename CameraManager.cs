using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");   // 플레이어 인식
        if (player != null)
        {
            // 카메라 좌표 갱신
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            // 가로 방향 동기화
            // 양 끝에 이동 제한
            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            // 세로 방향 동기화
            // 위 아래에 이동 제한
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }

            // 카메라 위치 벡터
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;
        }
    }
}
