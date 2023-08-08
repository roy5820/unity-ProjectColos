using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D RBody;
    //적 체력관련 변수 선언
    private EnemyManager EManager;
    public int MaxHealth = 100;

    //적 피격 상태관련 변수 선언
    bool isHurt = false;

    //적 데미지 관련 변수 선업
    public int EnemyDamage = 6;
    public int EnemyAttackDamage = 10;
    public float EnemyKnockBackPower = 5;

    public GameObject GroundSencor;
    public GameObject PlatformSencor;

    bool OnGround = false;
    bool OnPlatform = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        //실시간 센서 상태 업데이트
        OnGround = GroundSencor.GetComponent<PlayerSensor>().colSencorState();
        OnPlatform = PlatformSencor.GetComponent<PlayerSensor>().colSencorState();

        //캐릭터가 히트상태가 아닐 시 안밀리게 하는 임시 코드
        if (!isHurt && (OnGround || OnPlatform))
        {
            RBody.velocity = new Vector2(0,0);
        }
    }

    //피격함수
    public void HurtEnemy(int Damage, float KbPower, Transform ohterT)
    {
        isHurt = true;//피격상태 설정
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
            if (EManager.GSNowHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //히트 시 넉백 구현
    IEnumerator KnockBackTimer()
    {
        float isTime = 0f;

        while (isTime < 0.3f)
        {
            isTime += Time.deltaTime;

            yield return null;
        }

        //넉백이후 죽음
        if (EManager.GSNowHp <= 0)
        {
            Destroy(this.gameObject);
        }

        isHurt = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //플레이어와 충돌시 데미지
        if (other.gameObject.tag == "Player")
        {
            Transform thisE = gameObject.GetComponent<Transform>();//Enemy의 위치값
            PlayerController.instance.Hurt(EnemyDamage, EnemyKnockBackPower, thisE);//플레이어 피격 함수 호출
        }
    }
}
