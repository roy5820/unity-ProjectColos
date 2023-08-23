using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    public Color NColor; //�������
    GameObject NEffect; //���� ����Ʈ

    public int EnemyLayer; // �� ������Ʈ ���̾�
    public int ActObjLayer; // Ȱ��ȭ ������Ʈ ���̾�
    public int SkillDamage = 0; //��ų ������
    public float EfArrow = 1; //����Ʈ�� ������ ����
    public GameObject AttackEfPreRed; //������ ���� ����Ʈ
    public GameObject AttackEfPreBlue; //�Ķ��� ���� ����Ʈ
    public GameObject AttackEfPreGreen; //�ʷϻ� ���� ����Ʈ
    private GameObject AttackEf;

    // Start is called before the first frame update
    void Start()
    {
        //���� ����Ʈ ����
        if (NColor == Color.red)
            NEffect = AttackEfPreRed;
        else if (NColor == Color.blue)
            NEffect = AttackEfPreBlue;
        else if (NColor == Color.green)
            NEffect = AttackEfPreGreen;

        if(NEffect != null)
        {
            AttackEf = Instantiate(NEffect);
            AttackEf.transform.position = new Vector2(this.transform.position.x + (1f * EfArrow), this.transform.position.y);
        }
       // AttackEf.transform.SetParent(this.transform);
    }

    void OnDestroy()
    {
        //if(AttackEf != null)
           // AttackEf.transform.SetParent(null);
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
                Boss.HurtBoss(SkillDamage, Color.black);
            }
            else
            {
                EnemyController Enemy = other.GetComponent<EnemyController>(); // �� ��Ʈ�ѷ�
                Enemy.HurtEnemy(SkillDamage, 0, null); //���ǰ� �Լ� ȣ��
            }
        }
    }
}
