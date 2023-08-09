using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    public Color NColor; //현재색깔
    GameObject NEffect; //현재 이펙트

    public int EnemyLayer; // 적 오브젝트 레이어
    public int ActObjLayer; // 활성화 오브젝트 레이어
    public int SkillDamage = 0; //스킬 데미지
    public float EfArrow = 1; //이펙트를 생성할 방향
    public GameObject AttackEfPreRed; //빨간색 공격 이펙트
    public GameObject AttackEfPreBlue; //파란색 공격 이펙트
    public GameObject AttackEfPreGreen; //초록색 공격 이펙트
    private GameObject AttackEf;

    // Start is called before the first frame update
    void Start()
    {
        //공격 이펙트 생성
        if (NColor == Color.red)
            NEffect = AttackEfPreRed;
        else if (NColor == Color.blue)
            NEffect = AttackEfPreBlue;
        else if (NColor == Color.green)
            NEffect = AttackEfPreGreen;

        if(NEffect != null)
        {
            AttackEf = Instantiate(NEffect);
            AttackEf.transform.position = new Vector2(this.transform.position.x + (1f * EfArrow), this.transform.position.y);
        }
       // AttackEf.transform.SetParent(this.transform);
    }

    void OnDestroy()
    {
        //if(AttackEf != null)
           // AttackEf.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ActObjLayer == other.gameObject.layer)
        {
            other.gameObject.SendMessage("ActAction", NColor); // 활성화 오브젝트 활성화 함수 호출
        }
        else if (EnemyLayer == other.gameObject.layer)
        {
            EnemyController Enemy = other.GetComponent<EnemyController>(); // 적 컨트롤러
            Enemy.HurtEnemy(SkillDamage, 0, null); //적피격 함수 호출
        }
    }
}
