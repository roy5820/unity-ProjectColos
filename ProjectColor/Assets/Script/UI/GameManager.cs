using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //싱클톤 패턴화
    public static GameManager instance = null;

    // 씬 변환 시 체크할 이벤트 변수
    public event Action OnSceneChanged;

    // 색깔 선택 관련
    public Image nowColor;
    public Slider HealthBar;
    public Slider StaminaGage;

    bool inputNcolor;

    int NcolorCode = 3;// 현재색깔
    public Color NColor;
    
    //체력바
    public int MaxHealth = 100;
    public int NowHealth;

    // 스태미나 관리
    public int MaxStamina = 100;
    public int NowStamina;
    float staminaTimer;
    public float recoveryCycle = 0.2f; //스태미나 회복주기
    public int recoveryAmount = 3; //회복량

    //페이드 인 아웃 스크린
    public GameObject PadeScreen;
    public float PadeInTime = 1.5f;
    public float PadeOutTime = 1.5f;

    //아이템 메니저
    private ItemManager itemManager;
    public GameObject itemPanel;//아이템 선택 팝업 페널
    bool isSelectItem = false; // 아이템 선택 여부

    //플레이어 스폰 관련
    public GameObject PlayerPre;
    private GameObject SpawnPoint;
    GameObject FindPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //아이템 메니저 초기화
        itemManager = this.GetComponent<ItemManager>();

        //시작 체력, 스태미나 최대체력, 스태미나로 설정
        NowHealth = MaxHealth;
        NowStamina = MaxStamina;

        //씬 시작 시 페이드 아웃 적용
        StartCoroutine(PadeImageAndChangeScene(PadeOutTime, null));

        //플레이어 스폰
        FindPlayer = GameObject.FindWithTag("Player");
        SpawnPoint = GameObject.Find("SpawnPoint");
        if (FindPlayer == null)
        {
            FindPlayer = Instantiate(PlayerPre);
            FindPlayer.GetComponent<Transform>().position = SpawnPoint.GetComponent<Transform>().position;
        }
        
    }

    private void Awake()
    {
        // 다른 씬으로 넘어가더라도 이 객체를 유지하기 위해 싱글톤 패턴을 사용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 이 객체 파괴
        }
    }

    // 다른 씬으로 이동할 때 이벤트 호출
    public void ChangeScene()
    {
        //씬 시작 시 페이드 아웃 적용
        StartCoroutine(PadeImageAndChangeScene(PadeOutTime, null));
        SpawnPoint = GameObject.Find("SpawnPoint");

        //플레이어 스폰
        if (FindPlayer != null)
        {
            FindPlayer.SetActive(true);
            FindPlayer.GetComponent<Transform>().position = SpawnPoint.GetComponent<Transform>().position;
        }

        // 이벤트 발생
        OnSceneChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        inputNcolor = Input.GetKeyDown(KeyCode.C);
        
        if (inputNcolor)
        {
            NcolorCode += 3;

            if (NcolorCode > 10)
            {
                NcolorCode -= 9;
            }
        }

        //Ncolor에 따른 색 변경
        switch (NcolorCode)
        {
            case 3:
                NColor = Color.red;
                nowColor.color = NColor;
                break;
            case 6:
                NColor = Color.green;
                nowColor.color = NColor;
                break;
            case 9:
                NColor = Color.blue;
                nowColor.color = NColor;
                break;
        }

        //스태미나 관리
        staminaTimer += Time.deltaTime;
        if (staminaTimer > recoveryCycle)
        {
            
            if (NowStamina >= 100) NowStamina = 100;
            else NowStamina += recoveryAmount;
            staminaTimer = 0f;
        }

        StaminaGage.value = NowStamina;
        StaminaGage.maxValue = MaxStamina;

        //체력바 관리 PlayerMove에서 체력 관리
        HealthBar.value = NowHealth;
        HealthBar.maxValue = MaxHealth;
    }

    //스태미나 관리
    public int PlayerStamina
    {
        get
        {
            return NowStamina;
        }
        set
        {
            NowStamina -= value;
        }


    }

    //체력 속성
    public int PlayerHp
    {
        get
        {
            return NowHealth;
        }
        set
        {
            NowHealth -= value;
            if (NowHealth < 0) NowHealth = 0;
        }
    }
    //씬 체인지 함수
    public bool ChangScen(string SceneName)
    {
        
        StartCoroutine(PadeImageAndChangeScene(PadeInTime, SceneName));
        return true;
    }


    //씬 전환 및 씬 전환 시 페이드 인, 아웃을 주는 코루틴 
    //LimitTime: 페이드할 떄까지 걸리는 시간, SceneName: 이동할 씬 이름 없으면, 페이드 아웃 적용, GetItem: 아이템 획득 여부
    IEnumerator PadeImageAndChangeScene(float LimitTime, string SceneName)
    {
        float isTime = 0f;//시간제는 친구
        Color StartColor = PadeScreen.GetComponent<Image>().color; //시작 색깔 설정
        //이동할 씬 값에 따라 변경할 목표 색 설정
        Color AColor = StartColor;

        if(SceneName == null)
            AColor.a = 0f;
        else
            AColor.a = 1f;
        while (isTime <= LimitTime)
        {
            
            float NomalizedTime = isTime / LimitTime;

            PadeScreen.GetComponent<Image>().color = Color.Lerp(StartColor, AColor, NomalizedTime);
            isTime += Time.deltaTime;
            yield return null;
        }
        //페이드 적용 이후 씬 이동
        if(SceneName != null)
            SceneManager.LoadScene(SceneName);
    }

    //아이템 선택창 생성 코루틴
    public IEnumerator SelectItemAndChangeScene(string SceneName)
    {
        itemPanel.SetActive(true);//아이템 페널 활성화
        GameObject.FindWithTag("Player").SetActive(false); //플레이어 비활성화

        //아이템 패널의 자식들을 for문으로 배열에 할당
        int childCnt = itemPanel.transform.childCount;
        GameObject[] items = new GameObject[childCnt];
        for (int i = 0; i < childCnt; i++)
            items[i] = itemPanel.transform.GetChild(i).gameObject;

        //아이템 데이터 베이스에서 데이터 가져오기
        foreach(GameObject item in items)
        {
            Item randomItem = itemManager.GetRandomItem();//아이템 데이터 베이스에서 아이템 정보 추출

            if (randomItem == null)//데이터 베이스에 아이템이 없으면 break
                break;
            
            item.transform.GetChild(0).GetComponent<Image>().sprite = randomItem.icon;//아이콘 설정
            item.transform.GetChild(1).GetComponent<Text>().text = randomItem.itemName;//아이템 이름 설정
            item.transform.GetChild(2).GetComponent<Text>().text = randomItem.description;//아이템 설정
            item.transform.GetChild(3).GetComponent<Text>().text = randomItem.executionFunction;//아이템 함수 설정
        }

        //아이템 선택 할때까지 무겐 지옥
        while (!isSelectItem)
        {
            yield return null;
        }

        StartCoroutine(PadeImageAndChangeScene(PadeInTime, SceneName));
    }

    //아이템 선택 함수
    public void SelectItem(GameObject thisObj)
    {
        itemPanel.SetActive(false);//아이템 페널 비활성화
        isSelectItem = true;//아이템 선택 여부 활성화

        this.SendMessage(thisObj.transform.GetChild(3).GetComponent<Text>().text);
    }
}
