using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : MonoBehaviour
{
    // tart is called before the first frame update
    private float GRAVITY = 0.5f;
    private float mVelocity = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current = this.transform.position;
        mVelocity += GRAVITY * Time.deltaTime;
        current.y -= mVelocity * Time.deltaTime;
        this.transform.position = current;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Honey_Collider" || collision.gameObject.tag == "Land")
        {
            //Debug.Log("≤‹ ªË¡¶");
            Destroy(this.gameObject);
        }
    }
}
