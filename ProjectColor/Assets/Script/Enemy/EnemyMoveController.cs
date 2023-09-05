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
    public float MoveArrow = 1;   //�� ������Ʈ�� �̵� ����

    //�ٴ� üũ ����
    public GameObject GroundSensor;
    public GameObject PlatformSensor;
    bool OnGround = false;

    //���� üũ ���� 
    public GameObject FrontGroundSensor;
    public GameObject FrontPlatformSensor;
    bool OnFrontGround = false;

    //�� üũ ����
    public GameObject WallSensor;
    bool OnFrontWall = false;

    //�÷��̾� Ž�� ����
    bool ScanPlayer = true;
    float StapScanTime = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        EManager = GetComponent<EnemyManager>();
    }

    private void Update()
    {
        // ���� üũ
        if (ScanPlayer)
        {
            OnFrontGround = FrontGroundSensor.GetComponent<Sensor>().colSencorState() || FrontPlatformSensor.GetComponent<Sensor>().colSencorState();
            OnFrontWall = WallSensor.GetComponent<Sensor>().colSencorState();
        }

        // �÷��̾� ����
        if(GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;// �� �� �Ÿ� ���

            //�� ������Ʈ �̵� ���� ����
            //�÷��̾� Ž�� �Ͽ� �ش� �������� �̵�
            if (distanceToPlayer < detectionRange && ScanPlayer)
            {
                // �÷��̾ �������� MoveArrow ���� + ���ݽ� ���� ��ȯ X
                if (OnFrontGround && !EManager.isAttack)
                {
                    if (direction.x < -0.1f)
                        MoveArrow = -1;
                    else if (direction.x > 0.1f)
                        MoveArrow = 1;
                }
            }
        }

        //������ �� ������ ���� �� ���� �� ���� �ð����� �÷��̾� Ž�� ���� + ��� ���� �ڸ��� ������ ����
        if (!OnFrontGround || OnFrontWall)
        {
            if(!OnFrontGround)
                OnFrontGround = true;
            if(OnFrontWall)
                OnFrontWall = false;
            MoveArrow *= -1;
            StartCoroutine(StopScanPlayer());
        }

        //�� ������Ʈ �̵� ���� ����
        if (EManager.isKnockBack)
            EManager.isMove = false;
        else if (EManager.isAttack)//���� �� ����
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            EManager.isMove = false;
        }
        else if (OnFrontGround)
            EManager.isMove = true;

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
        if (EManager.isMove)
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