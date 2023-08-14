using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D RBody;
    Color NColor = Color.red; //������

    private int SkillDamage = 0; //��ų ������
    private float ShootingSpeed = 10f; // ����ü �ӵ�
    private float KnockBackPower = 5f; // ����ü �˹� �Ŀ�
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
        //����ü �÷��� ���
        if(other.gameObject.layer != LayerMask.NameToLayer("Platform"))
            Destroy(this.gameObject);
        if (other.gameObject.tag == "Player")
        {
            Transform thisE = gameObject.GetComponent<Transform>();//Enemy�� ��ġ��
            PlayerController.instance.Hurt(SkillDamage, KnockBackPower, thisE);//�÷��̾� �ǰ� �Լ� ȣ��
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

    //����ü �˹� �Ŀ��� ��Ʈ���ϴ� �Լ�
    public float UdateKnockBackPower
    {
        get
        {
            return KnockBackPower;
        }
        set
        {
            KnockBackPower = value;
        }
    }
}
