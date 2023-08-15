using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D RBody;
    Color NColor = Color.red; //빨간색

    private int SkillDamage = 0; //스킬 데미지
    private float ShootingSpeed = 10f; // 투사체 속도
    private float KnockBackPower = 5f; // 투사체 넉백 파워
    private Vector2 MoveVector = new Vector2(1, 0); //투사체 방향
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
        RBody.velocity = MoveVector * ShootingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //투사체 플렛폼 통과
        if(other.gameObject.layer != LayerMask.NameToLayer("Platform"))
            Destroy(this.gameObject);
        if (other.gameObject.tag == "Player")
        {
            Transform thisE = gameObject.GetComponent<Transform>();//Enemy의 위치값
            PlayerController.instance.Hurt(SkillDamage, KnockBackPower, thisE);//플레이어 피격 함수 호출
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

    //투사체 넉백 파워를 컨트롤하는 함수
    public float UdateKnockBackPower
    {
        get
        {
            return KnockBackPower;
        }
        set
        {
            KnockBackPower = value;
        }
    }

    //투사체 이동 방향을 컨트롤 하는 함수
    public Vector2 UdateMoveVector
    {
        get
        {
            return MoveVector;
        }
        set
        {
            MoveVector = value;
        }
    }
}
