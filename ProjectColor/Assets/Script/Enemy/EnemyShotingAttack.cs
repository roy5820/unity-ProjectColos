using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotingAttack : MonoBehaviour
{
    public int AttackDamage = 15;//공격 데미지
    public float ShootingSpeed = 10f;//투사체 발사속도
    public float AttackUseRange = 5f;//공격 사용 사거리
    public float AttackDelay = 0.3f;// 공격 딜레이
    public float AttackTime = 0.5f;//공격 시간
    public float AttackKnockBackPower = 10.0f;//공격 넉백 파워
    public GameObject BulletPre; //생성할 총알 오브젝트

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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); //플레이어 탐지
        //공격 번위 안에 플레이어가 있을 시 원거리 공격 발사
        if (distanceToPlayer <= AttackUseRange && !m_EnemyManager.isAttack && !m_EnemyManager.isKnockBack)
        {
            Transform playerPos = player.transform;
            Vector3 playerCenter = player.position + new Vector3(0f, player.GetComponent<Collider2D>().bounds.extents.y, 0f);
            Vector2 playerDirection = (playerCenter - this.transform.position).normalized;//플레이어가 있는 방향 계산

            StartCoroutine(ShootingAttack(playerDirection));
        }
    }

    //투사체 공격 구현 코루틴
    IEnumerator ShootingAttack(Vector2 MoveVector)
    {
        m_EnemyManager.isAttack = true;
        GameObject EnemyBulletPre = Instantiate(BulletPre);

        if (!m_EnemyManager.isKnockBack)//공격 가능 상태일 시 공격
        {
            EnemyBulletPre.transform.position = this.transform.position;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateDamege = AttackDamage;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateKnockBackPower = AttackKnockBackPower;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateShootingSpeed = ShootingSpeed;
            EnemyBulletPre.GetComponent<EnemyBullet>().UdateMoveVector = MoveVector;

            yield return new WaitForSeconds(AttackDelay);//공격 선 딜레이
        }

        yield return new WaitForSeconds(AttackTime - AttackDelay);//공격 후 딜레이

        m_EnemyManager.isAttack = false;
    }
}
