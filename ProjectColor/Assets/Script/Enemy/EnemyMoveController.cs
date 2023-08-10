using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    private EnemyManager EManager;// ���ʹ� �޴��� ����

    public float detectionRange = 5f;  // �÷��̾ �����ϴ� ����
    public float movementSpeed = 2f;   // �̵� �ӵ�

    private Transform player;           // �÷��̾��� Transform ������Ʈ
    private Rigidbody2D rb;            // Rigidbody2D ������Ʈ
    private float MoveArrow = 1;

    //�ٴ� üũ ����
    public GameObject GroundSensor;
    public GameObject PlatformSensor;
    bool OnGround = false;

    //���� üũ ���� 
    public GameObject FrontGroundSensor;
    public GameObject FrontPlatformSensor;
    bool OnFrontGround = false;

    //�̵� ����
    bool isMove = false;

    //�÷��̾� Ž�� ����
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
        // ���� üũ
        if(ScanPlayer)
            OnFrontGround = FrontGroundSensor.GetComponent<Sensor>().colSencorState() || FrontPlatformSensor.GetComponent<Sensor>().colSencorState();

        // �÷��̾� ����
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;// �� �� �Ÿ� ���

        //�� ������Ʈ �̵� ���� ����
        //�÷��̾� Ž�� �Ͽ� �ش� �������� �̵�
        if (distanceToPlayer < detectionRange && ScanPlayer)
        {
            // �÷��̾ �������� MoveArrow ����
            if (OnFrontGround)
            {
                if (direction.x < -0.1f)
                    MoveArrow = -1;
                else if(direction.x > 0.1f)
                    MoveArrow = 1;
            }
        }
        //������ �� ������ ���� �� ���� �� ���� �ð����� �÷��̾� Ž�� ����
        if (!OnFrontGround)
        {
            OnFrontGround = true;
            MoveArrow *= -1;
            StartCoroutine(StopScanPlayer());
        }

        //�� ������Ʈ �̵� ���� ����
        if (EManager.isKnockBack)
            isMove = false;
        else if (OnFrontGround)
            isMove = true;

        //MoveArrow�� ���� ���ý����� ����
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
        //Enemy �̵� ����
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
