using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D RBody;
    private EnemyManager EManager;// ���ʹ� �޴��� ����
    
    //�� ü�°��� ���� ����
    public int MaxHealth = 100;

    //�� ������ ���� ���� ����
    public int EnemyDamage = 6;
    public int EnemyAttackDamage = 10;
    public float EnemyKnockBackPower = 5;

    //���� �ִϸ��̼�
    string NowAni = "";
    Animator EnemyAni;
    public GameObject EnemySprite;

    // Start is called before the first frame update
    void Start()
    {
        //�ش� ��ü�� ������ٵ� �� �ʱ�ȭ
        RBody = this.GetComponent<Rigidbody2D>();
        //EnemyManager ����
        EManager = GetComponent<EnemyManager>();
        //�ʱ� Enemyü�� �� ����
        EManager.GSMaxHp = MaxHealth;
        EManager.GSNowHp = MaxHealth;
        //�ִϸ����� ����
        EnemyAni = EnemySprite.GetComponent<Animator>();
    }

    private void Update()
    {
        //���º� �ִϸ��̼� ����
        if (EManager.isAttack)
            NowAni = "isAttack";
        if (EManager.isMove)
            NowAni = "isWalk";

        if (NowAni == "isAttack")
            EnemyAni.SetBool("isAttack", true);
        else
            EnemyAni.SetBool("isAttack", false);

        if (NowAni == "isWalk")
            EnemyAni.SetBool("isWalk", true);
        else
            EnemyAni.SetBool("isWalk", false);

    }

    private void OnDestroy()
    {
        GameManager.instance.KillEnemyCnt++;
    }

    //�ǰ��Լ�
    public void HurtEnemy(int Damage, float KbPower, Transform ohterT)
    {
        EManager.isHurt = true;//�ǰݻ��� ����
        EManager.GSNowHp = EManager.GSNowHp - Damage;//�ǰ� ������ �ݿ�
        
        if(ohterT != null)
        {
            float x = transform.position.x - ohterT.position.x;

            x = x < 0 ? -1 : 1;

            RBody.velocity = new Vector2(0, 0);

            Vector2 KnockBackV = new Vector2(x, 1);
            RBody.AddForce(KnockBackV * KbPower, ForceMode2D.Impulse);

            StartCoroutine(KnockBackTimer());
        }
        else
        {
            EManager.isHurt = false;
            if (EManager.GSNowHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //��Ʈ �� �˹� ����
    IEnumerator KnockBackTimer()
    {
        EManager.isKnockBack = true;//�˹���� ����
        float isTime = 0f;

        while (isTime < 0.5f)
        {
            isTime += Time.deltaTime;

            yield return null;
        }

        //�˹����� ����
        if (EManager.GSNowHp <= 0)
        {
            Destroy(this.gameObject);
        }

        EManager.isHurt = false;
        EManager.isKnockBack = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //�÷��̾�� �浹�� ������
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController;//�÷��̾� ��Ʈ�ѷ� ������ ���� ���� ����
            //�÷��̾���Ʈ�ѷ� ���� �ʱ�ȭ
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            Transform thisE = gameObject.GetComponent<Transform>();//Enemy�� ��ġ��
            playerController.Hurt(EnemyDamage, EnemyKnockBackPower, thisE);//�÷��̾� �ǰ� �Լ� ȣ��
        }
    }
}
