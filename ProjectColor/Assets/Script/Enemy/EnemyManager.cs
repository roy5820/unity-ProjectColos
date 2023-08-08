using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //Enemy의 hp를 관리하기 위한 변수 선언
    [SerializeField] private Slider HpSlider;
    int MaxHp = 0;
    int NowHp = 0;

    //색깔 반응 관련 변수
    public Image ReactionColorImage;
    public float ReColorReactionTime = 3.0f;
    bool isColorReaction = false; //색상 반응 여부
    Color NowReactionColor = Color.black;
    Color PurpleReaction = new Color(1, 0, 1, 1);
    Color YellowReaction = new Color(1, 1, 0, 1);
    Color SkyblueReaction = new Color(0, 1, 1, 1);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    //색반응 쿨타임 후 다시 색칠해 지게 만들기
    IEnumerator ColorReactionAction(Color ReactionColor)
    {
        this.GetComponent<EnemyController>().HurtEnemy(30, 0, null);//데미지 부여
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
