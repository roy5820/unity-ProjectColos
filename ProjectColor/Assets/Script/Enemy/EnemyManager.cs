using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //Enemy UI관련 변수
    public GameObject EnemyCanvusPre; //불러올 프리펩
    GameObject EnemyCanvus;

    //Enemy의 hp를 관리하기 위한 변수 선언
    private Slider HpSlider;
    int MaxHp = 0;
    int NowHp = 0;

    //Enemy 상태 관련 변수
    public bool isHurt = false;
    public bool isKnockBack = false;

    //색깔 반응 관련 변수
    Image ReactionColorImage;
    public float ReColorReactionTime = 3.0f;
    bool isColorReaction = false; //색상 반응 여부
    Color NowReactionColor = Color.black;
    Color PurpleReaction = new Color(1, 0, 1, 1);
    Color YellowReaction = new Color(1, 1, 0, 1);
    Color SkyblueReaction = new Color(0, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        EnemyCanvus = Instantiate(EnemyCanvusPre, this.transform);//적 오브젝트 UI생성
        EnemyCanvus.transform.SetParent(null); //UI를 독립적으로 만들기
        HpSlider = EnemyCanvus.transform.FindChild("HpBar").GetComponent<Slider>(); 
        ReactionColorImage = EnemyCanvus.transform.FindChild("ReactionColor").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCanvus.transform.localPosition = this.transform.position;
        //설정된 최대체력 현제 체력 반영
        HpSlider.maxValue = MaxHp;
        HpSlider.value = NowHp;

        //색 적용
        ReactionColorImage.GetComponent<Image>().color = NowReactionColor;

        //반응 색깔 컨트롤
        if ((NowReactionColor == PurpleReaction || NowReactionColor == YellowReaction || NowReactionColor == SkyblueReaction) && !isColorReaction)
        {
            StartCoroutine(ColorReactionAction(NowReactionColor));
        }
    }

    //적 죽을 때 UI이도 같이 삭제
    private void OnDestroy()
    {
        Destroy(EnemyCanvus);
    }

    //색반응 액션 + 쿨타임 구현
    IEnumerator ColorReactionAction(Color ReactionColor)
    {
        //색깔별 데미지 계산
        int ColorReactionDamage = 0;
        if (ReactionColor == PurpleReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.PurpleReactionDamage);
        else if (ReactionColor == YellowReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.YellowReactionDamage);
        else if (ReactionColor == SkyblueReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.SkyblueReactionDamage);
        this.GetComponent<EnemyController>().HurtEnemy(ColorReactionDamage, 0, null);//데미지 부여
        
        isColorReaction = true; //색반용 여부 true
        yield return new WaitForSeconds(ReColorReactionTime);
        NowReactionColor = Color.black; //색 원상태로
        isColorReaction = false; //색반응 여부 false
    }

    //최대 Hp컨트롤
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
    //현재 Hp컨트롤
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

    //현재 반응 색깔 컨트롤
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
            }
        }
    }
}
