using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSkill : MonoBehaviour
{
    Color NColor = Color.green; //초록색

    public int EnemyLayer; // 적 오브젝트 레이어
    public int ActObjLayer; // 활성화 오브젝트 레이어
    private int SkillDamage = 0; //스킬 데미지
    private float KnockBackPower = 0f; // 넉백 파워
    public float EfArrow = 1; //이펙트를 생성할 위치
    public GameObject AttackEfPre; //공격 이펙트 프리펩
    private GameObject AttackEf; // 생성한 공격 이펙트 프리팹



    // Start is called before the first frame update
    void Start()
    {
        //공격 이펙트 생성
        //AttackEf = Instantiate(AttackEfPre);
        //AttackEf.transform.position = new Vector2(this.transform.position.x + (0.5f * EfArrow), this.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ActObjLayer == other.gameObject.layer)
        {
            other.gameObject.SendMessage("ActAction", NColor); // 활성화 오브젝트 활성화 함수 호출
            
        }
        else if (EnemyLayer == other.gameObject.layer)
        {
            if(other.tag == "Boss")
            {
                BossController Boss = other.GetComponent<BossController>();
                Boss.HurtBoss(SkillDamage, NColor);
            }
            else
            {
                EnemyController Enemy = other.GetComponent<EnemyController>(); // 적 컨트롤러
                Enemy.HurtEnemy(SkillDamage, KnockBackPower, this.gameObject.transform); //적피격 함수 호출

                other.GetComponent<EnemyManager>().GSNowReactionColor = NColor; //적 색반응 적용
            }
        }
    }
    //데미지 값을 컨트롤 하는 함수
    public int UdateDamege {
        get
        {
            return SkillDamage;
        }
        set
        {
            SkillDamage = value;
        }
    }
    //넉백파워를 컨트롤하는 함수
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
}
