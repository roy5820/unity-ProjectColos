using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //Enemy UI생성 관련 변수
    public GameObject EnemyCanvusPre; //적 UI 프리펩
    GameObject EnemyCanvus;

    //Enemy UI 설정 관련
    private Slider HpSlider;
    int MaxHp = 0;
    int NowHp = 0;

    //Enemy 상태관련 변수
    public bool isHurt = false;//피격 여부
    public bool isKnockBack = false;//넉백 여부
    public bool isMove = false; //이동 여부
    public bool isAttack = false; //공격 여부

    //색반응 관련 설정
    Image ReactionColorImage;
    public float ReColorReactionTime = 3.0f;
    bool isColorReaction = false; //색반응 여부
    Color NowReactionColor = Color.black;
    Color PurpleReaction = new Color(1, 0, 1, 1);
    Color YellowReaction = new Color(1, 1, 0, 1);
    Color SkyblueReaction = new Color(0, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        EnemyCanvus = Instantiate(EnemyCanvusPre, this.transform);//적 UI 생성
        EnemyCanvus.transform.SetParent(null); //UI 독립화
        HpSlider = EnemyCanvus.transform.Find("HpBar").GetComponent<Slider>(); 
        ReactionColorImage = EnemyCanvus.transform.Find("ReactionColor").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    //적 죽을 시 적UI 삭제
    private void OnDestroy()
    {
        Destroy(EnemyCanvus);
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
        this.GetComponent<EnemyController>().HurtEnemy(ColorReactionDamage, 0, null);//데미지 구현
        
        yield return new WaitForSeconds(ReColorReactionTime);

        NowReactionColor = Color.black; //색 초기화
        isColorReaction = false; //색 반응 여부 false
    }

    //현재 HP 컨트롤 함수
    public int GSMaxHp
    {
        get
        {
            return MaxHp;
        }
        set
        {
            MaxHp = value;
        }
    }
    //최대 HP 컨트롤 함수
    public int GSNowHp
    {
        get
        {
            return NowHp;
        }
        set
        {
            NowHp = value;
        }
    }

    //반응 색 컨트롤 함수
    public Color GSNowReactionColor
    {
        get
        {
            return NowReactionColor;
        }
        set
        {
            if (!isColorReaction)
            {
                NowReactionColor += value;
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
}
