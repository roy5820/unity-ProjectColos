using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;   // �� ������
    public GameObject trapObj;   // ���� ������Ʈ
    public GameObject trapBewareObj;   // ���� ���� ������Ʈ

    public Transform[] rainSpawnPoints; // �� ���� ��ġ �迭

    public float trapSpawnInterval = 10.0f; // �ʱ� ���� ���� �ð�
    public float rainSpawnInterval = 0.6f; // �� ���� ����
    public float rainSpawnTime = 2f; // �� ���� �ð�

    public float BossRandomAttackTimeS = 5f; // ���� ��ų �ֱ� �ּ� ��
    public float BossRandomAttackTimeE = 10f;// ���� ��ų �ֱ� �ִ� ��

    private float trapTimer; // ���� ���� Ÿ�̸�
    private float rainTimer; // �� ���� �ֱ� Ÿ�̸�
    private float rainIntervalTimer; // �� ���� Ÿ�̸�

    

    private void Start()
    {
        trapTimer = trapSpawnInterval;
        rainIntervalTimer = rainSpawnInterval;
        rainTimer = rainSpawnTime;
    }

    private void Update()
    {
        // ���� ���� Ÿ�̸� ����
        trapTimer -= Time.deltaTime;
        // �� ���� Ÿ�̸� ����
        rainIntervalTimer -= Time.deltaTime;
        rainTimer -= Time.deltaTime;

        // ���� ���ݸ��� ���� ����
        if (trapTimer <= 0)
        {
            SpawnTrap();
            trapTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        }

        //�����
        if (rainTimer <= rainSpawnTime)
        {
            // ���� ���ݸ��� �� ����
            if (rainIntervalTimer <= 0)
            {

                SpawnRain();
                rainIntervalTimer = rainSpawnInterval;
            }
            if(rainTimer <= 0)
                rainTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        }
    }

    // ���� ���� �޼���
    void SpawnTrap()
    {
        
    }

    // �� ���� �޼���
    void SpawnRain()
    {
        // ������ ��ġ���� �� ����
        int randomIndex = Random.Range(0, rainSpawnPoints.Length);
        GameObject rain = Instantiate(rainPrefab);
        rain.transform.position = rainSpawnPoints[randomIndex].position;
        //Debug.Log("������");
    }
}
