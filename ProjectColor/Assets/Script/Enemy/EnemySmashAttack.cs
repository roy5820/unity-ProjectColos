using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmashAttack : MonoBehaviour
{
    public int AttackDamage = 15;//���� ������
    public float AttackRange = 4f;//���� ��Ÿ�
    public float AttackUseRange = 2.5f;//���� ��� ��Ÿ�
    public float AttackDelay = 0.3f;// ���� ������
    public float AttackTime = 0.5f;//���� �ð�
    public float AttackKnockBackPower = 10.0f;//���� �˹� �Ŀ�

    //�ش� ��ü�� ���°��� �������� ���� EnemyManager ���� ����
    EnemyManager m_EnemyManager;

    RaycastHit2D[] hits; // ���� ��� ��ĵ�� ���� �����ɽ�Ʈ ����
    Collider2D col2D;//�ڽ��ɽ�Ʈ ũ�⸦ ���ϱ� ���� �ش� ������Ʈ�� �ݶ��̴� ���� �����ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        //EnemyManager ���� �ʱ�ȭ
        m_EnemyManager = this.GetComponent<EnemyManager>();

        //�ݶ��̴� �� ����
        col2D = this.GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        float MoveArrow = this.GetComponent<EnemyMoveController>().MoveArrow; // ���� ������ ���� EnemyMoveController���� �̵� ���� ���ؿ���
        //RayCast�� Player üũ
        float thisAttackRange = m_EnemyManager.isAttack ? AttackRange : AttackUseRange;//��Ȳ�� ���� ��Ÿ�
        Vector2 thisAttackBoxSize =  new Vector2(this.transform.localScale.x * thisAttackRange * MoveArrow, col2D.bounds.size.y*1.5f); // ���� �ڽ� ũ�� ����
        hits = Physics2D.BoxCastAll(col2D.bounds.center, thisAttackBoxSize, 0f, new Vector2(MoveArrow, 0), 1);
        
        //üũ�� �ݶ��̴��� �÷��̾ ���� �� �÷��̾� ���� �ǽ�
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
