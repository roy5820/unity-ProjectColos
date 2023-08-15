using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotingAttack : MonoBehaviour
{
    public int AttackDamage = 15;//���� ������
    public float ShootingSpeed = 10f;//����ü �߻�ӵ�
    public float AttackUseRange = 5f;//���� ��� ��Ÿ�
    public float AttackDelay = 0.3f;// ���� ������
    public float AttackTime = 0.5f;//���� �ð�
    public float AttackKnockBackPower = 10.0f;//���� �˹� �Ŀ�
    public GameObject BulletPre; //������ �Ѿ� ������Ʈ

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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); //�÷��̾� Ž��
        //���� ���� �ȿ� �÷��̾ ���� �� ���Ÿ� ���� �߻�
        if (distanceToPlayer <= AttackUseRange && !m_EnemyManager.isAttack && !m_EnemyManager.isKnockBack)
        {
            Transform playerPos = player.transform;
            Vector3 playerCenter = player.position + new Vector3(0f, player.GetComponent<Collider2D>().bounds.extents.y, 0f);
            Vector2 playerDirection = (playerCenter - this.transform.position).normalized;//�÷��̾ �ִ� ���� ���

            StartCoroutine(ShootingAttack(playerDirection));
        }
    }

    //����ü ���� ���� �ڷ�ƾ
    IEnumerator ShootingAttack(Vector2 MoveVector)
    {
        m_EnemyManager.isAttack = true;
        GameObject EnemyBulletPre = Instantiate(BulletPre);

        if (!m_EnemyManager.isKnockBack)//���� ���� ������ �� ����
        {
            EnemyBulletPre.transform.position = this.transform.position;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateDamege = AttackDamage;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateKnockBackPower = AttackKnockBackPower;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateShootingSpeed = ShootingSpeed;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateMoveVector = MoveVector;

            yield return new WaitForSeconds(AttackDelay);//���� �� ������
        }

        yield return new WaitForSeconds(AttackTime - AttackDelay);//���� �� ������

        m_EnemyManager.isAttack = false;
    }
}
