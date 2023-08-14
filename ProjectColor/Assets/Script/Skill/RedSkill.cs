using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkill : MonoBehaviour
{
    Rigidbody2D RBody;
    Color NColor = Color.red; //빨간색

    public int EnemyLayer; // 적 오브젝트 레이어
    public int ActObjLayer; // 활성화 오브젝트 레이어
    private int SkillDamage = 0; //스킬 데미지
    private float ShootingSpeed = 10f; // 투사체 속도
    public float SpawnArrow = 1; //오브젝트 생성할 위치(투사체 방향, 이펙트 생성위치 설정)
    public GameObject AttackEfPre; //공격 이펙트 프리펩
    private GameObject AttackEf; // 생성한 공격 이펙트 프리팹



    // Start is called before the first frame update
    void Start()
    {
        //공격 이펙트 생성
        //AttackEf = Instantiate(AttackEfPre);
        //AttackEf.transform.position = new Vector2(this.transform.position.x + (0.5f * EfArrow), this.transform.position.y);

        RBody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RBody.velocity = new Vector2(ShootingSpeed * SpawnArrow, RBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ladder"))
            Destroy(this.gameObject);
        if (ActObjLayer == other.gameObject.layer)
        {
            other.gameObject.SendMessage("ActAction", NColor); // 활성화 오브젝트 활성화 함수 호출

        }
        else if (EnemyLayer == other.gameObject.layer)
        {
            EnemyController Enemy = other.GetComponent<EnemyController>(); // 적 컨트롤러
            Enemy.HurtEnemy(SkillDamage, 0, null); //적피격 함수 호출

            other.GetComponent<EnemyManager>().GSNowReactionColor = NColor; //적 색반응 적용
        }
    }



    //데미지 값을 컨트롤 하는 함수
    public int UdateDamege
    {
        get
        {
            return SkillDamage;
        }
        set
        {
            SkillDamage = value;
        }
    }

    //투사체 속도를 컨트롤하는 함수
    public float UdateShootingSpeed
    {
        get
        {
            return ShootingSpeed;
        }
        set
        {
            ShootingSpeed = value;
        }
    }
}