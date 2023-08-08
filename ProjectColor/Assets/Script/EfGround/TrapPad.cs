using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPad : MonoBehaviour
{
    public float KnockbackForce = 10f; //넉백 파워
    public int TrapDamage = 10; // 트랩 데미지
    Transform thisT; // 트랩의 위치값


    private void Update()
    {
        thisT = gameObject.GetComponent<Transform>();//트랩의 위치값 업데이트
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.instance.Hurt(TrapDamage, KnockbackForce, thisT);//플레이어 피격함수 호출
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyController Enemy = other.GetComponent<EnemyController>(); // 적 컨트롤러
            Enemy.HurtEnemy(TrapDamage, KnockbackForce, thisT); // 적 피격함수 호출

        }
    }
}
