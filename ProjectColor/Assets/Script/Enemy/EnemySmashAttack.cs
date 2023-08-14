using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmashAttack : MonoBehaviour
{
    public int AttackDamage = 15;//공격 데미지
    public float AttackRange = 3f;//공격 사거리
    public float AttackUseRange = 2f;//공격 사용 사거리
    public float AttackDelay = 0.3f;// 공격 딜레이
    public float AttackTime = 0.5f;//공격 시간
    public float AttackKnockBackPower = 10.0f;//공격 넉백 파워

    //해당 객체의 상태값을 가져오기 위해 EnemyManager 변수 선언
    EnemyManager m_EnemyManager;

    RaycastHit2D[] hits; // 공격 대상 스캔을 위한 레이케스트 변수

    // Start is called before the first frame update
    void Start()
    {
        //EnemyManager 변수 초기화
        m_EnemyManager = this.GetComponent<EnemyManager>();
    }

    private void FixedUpdate()
    {
        float MoveArrow = this.GetComponent<EnemyMoveController>().MoveArrow; // 공격 방향을 위해 EnemyMoveController에서 이동 방향 구해오기
        //RayCast로 Player 체크
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

    //공격 구현 코루틴
    IEnumerator PlayerAttack()
    {
        m_EnemyManager.isAttack = true;//공격 여부 True
        yield return new WaitForSeconds(AttackDelay); //공격 딜레이 구현
        
        foreach (RaycastHit2D hit in hits)//딜레이 이후 플레이어가 공격 범위에 있는 지 이중 체크
        {
            if (hit.collider.CompareTag("Player") && !m_EnemyManager.isKnockBack && hit.collider.gameObject.layer != LayerMask.NameToLayer("InvcPlayer"))
             {
                Transform thisE = gameObject.GetComponent<Transform>();//Enemy의 위치값
                PlayerController.instance.Hurt(AttackDamage, AttackKnockBackPower, thisE);
                yield return new WaitForSeconds(AttackTime - AttackDelay);
             }
        }
        m_EnemyManager.isAttack = false;//공격 여부 false
    }
}
