using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ShellController : MonoBehaviour
{
    public float deleteTime = 30.0f;   // 제거할 시간 지정

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
            Destroy(gameObject);   // 오브젝트에 접촉되면 제거
        }
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Enemy에 접촉되면 해당 오브젝트 제거
        }
        */
        if (collision.gameObject.tag == "Head")
        {
            Debug.Log("머리 공격 중");
            hpBar.GetComponent<HP_bar>().Damage(0.05f);
        }
        else if (collision.gameObject.tag == "Body")
        {
            Debug.Log("몸통 공격 중");
            //hpBar.GetComponent<HP_bar>().Damage(0.005f);
            hpBar.GetComponent<HP_bar>().Damage(0.025f);
        }

    }

}


