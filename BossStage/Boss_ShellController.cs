using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ShellController : MonoBehaviour
{
    public float deleteTime = 30.0f;   // ������ �ð� ����

    public GameObject hpBar;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);
        hpBar = GameObject.FindObjectOfType<HP_bar>().gameObject;

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
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Enemy�� ���˵Ǹ� �ش� ������Ʈ ����
        }
        */
        if (collision.gameObject.tag == "Head")
        {
            Debug.Log("�Ӹ� ���� ��");
            hpBar.GetComponent<HP_bar>().Damage(0.05f);
        }
        else if (collision.gameObject.tag == "Body")
        {
            Debug.Log("���� ���� ��");
            //hpBar.GetComponent<HP_bar>().Damage(0.005f);
            hpBar.GetComponent<HP_bar>().Damage(0.025f);
        }

    }

}


