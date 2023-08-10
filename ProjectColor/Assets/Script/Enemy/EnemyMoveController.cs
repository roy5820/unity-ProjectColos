using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    public float detectionRange = 4f;  // 플레이어를 감지하는 범위
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

    //

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // 플레이어 감지
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 절벽 체크
        OnFrontGround = FrontGroundSensor.GetComponent<Sensor>().colSencorState() || FrontPlatformSensor.GetComponent<Sensor>().colSencorState();
        Debug.Log(OnFrontGround);
        if (distanceToPlayer < detectionRange)
        {
            // 플레이어를 방향으로 MoveArrow 변경
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x < 0)
                MoveArrow = -1;
            else
                MoveArrow = 1;
        }

        // 
        if (OnFrontGround)
        {
            rb.velocity = new Vector2(movementSpeed * MoveArrow, rb.velocity.y);
            Debug.Log("MoveValue: " + movementSpeed + ", " + MoveArrow);
        }
        else
        {
            MoveArrow *= -1;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //MoveArrow에 따라 로컬스케일 반전
        if (MoveArrow > 0)
            this.transform.localScale = new Vector2(1, 1);
        else if (MoveArrow < 0)
            this.transform.localScale = new Vector2(-1, 1);
    }
}
