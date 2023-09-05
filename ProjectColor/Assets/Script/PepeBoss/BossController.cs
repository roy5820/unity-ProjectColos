using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;   // 비 프리팹
    public GameObject trapObj;   // 함정 오브젝트
    public GameObject trapBewareObj;   // 함정 주의 오브젝트

    public Transform[] rainSpawnPoints; // 비 생성 위치 배열

    public float trapSpawnInterval = 0.3f; // 함정 생성 시간
    public float trapBewareSpawnTime = 1f; // 함정 경고 생성 시간
    public float rainSpawnInterval = 0.6f; // 비 생성 간격
    public float rainSpawnTime = 2f; // 비 생성 시간

    public float BossRandomAttackTimeS = 5f; // 보스 스킬 주기 최솟 값
    public float BossRandomAttackTimeE = 10f;// 보스 스킬 주기 최댓 값

    private float trapTimer; // 함정 생성 타이머
    private float rainTimer; // 비 생성 주기 타이머
    private float rainIntervalTimer; // 비 생성 타이머

    //Boss UI생성 관련 변수
    public GameObject EnemyCanvus;

    //Enemy UI 설정 관련
    private Slider HpSlider;
    public int MaxHp = 100;
    int NowHp;

    public string ChangScenName; // 보스 처치 시 이동할 스테이지

    //색반응 관련 설정
    Image ReactionColorImage;
    public float ReColorReactionTime = 3.0f;
    bool isColorReaction = false; //색반응 여부
    Color NowReactionColor = Color.black;
    Color PurpleReaction = new Color(1, 0, 1, 1);
    Color YellowReaction = new Color(1, 1, 0, 1);
    Color SkyblueReaction = new Color(0, 1, 1, 1);

    //보스 애니메이션
    Animator BossAni;
    bool isAttack = false;


    private void Start()
    {
        //바닥 트랩생성 패턴 관련 변수 초기화
        trapTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        //몬스터 비 패턴 관련 변수 초기화
        rainTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
        rainIntervalTimer = rainSpawnInterval;
        //UI관련 설정 값 초기화
        HpSlider = EnemyCanvus.transform.GetChild(0).Find("HpBar").GetComponent<Slider>();
        ReactionColorImage = EnemyCanvus.transform.GetChild(0).Find("ReactionColor").GetComponent<Image>();
        NowHp = MaxHp;
        //보스 애니메이터 연결
        BossAni = this.GetComponent<Animator>();
    }

    private void Update()
    {
        // 함정 생성 타이머 감소
        trapTimer -= Time.deltaTime;
        // 비 생성 타이머 감소
        rainIntervalTimer -= Time.deltaTime;
        rainTimer -= Time.deltaTime;
            

        //일정  간격마다 함정 경고 생성
        if(trapTimer <= trapBewareSpawnTime)
        {
            isAttack = true;
            trapBewareObj.SetActive(true);
            // 일정 간격마다 함정 생성
            if (trapTimer <= 0)
            {
                isAttack = false;
                trapBewareObj.SetActive(false);
                SpawnTrap();
                Invoke("SpawnTrap", trapSpawnInterval);
                trapTimer = Random.Range(BossRandomAttackTimeS + rainSpawnTime, BossRandomAttackTimeE + rainSpawnTime);
            }
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

        EnemyCanvus.transform.localPosition = this.transform.position;
        //적 체력 업데이트
        HpSlider.maxValue = MaxHp;
        HpSlider.value = NowHp;

        //색 반응 업데이트
        ReactionColorImage.GetComponent<Image>().color = NowReactionColor;

        //색반응 시 효과 적용
        if ((NowReactionColor == PurpleReaction || NowReactionColor == YellowReaction || NowReactionColor == SkyblueReaction) && !isColorReaction)
        {
            isColorReaction = true; //색반응 여부 true
            StartCoroutine(ColorReactionAction(NowReactionColor));
        }
        //보스 사망 시 2페이지로 씬 이동
        if (NowHp <= 0)
        {
            if(GameObject.FindWithTag("Player") != null)
                GameObject.FindWithTag("Player").SetActive(false);
            GameManager.instance.ChangScen(ChangScenName);
            Destroy(this.gameObject);
        }

        //애니메이션 일괄 처리
        if(isAttack == true)
        {
            BossAni.SetBool("isAttack", true);
        }
        else
        {
            BossAni.SetBool("isAttack", false);
        }
    }

    //색반응 액션 구현
    IEnumerator ColorReactionAction(Color ReactionColor)
    {
        //플레이어 컨트롤러 연결
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        //색 반응 데미지 계산
        int ColorReactionDamage = 0;
        if (ReactionColor == PurpleReaction)
            ColorReactionDamage = (int)Mathf.Round(playerController.AttackPower * playerController.PurpleReactionDamage);
        else if (ReactionColor == YellowReaction)
            ColorReactionDamage = (int)Mathf.Round(playerController.AttackPower * playerController.YellowReactionDamage);
        else if (ReactionColor == SkyblueReaction)
            ColorReactionDamage = (int)Mathf.Round(playerController.AttackPower * playerController.SkyblueReactionDamage);
        
        HurtBoss(ColorReactionDamage, Color.black);//데미지 구현
        

        yield return new WaitForSeconds(ReColorReactionTime);

        NowReactionColor = Color.black; //색 초기화
        isColorReaction = false; //색 반응 여부 false
    }

    // 함정 생성 메서드
    void SpawnTrap()
    {
        if(trapObj.activeSelf == false)
            trapObj.SetActive(true);
        else if (trapObj.activeSelf == true)
            trapObj.SetActive(false);

    }

    // 비 생성 메서드
    void SpawnRain()
    {
        // 랜덤한 위치에서 비 생성
        int randomIndex = Random.Range(0, rainSpawnPoints.Length);
        GameObject rain = Instantiate(rainPrefab);
        rain.transform.position = rainSpawnPoints[randomIndex].position;
    }

    //피격함수
    public void HurtBoss(int Damage, Color isColor)
    {
        NowHp = NowHp - Damage;//피격 데미지 반영

        if(isColor != Color.black && !isColorReaction)
        {
            NowReactionColor += isColor;
            if (NowReactionColor.a == 2)
            {
                NowReactionColor.a -= 1;
            }
            if (NowReactionColor.r == 2)
            {
                NowReactionColor.r -= 1;
            }
            if (NowReactionColor.b == 2)
            {
                NowReactionColor.b -= 1;
            }
            if (NowReactionColor.g == 2)
            {
                NowReactionColor.g -= 1;
            }
        }
    }
}
