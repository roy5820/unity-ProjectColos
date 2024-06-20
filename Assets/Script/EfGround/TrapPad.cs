using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPad : MonoBehaviour
{
    public float KnockbackForce = 10f; //�˹� �Ŀ�
    public int TrapDamage = 10; // Ʈ�� ������
    Transform thisT; // Ʈ���� ��ġ��


    private void Update()
    {
        thisT = gameObject.GetComponent<Transform>();//Ʈ���� ��ġ�� ������Ʈ
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //�÷��̾� ��Ʈ�ѷ� ����
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            playerController.Hurt(TrapDamage, KnockbackForce, thisT);//�÷��̾� �ǰ��Լ� ȣ��
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyController Enemy = other.GetComponent<EnemyController>(); // �� ��Ʈ�ѷ�
            Enemy.HurtEnemy(TrapDamage, KnockbackForce, thisT); // �� �ǰ��Լ� ȣ��

        }
    }
}
