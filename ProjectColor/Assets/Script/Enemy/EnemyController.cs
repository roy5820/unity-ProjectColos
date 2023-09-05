using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D RBody;
    private EnemyManager EManager;// 에너미 메니저 연결
    
    //적 체력관련 변수 선언
    public int MaxHealth = 100;

    //적 데미지 관련 변수 선업
    public int EnemyDamage = 6;
    public int EnemyAttackDamage = 10;
    public float EnemyKnockBackPower = 5;

    //현제 애니메이션
    string NowAni = "";
    Animator EnemyAni;
    public GameObject EnemySprite;

    // Start is called before the first frame update
    void Start()
    {
        //해당 객체의 리지드바디 값 초기화
        RBody = this.GetComponent<Rigidbody2D>();
        //EnemyManager 연결
        EManager = GetComponent<EnemyManager>();
        //초기 Enemy체력 값 설정
        EManager.GSMaxHp = MaxHealth;
        EManager.GSNowHp = MaxHealth;
        //애니메이털 연결
        EnemyAni = EnemySprite.GetComponent<Animator>();
    }

    private void Update()
    {
        //상태별 애니메이션 관리
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

    //피격함수
    public void HurtEnemy(int Damage, float KbPower, Transform ohterT)
    {
        EManager.isHurt = true;//피격상태 설정
        EManager.GSNowHp = EManager.GSNowHp - Damage;//피격 데미지 반영
        
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

    //히트 시 넉백 구현
    IEnumerator KnockBackTimer()
    {
        EManager.isKnockBack = true;//넉백상태 설정
        float isTime = 0f;

        while (isTime < 0.5f)
        {
            isTime += Time.deltaTime;

            yield return null;
        }

        //넉백이후 죽음
        if (EManager.GSNowHp <= 0)
        {
            Destroy(this.gameObject);
        }

        EManager.isHurt = false;
        EManager.isKnockBack = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //플레이어와 충돌시 데미지
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController;//플레이어 컨트롤러 연결을 위한 변수 선언
            //플레이어컨트롤러 변수 초기화
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            Transform thisE = gameObject.GetComponent<Transform>();//Enemy의 위치값
            playerController.Hurt(EnemyDamage, EnemyKnockBackPower, thisE);//플레이어 피격 함수 호출
        }
    }
}
