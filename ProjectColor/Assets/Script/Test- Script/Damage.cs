using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int TrapDamage = 10;
    public float KnockbackForce = 5.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어 피격 함수 호출
            other.GetComponent<PlayerController>().Hurt(TrapDamage, KnockbackForce, transform);
        }
    }
}