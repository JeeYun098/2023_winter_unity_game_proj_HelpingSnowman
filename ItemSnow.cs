using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ItemSnow : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerExpand(other.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
            Destroy(gameObject); // ���� ������Ʈ �� �ı�
        }
    }

    void PlayerExpand(GameObject player)
    {
        if (PlayerController.life < 5) //�ִ� ���� 5��
        {
            Vector3 currentScale = player.transform.localScale;
            currentScale *= 1.2f; // �÷��̾��� ũ�⸦ 1.2��� �ø�
            player.transform.localScale = currentScale;
            Debug.Log("������ �� ȹ��, �÷��̾� ũ�� 20% ����");
            PlayerController.life++; //��������
            Debug.Log(PlayerController.life);
        }
        else if (PlayerController.life >= 5) //������ 6������ �������� ���̻� �������� ����
        {
            Debug.Log("�ִ�ũ�⵵��, ���̻� �������� ����");
            Debug.Log(PlayerController.life);
        }
    }
}