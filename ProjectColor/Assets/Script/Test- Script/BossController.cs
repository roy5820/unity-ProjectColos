using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;   // 비 프리팹
    public GameObject trapObj;   // 함정 오브젝트
    public GameObject trapBewareObj;   // 함정 주의 오브젝트

    public Transform[] rainSpawnPoints; // 비 생성 위치 배열

    public float trapSpawnInterval = 10.0f; // 초기 함정 생성 시간
    public float rainSpawnInterval = 0.6f; // 비 생성 간격
    public float rainSpawnTime = 2f; // 비 생성 시간

    public float BossRandomAttackTimeS = 5f; // 보스 스킬 주기 최솟 값
    public float BossRandomAttackTimeE = 10f;// 보스 스킬 주기 최댓 값

    private float trapTimer; // 함정 생성 타이머
    private float rainTimer; // 비 생성 주기 타이머
    private float rainIntervalTimer; // 비 생성 타이머

    

    private void Start()
    {
        trapTimer = trapSpawnInterval;
        rainIntervalTimer = rainSpawnInterval;
        rainTimer = rainSpawnTime;
    }

    private void Update()
    {
        // 함정 생성 타이머 감소
        trapTimer -= Time.deltaTime;
        // 비 생성 타이머 감소
        rainIntervalTimer -= Time.deltaTime;
        rainTimer -= Time.deltaTime;

        // 일정 간격마다 함정 생성
        if (trapTimer <= 0)
        {
            SpawnTrap();
            trapTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        }

        //비생성
        if (rainTimer <= rainSpawnTime)
        {
            // 일정 간격마다 비 생성
            if (rainIntervalTimer <= 0)
            {

                SpawnRain();
                rainIntervalTimer = rainSpawnInterval;
            }
            if(rainTimer <= 0)
                rainTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        }
    }

    // 함정 생성 메서드
    void SpawnTrap()
    {
        
    }

    // 비 생성 메서드
    void SpawnRain()
    {
        // 랜덤한 위치에서 비 생성
        int randomIndex = Random.Range(0, rainSpawnPoints.Length);
        GameObject rain = Instantiate(rainPrefab);
        rain.transform.position = rainSpawnPoints[randomIndex].position;
        //Debug.Log("날린다");
    }
}
