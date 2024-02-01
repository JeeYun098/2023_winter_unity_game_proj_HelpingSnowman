using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 5.0f;   // 제거할 시간 지정

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Dead"))
        {
            Destroy(gameObject);   // 오브젝트에 접촉되면 제거
        }
        if (collision.gameObject.CompareTag("Bear"))
        {
            BearAnim bearAnim = collision.gameObject.GetComponent<BearAnim>();
            if (bearAnim != null)
            {
                bearAnim.BearDie();
            }
        }
        if (collision.gameObject.CompareTag("Rabbit"))
        {
            RabbitAnim rabbitAnim = collision.gameObject.GetComponent<RabbitAnim>();
            if (rabbitAnim != null)
            {
                rabbitAnim.RabbitDie();
            }
        }
    }
}
