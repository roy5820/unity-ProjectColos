using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //싱클톤 패턴화
    public static GameManager instance = null;
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

    // Start is called before the first frame update
    void Start()
    {
        //
        PadeOutScreen();
        //시작 체력, 스태미나 최대체력, 스태미나로 설정
        NowHealth = MaxHealth;
        NowStamina = MaxStamina;
    }

    //싱클톤 패턴 초기화
    private void Awake()
    {
        instance = this;
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
    //페이트 인을 실행하는 함수
    public bool PadeInScreen()
    {
        Color AColor = PadeScreen.GetComponent<Image>().color;
        AColor.a = 1f;
        StartCoroutine(PadeImage(AColor, PadeInTime));
        return true;
    }
    //페이트 아웃을 실행하는 함수
    public bool PadeOutScreen()
    {
        Color AColor = PadeScreen.GetComponent<Image>().color;
        AColor.a = 0f;
        StartCoroutine(PadeImage(AColor, PadeOutTime));
        return true;
    }

    //이미지에 페이드 인/아웃 효과를 주는 코루틴 EndColor: 변경할 색, LimitTime: 페이드할 떄까지 걸리는 시간
    IEnumerator PadeImage(Color EndColor, float LimitTime)
    {
        float isTime = 0f;//시간제는 친구

        while(isTime <= LimitTime)
        {
            Color StartColor = PadeScreen.GetComponent<Image>().color;
            float NomalizedTime = isTime / LimitTime;

            PadeScreen.GetComponent<Image>().color = Color.Lerp(StartColor, EndColor, NomalizedTime);
            isTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
