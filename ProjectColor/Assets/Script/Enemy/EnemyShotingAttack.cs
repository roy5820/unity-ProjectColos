using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotingAttack : MonoBehaviour
{
    public int AttackDamage = 15;//공격 데미지
    public float AttackUseRange = 2f;//공격 사용 사거리
    public float AttackDelay = 0.3f;// 공격 딜레이
    public float AttackTime = 0.5f;//공격 시간
    public float AttackKnockBackPower = 10.0f;//공격 넉백 파워

    private Transform player;//플레이어 위치값 변수

    //해당 객체의 상태값을 가져오기 위해 EnemyManager 변수 선언
    EnemyManager m_EnemyManager;

    // Start is called before the first frame update
    void Start()
    {
        //EnemyManager 변수 초기화
        m_EnemyManager = this.GetComponent<EnemyManager>();
        //플레이어 위치값 구하기 위한 변수 초기화
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float MoveArrow = this.GetComponent<EnemyMoveController>().MoveArrow; // 공격 방향을 위해 EnemyMoveController에서 이동 방향 구해오기
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }
}
