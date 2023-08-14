using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmashAttack : MonoBehaviour
{
    public int AttackDamage = 15;//���� ������
    public float AttackRange = 3f;//���� ��Ÿ�
    public float AttackUseRange = 2f;//���� ��� ��Ÿ�
    public float AttackDelay = 0.3f;// ���� ������
    public float AttackTime = 0.5f;//���� �ð�
    public float AttackKnockBackPower = 10.0f;//���� �˹� �Ŀ�

    //�ش� ��ü�� ���°��� �������� ���� EnemyManager ���� ����
    EnemyManager m_EnemyManager;

    RaycastHit2D[] hits; // ���� ��� ��ĵ�� ���� �����ɽ�Ʈ ����

    // Start is called before the first frame update
    void Start()
    {
        //EnemyManager ���� �ʱ�ȭ
        m_EnemyManager = this.GetComponent<EnemyManager>();
    }

    private void FixedUpdate()
    {
        float MoveArrow = this.GetComponent<EnemyMoveController>().MoveArrow; // ���� ������ ���� EnemyMoveController���� �̵� ���� ���ؿ���
        //RayCast�� Player üũ
        float thisAttackRange = m_EnemyManager.isAttack ? AttackRange : AttackUseRange;
        hits = Physics2D.RaycastAll(this.transform.position, new Vector2(MoveArrow, 0), thisAttackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player") && !m_EnemyManager.isKnockBack && !m_EnemyManager.isAttack)
                    StartCoroutine(PlayerAttack());
            }
        }
    }

    //���� ���� �ڷ�ƾ
    IEnumerator PlayerAttack()
    {
        m_EnemyManager.isAttack = true;//���� ���� True
        yield return new WaitForSeconds(AttackDelay); //���� ������ ����
        
        foreach (RaycastHit2D hit in hits)//������ ���� �÷��̾ ���� ������ �ִ� �� ���� üũ
        {
            if (hit.collider.CompareTag("Player") && !m_EnemyManager.isKnockBack && hit.collider.gameObject.layer != LayerMask.NameToLayer("InvcPlayer"))
             {
                Transform thisE = gameObject.GetComponent<Transform>();//Enemy�� ��ġ��
                PlayerController.instance.Hurt(AttackDamage, AttackKnockBackPower, thisE);
                yield return new WaitForSeconds(AttackTime - AttackDelay);
             }
        }
        m_EnemyManager.isAttack = false;//���� ���� false
    }
}
