using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    private EnemyManager EManager;// 에너미 메니저 연결

    public float detectionRange = 5f;  // 플레이어를 감지하는 범위
    public float movementSpeed = 2f;   // 이동 속도

    private Transform player;           // 플레이어의 Transform 컴포넌트
    private Rigidbody2D rb;            // Rigidbody2D 컴포넌트
    private float MoveArrow = 1;

    //바닥 체크 센서
    public GameObject GroundSensor;
    public GameObject PlatformSensor;
    bool OnGround = false;

    //절벽 체크 센서 
    public GameObject FrontGroundSensor;
    public GameObject FrontPlatformSensor;
    bool OnFrontGround = false;

    //이동 여부
    bool isMove = false;

    //플레이어 탐색 여부
    bool ScanPlayer = true;
    float StapScanTime = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EManager = GetComponent<EnemyManager>();
    }

    private void Update()
    {
        // 절벽 체크
        if(ScanPlayer)
            OnFrontGround = FrontGroundSensor.GetComponent<Sensor>().colSencorState() || FrontPlatformSensor.GetComponent<Sensor>().colSencorState();

        // 플레이어 감지
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;// 앞 뒤 거리 계산

        //적 오브젝트 이동 방향 설정
        //플레이어 탐색 하여 해당 방향으로 이동
        if (distanceToPlayer < detectionRange && ScanPlayer)
        {
            // 플레이어를 방향으로 MoveArrow 변경
            if (OnFrontGround)
            {
                if (direction.x < -0.1f)
                    MoveArrow = -1;
                else if(direction.x > 0.1f)
                    MoveArrow = 1;
            }
        }
        //앞으로 갈 공간이 없을 시 유턴 및 일정 시간동안 플레이어 탐지 안함
        if (!OnFrontGround)
        {
            OnFrontGround = true;
            MoveArrow *= -1;
            StartCoroutine(StopScanPlayer());
        }

        //적 오브젝트 이동 여부 설정
        if (EManager.isKnockBack)
            isMove = false;
        else if (OnFrontGround)
            isMove = true;

        //MoveArrow에 따라 로컬스케일 반전
        if (MoveArrow > 0)
        {
            this.transform.localScale = new Vector2(1, 1);
        }
        else if (MoveArrow < 0)
        {
            this.transform.localScale = new Vector2(-1, 1);
            //this.transform.FindChild("EnemyCanvas").transform.localScale = new Vector2(1, 1);
        }
    }

    private void FixedUpdate()
    {
        //Enemy 이동 구현
        if (isMove)
        {
            rb.velocity = new Vector2(movementSpeed * MoveArrow, rb.velocity.y);
        }
    }
    private IEnumerator StopScanPlayer()
    {
        ScanPlayer = false;
        yield return new WaitForSeconds(StapScanTime);
        ScanPlayer = true;
    }
}
