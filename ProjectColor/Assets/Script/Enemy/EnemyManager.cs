using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //Enemy UI���� ����
    public GameObject EnemyCanvusPre; //�ҷ��� ������
    GameObject EnemyCanvus;

    //Enemy�� hp�� �����ϱ� ���� ���� ����
    private Slider HpSlider;
    int MaxHp = 0;
    int NowHp = 0;

    //Enemy ���� ���� ����
    public bool isHurt = false;
    public bool isKnockBack = false;

    //���� ���� ���� ����
    Image ReactionColorImage;
    public float ReColorReactionTime = 3.0f;
    bool isColorReaction = false; //���� ���� ����
    Color NowReactionColor = Color.black;
    Color PurpleReaction = new Color(1, 0, 1, 1);
    Color YellowReaction = new Color(1, 1, 0, 1);
    Color SkyblueReaction = new Color(0, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        EnemyCanvus = Instantiate(EnemyCanvusPre, this.transform);//�� ������Ʈ UI����
        EnemyCanvus.transform.SetParent(null); //UI�� ���������� �����
        HpSlider = EnemyCanvus.transform.FindChild("HpBar").GetComponent<Slider>(); 
        ReactionColorImage = EnemyCanvus.transform.FindChild("ReactionColor").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCanvus.transform.localPosition = this.transform.position;
        //������ �ִ�ü�� ���� ü�� �ݿ�
        HpSlider.maxValue = MaxHp;
        HpSlider.value = NowHp;

        //�� ����
        ReactionColorImage.GetComponent<Image>().color = NowReactionColor;

        //���� ���� ��Ʈ��
        if ((NowReactionColor == PurpleReaction || NowReactionColor == YellowReaction || NowReactionColor == SkyblueReaction) && !isColorReaction)
        {
            StartCoroutine(ColorReactionAction(NowReactionColor));
        }
    }

    //�� ���� �� UI�̵� ���� ����
    private void OnDestroy()
    {
        Destroy(EnemyCanvus);
    }

    //������ �׼� + ��Ÿ�� ����
    IEnumerator ColorReactionAction(Color ReactionColor)
    {
        //���� ������ ���
        int ColorReactionDamage = 0;
        if (ReactionColor == PurpleReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.PurpleReactionDamage);
        else if (ReactionColor == YellowReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.YellowReactionDamage);
        else if (ReactionColor == SkyblueReaction)
            ColorReactionDamage = (int)Mathf.Round(PlayerController.instance.AttackPower * PlayerController.instance.SkyblueReactionDamage);
        this.GetComponent<EnemyController>().HurtEnemy(ColorReactionDamage, 0, null);//������ �ο�
        
        isColorReaction = true; //���ݿ� ���� true
        yield return new WaitForSeconds(ReColorReactionTime);
        NowReactionColor = Color.black; //�� �����·�
        isColorReaction = false; //������ ���� false
    }

    //�ִ� Hp��Ʈ��
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
    //���� Hp��Ʈ��
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

    //���� ���� ���� ��Ʈ��
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
