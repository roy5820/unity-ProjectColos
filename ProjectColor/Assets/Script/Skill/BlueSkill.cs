using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSkill : MonoBehaviour
{
    Color NColor = Color.blue; //�Ķ���

    public int EnemyLayer; // �� ������Ʈ ���̾�
    public int ActObjLayer; // Ȱ��ȭ ������Ʈ ���̾�
    private int SkillDamage = 0; //��ų ������
    public float EfArrow = 1; //����Ʈ�� ������ ��ġ
    public GameObject AttackEfPre; //���� ����Ʈ ������
    private GameObject AttackEf; // ������ ���� ����Ʈ ������


    // Start is called before the first frame update
    void Start()
    {
        //���� ����Ʈ ����
        //AttackEf = Instantiate(AttackEfPre);
        //AttackEf.transform.position = new Vector2(this.transform.position.x + (0.5f * EfArrow), this.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ActObjLayer == other.gameObject.layer)
        {
            other.gameObject.SendMessage("ActAction", NColor); // Ȱ��ȭ ������Ʈ Ȱ��ȭ �Լ� ȣ��
            
        }
        else if (EnemyLayer == other.gameObject.layer)
        {
            if (other.tag == "Boss")
            {
                BossController Boss = other.GetComponent<BossController>();
                Boss.HurtBoss(SkillDamage, NColor);
            }
            else
            {
                EnemyController Enemy = other.GetComponent<EnemyController>(); // �� ��Ʈ�ѷ�
                Enemy.HurtEnemy(SkillDamage, 2, this.gameObject.transform); //���ǰ� �Լ� ȣ��

                other.GetComponent<EnemyManager>().GSNowReactionColor = NColor; //�� ������ ����
            }
        }
    }
    
    public int UdateDamege {
        get
        {
            return SkillDamage;
        }
        set
        {
            SkillDamage = value;
        }
    }

}
