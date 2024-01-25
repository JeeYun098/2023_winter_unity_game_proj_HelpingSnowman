using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 30.0f;   // ������ �ð� ����

    // Start is called before the first frame update
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
            Destroy(gameObject);   // ������Ʈ�� ���˵Ǹ� ����
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
