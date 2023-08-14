using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkill : MonoBehaviour
{
    Rigidbody2D RBody;
    Color NColor = Color.red; //������

    public int EnemyLayer; // �� ������Ʈ ���̾�
    public int ActObjLayer; // Ȱ��ȭ ������Ʈ ���̾�
    private int SkillDamage = 0; //��ų ������
    private float ShootingSpeed = 10f; // ����ü �ӵ�
    public float SpawnArrow = 1; //������Ʈ ������ ��ġ(����ü ����, ����Ʈ ������ġ ����)
    public GameObject AttackEfPre; //���� ����Ʈ ������
    private GameObject AttackEf; // ������ ���� ����Ʈ ������



    // Start is called before the first frame update
    void Start()
    {
        //���� ����Ʈ ����
        //AttackEf = Instantiate(AttackEfPre);
        //AttackEf.transform.position = new Vector2(this.transform.position.x + (0.5f * EfArrow), this.transform.position.y);

        RBody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RBody.velocity = new Vector2(ShootingSpeed * SpawnArrow, RBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ladder"))
            Destroy(this.gameObject);
        if (ActObjLayer == other.gameObject.layer)
        {
            other.gameObject.SendMessage("ActAction", NColor); // Ȱ��ȭ ������Ʈ Ȱ��ȭ �Լ� ȣ��

        }
        else if (EnemyLayer == other.gameObject.layer)
        {
            EnemyController Enemy = other.GetComponent<EnemyController>(); // �� ��Ʈ�ѷ�
            Enemy.HurtEnemy(SkillDamage, 0, null); //���ǰ� �Լ� ȣ��

            other.GetComponent<EnemyManager>().GSNowReactionColor = NColor; //�� ������ ����
        }
    }



    //������ ���� ��Ʈ�� �ϴ� �Լ�
    public int UdateDamege
    {
        get
        {
            return SkillDamage;
        }
        set
        {
            SkillDamage = value;
        }
    }

    //����ü �ӵ��� ��Ʈ���ϴ� �Լ�
    public float UdateShootingSpeed
    {
        get
        {
            return ShootingSpeed;
        }
        set
        {
            ShootingSpeed = value;
        }
    }
}