using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotingAttack : MonoBehaviour
{
    public int AttackDamage = 15;//���� ������
    public float AttackUseRange = 2f;//���� ��� ��Ÿ�
    public float AttackDelay = 0.3f;// ���� ������
    public float AttackTime = 0.5f;//���� �ð�
    public float AttackKnockBackPower = 10.0f;//���� �˹� �Ŀ�

    private Transform player;//�÷��̾� ��ġ�� ����

    //�ش� ��ü�� ���°��� �������� ���� EnemyManager ���� ����
    EnemyManager m_EnemyManager;

    // Start is called before the first frame update
    void Start()
    {
        //EnemyManager ���� �ʱ�ȭ
        m_EnemyManager = this.GetComponent<EnemyManager>();
        //�÷��̾� ��ġ�� ���ϱ� ���� ���� �ʱ�ȭ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float MoveArrow = this.GetComponent<EnemyMoveController>().MoveArrow; // ���� ������ ���� EnemyMoveController���� �̵� ���� ���ؿ���
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }
}
