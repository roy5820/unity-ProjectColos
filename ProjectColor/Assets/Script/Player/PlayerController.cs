
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�ʱ� ���� ����
    Rigidbody2D RBody;
    float RBodyGravity;

    //�÷��̾� ��������Ʈ
    public GameObject PlayerSfrite;

    //�÷��� �̵����� �浹�� �÷��� ����
    GameObject OtherPlatform = null;

    //�浹 üũ ���� ���� ����
    private Sensor GroundSensor; // ��üũ
    private Sensor PlatformSensor;
    private Sensor LadderSensor;

    //�÷��̾� ���� ���̾� ����
    public int LadderLayer; //��ٸ� ���̾�
    public int InLadderPlayerLayer; //��ٸ� Ÿ�� ����
    public int DropPlayerLayer; //�ؿ� ���� ���� - �� ������Ʈ �浹 X
    public int InvcPlayerLayer; // ���� ���� - �ֳʹ� ������Ʈ �浹 X
    public int PlayerLayer; // �Ϲ����� �÷��̾� ����

    //�÷��̾� ������ ����
    bool OnGround = false; // ��üũ
    bool UpperBodyInLadder = false; //ĳ���� ��ü�� ��ٸ��� ��Ҵ��� üũ
    bool OnPlatform = false; // �÷��� üũ

    //Run ���� ���� ����
    float inputRun = 0.0f;
    bool isRun = false; //Run �Է� ó��
    bool isAniRun = false; //Run �ִϸ��̼� ó��
    public float runSpeed = 3.0f;

    //Jump ���� ���� ����
    bool inputJump = false;
    bool isJump = false;
    bool isAniJump = false;
    public float jumpPower = 12.0f;
    int extraJumpCnt = 0; //���� ���� Ƚ��
    public int extraJumpMaxCnt = 2; //�ִ� ���� Ƚ��
    bool isAniFlight = false;

    //dash ���� ���� ����
    bool inputDash = false;
    bool isDash = false;
    bool isAniDash = false;
    public float DashTime = 0.8f;
    public float DashPower = 16.0f;
    public int coatDash = 30; // ������ ���¹̳� �Ҹ�

    //PlayerClimb ���� ����
    float inputClimb = 0.0f;
    bool isClimb = false;
    public float ClimbSpeed = 5;
    bool collisionLadder = false;
    float LadderX = 0.0f;

    //PlayerDownJump ���� ����
    bool isDownJump = false;
    public float DropTime = 0.5f;

    //Player Hurt ���� ���� ����
    private bool isHurt = false;
    bool isHurtInvc = false;
    public float HurtInvcTime = 1f;

    Color halfColor = new Color(1, 1, 1, 0.5f);
    Color fullColor = new Color(1, 1, 1, 1f);

    //���� ����
    public Transform AttackPoint; // ���� ����Ʈ
    public float AttackPower = 10; //�÷��̾� ���ݷ�
    bool isAttack = false; //���� ����
    bool isSuperArmor = false; // ���ݽ� ���۾Ƹ� ��������

    //nomalAttack ���� ���� ����
    public GameObject NomalAttackPre;
    bool inputNAttack = false;
    bool isAniNAttack = false;
    public float NAttackTime = 0.4f; // ���� �ð�
    public float NAttackDelay = 0.15f; // ���� ����
    public float NAttackDashPower = 7; // �̵� ���� �� �������� �̵��� �Ÿ�
    public int NAttackStaminaCost = 10; //NAttack ���� �� �Ҹ� ���¹̳�
    public float NAttackCoefficient = 1.0f; //Nattack ���

    //ColorSkill ���� ���� ����
    bool inputSkill = false;
    bool isSkillAni = false;

    //RedSkill ���� ���� ����
    public GameObject RedSkillPre;
    public int RedSkillStaminaCoat = 20;
    bool isRedSkillAni = false;
    public float RedSkilTime = 0.3f;
    public float RedSkillDelay = 0.2f;
    public float RedSkillCoefficient = 0.3f;
    public float RedSkillShotingSpeed = 10f;

    //BlueSkill ���� ���� ����
    public GameObject BlueSkillPre;
    public int BlueSkillStaminaCoat = 60;
    public float BlueSkillDashPower = 2;
    public float BlueSkillJumpPower = 25;
    public float BlueSkilTime = 0.5f;
    public float BlueSkillDelay = 0.2f;
    public float BlueSkillCoefficient = 1.2f;
    bool isBlueSkillAni = false;

    //GreenSkill ���� ���� ����
    public GameObject GreenSkillPre;
    public int GreenSkillStaminaCoat = 45;
    public float GreenSkillKnockBackPower = 7f;
    public float GreenSkilTime = 0.7f;
    public float GreenSkillDelay = 0.4f;
    public float GreenSkillCoefficient = 1.5f;
    bool isGreenSkillAni = false;

    //���� ���������� ���� ����
    public float PurpleReactionDamage = 1.5f;
    public float YellowReactionDamage = 1.5f;
    public float SkyblueReactionDamage = 2.0f;

    //�ִϸ��̼� ���� ����
    Animator PlayerAnimation;
    string nowAni = "None";

    // Start is called before the first frame update
    void Start()
    {
        //�ٴ�üũ ����
        GroundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        PlatformSensor = transform.Find("PlatformSensor").GetComponent<Sensor>();
        LadderSensor = transform.Find("LadderSensor").GetComponent<Sensor>();

        //���� �ʱ�ȭ
        RBody = this.GetComponent<Rigidbody2D>();
        RBodyGravity = RBody.gravityScale;
        PlayerAnimation = transform.Find("PlayerSfrite").GetComponent<Animator>();
    }

    private void Awake()
    {
       DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //������ ���� ������Ʈ
        OnPlatform = PlatformSensor.colSencorState();
        OnGround = GroundSensor.colSencorState();
        

        //�Է�ó��
        inputRun = Input.GetAxisRaw("Horizontal");
        inputJump = Input.GetButtonDown("Jump");
        inputDash = Input.GetButtonDown("Fire3");
        inputNAttack = Input.GetKeyDown(KeyCode.Z);
        inputSkill = Input.GetKeyDown(KeyCode.X);
        inputClimb = Input.GetAxisRaw("Vertical");

        //�÷��̾� �ൿ �ϰ� ó��
        // �޸���
        if (!isDash && inputRun != 0 && !isAttack && !isClimb && !isHurt) // �޸��� �Ұ� ���� üũ
            isRun = true;


        if (inputDash && !isDash && (OnGround || OnPlatform) && GameManager.instance.NowStamina >= coatDash && !isAttack)
        {
            PlayerDash();
        }
        else if(inputSkill && !isAttack && !isDash && !isHurt)
        {
            
            Color isColor = GameManager.instance.NColor;
            int isStamina = GameManager.instance.NowStamina;

            if (isColor == Color.blue && isStamina >= BlueSkillStaminaCoat)
            {
                //���׹̳� �Ҹ�
                GameManager.instance.NowStamina -= BlueSkillStaminaCoat;
                ColorSkill(BlueSkillPre, isColor, BlueSkilTime, BlueSkillDelay);
            }
            else if(isColor == Color.green && (OnGround || OnPlatform) && isStamina >= GreenSkillStaminaCoat)
            {
                //���׹̳� �Ҹ�
                GameManager.instance.NowStamina -= GreenSkillStaminaCoat;
                ColorSkill(GreenSkillPre, isColor, GreenSkilTime, GreenSkillDelay);
                SFXManager.Instance.PlaySound(SFXManager.Instance.playerGreenSkill); // ���� ���
            }
            else if(isColor == Color.red && isStamina >= RedSkillStaminaCoat)
            {
                //���׹̳� �Ҹ�
                GameManager.instance.NowStamina -= RedSkillStaminaCoat;
                ColorSkill(RedSkillPre, isColor, RedSkilTime, RedSkillDelay);
            }
            
        }
        else if (inputNAttack && !isAttack && !isDash && !isHurt && GameManager.instance.NowStamina >= NAttackStaminaCost && !isHurt)
        {
            PlayerNomalAttack();
            SFXManager.Instance.PlaySound(SFXManager.Instance.playerNomalAttack);//���� ���
        }
        else if (inputJump && inputClimb < 0 && !isHurt && OnPlatform)
        {
            StartCoroutine(PlayerDownJump());
        }
        else if (extraJumpCnt < extraJumpMaxCnt && inputJump && !isDash && !isHurt)
            PlayerJump();
        else if (inputClimb != 0 && collisionLadder && !isHurt)
            PlayerClimb();



        //���� ����

        //�������� ���°���
        if (OnGround || OnPlatform || isClimb || isAttack)
        {
            extraJumpCnt = 0;
            isAniFlight = false;//ü������ ����
        }
        else
        {
            isAniFlight = true;//ü���ִ� ���� ����
        }


        //Platform �̵����� ���°���
        if (!isClimb && OnPlatform)
        {
            if (OtherPlatform != null) transform.SetParent(OtherPlatform.transform);
        }  
         else
            transform.SetParent(null);

        //���� �̲��� ���� �ڵ�
        if((OnGround || OnPlatform) && !isClimb && !isDash  && !isAniRun && !isJump && !isHurt && !isAttack)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if(!isAttack)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //�ִϸ��̼� �ϰ� ó��
        if (isAniDash)
        {
            nowAni = "Rolling";
        }
        else if (isRedSkillAni)
        {
            nowAni = "RedSkill";
        }
        else if (isGreenSkillAni)
        {
            nowAni = "GreenSkill";
        }
        else if (isBlueSkillAni)
        {
            nowAni = "BlueSkill";
        }
        else if (isAniNAttack)
        {
            nowAni = "NomalAttack";
        }
        else if (isAniJump)
        {
            nowAni = "Jump";
        }
        else if (isAniFlight)
        {
            nowAni = "Flight";
        }
        else if (isAniRun)
        {
            nowAni = "Run";
        }
        else
        {
            nowAni = "None";
        }

        //������ �ִ� ó��
        if (nowAni == "Rolling")
        {
            PlayerAnimation.SetBool("goRolling", true);
        }
        else
        {
            PlayerAnimation.SetBool("goRolling", false);
        }
        //������ ��ų �ִ� ó��
        if (nowAni == "RedSkill")
        {
            PlayerAnimation.SetBool("goRSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goRSkill", false);
        }
        //�ʷϻ� ��ų �ִ� ó��
        if (nowAni == "GreenSkill")
        {
            PlayerAnimation.SetBool("goGSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goGSkill", false);
        }
        //�Ķ��� ��ų �ִ� ó��
        if (nowAni == "BlueSkill")
        {
            PlayerAnimation.SetBool("goBSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goBSkill", false);
        }
        //�븻���� �ִ� ó��
        if (nowAni == "NomalAttack")
        {
            PlayerAnimation.SetBool("goNAttack", true);
        }
        else
        {
            PlayerAnimation.SetBool("goNAttack", false);
        }

        //���� �ִ� ó��
        if (nowAni == "Jump")
        {
            PlayerAnimation.SetTrigger("goJump");
            isAniJump = false;
        }

        //ü���� �ִ� ó��
        if(nowAni == "Flight")
        {
            PlayerAnimation.SetBool("goFlight", true);
        }
        else
        {
            PlayerAnimation.SetBool("goFlight", false);
        }

        //�޸��� �ִ� ó��
        if (nowAni == "Run")
        {
            PlayerAnimation.SetBool("goRun", true);
        }
        else
        {
            PlayerAnimation.SetBool("goRun", false);
        }
    }

    private void FixedUpdate()
    {
        //�޸��� ���� �Լ� ȣ��
        PlayerRun();
    }

    //�޸��� ���� �Լ�
    public void PlayerRun()
    {
        //�޸��� �������� üũ
        if (isRun && (OnGround || OnPlatform))
        {
            isAniRun = true;
        }
        else isAniRun = false;

        if ((inputRun != 0 || (OnGround || OnPlatform)) && !isAttack && !isClimb && !isHurt && !isDash)
        {
            Vector2 runV = new Vector2(inputRun * runSpeed, RBody.velocity.y);
            RBody.velocity = runV;
        }

        //�̵� ������ ���� �̹��� ����
        if (inputRun < 0.0f && isRun && !isDash && !isAttack)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (inputRun > 0.0f && isRun && !isDash && !isAttack)
        {
            transform.localScale = new Vector2(1, 1);
        }

        isRun = false;
    }

    //���� ���� �Լ�
    public void PlayerJump()
    {
        isJump = true;
        isAniJump = true;
        isClimb = false;


        if ((OnGround || OnPlatform) || extraJumpCnt < extraJumpMaxCnt) { 
            extraJumpCnt++;

            RBody.velocity = new Vector2(RBody.velocity.x, 0f);
            RBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        isJump = false;
    }

    //�뽬 ���� �Լ�
    public void PlayerDash()
    {
        isDash = true;
        GameManager.instance.PlayerStamina = coatDash;
        gameObject.layer = InvcPlayerLayer; // ���� ���̾�� ����
        float moveArrow = transform.localScale.x;


        RBody.velocity = new Vector2(0f, RBody.velocity.y);
        RBody.AddForce(Vector2.right * moveArrow * DashPower, ForceMode2D.Impulse);

        isAniDash = true;

        StartCoroutine(DashTimer());
    }
    //�뽬 Ÿ�̸�
    IEnumerator DashTimer()
    {
        float isTime = 0f;
        float EalryPosY = this.GetComponent<Transform>().position.y;

        while (isTime < DashTime/1.5f)
        {
            isTime += Time.deltaTime;
            RBody.velocity = new Vector2(RBody.velocity.x, RBodyGravity);
        }

        yield return new WaitForSeconds(DashTime);

        isDash = false;
        isAniDash = false;
        gameObject.layer = PlayerLayer;
    }

    //�Ϲݰ��� ���� �Լ�
    public void PlayerNomalAttack()
    {
        if (isHurt) return;
        GameManager.instance.PlayerStamina = NAttackStaminaCost;

        isAniNAttack = true; // ���� �ִ� Ȱ��ȭ
        isAttack = true;//���� ���� Ȱ��ȭ

        //���� ���� �ڷ�ƾ ȣ��
        StartCoroutine(ProduceAttack());
    }
    //���� ���� �ڷ�ƾ
    IEnumerator ProduceAttack()
    {
        GameObject NomalAttack = null;
        float isTime = 0f;

        //�������� ������ �̵��ϸ鼭 ���ݽ� ���ݽ� ���� ����
        if ((OnGround || OnPlatform) && inputRun != 0)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            RBody.velocity = new Vector2(0, RBody.velocity.y); // ���� �� ����
            float moveArrow = transform.localScale.x;
            
            RBody.AddForce(Vector2.right * moveArrow * NAttackDashPower, ForceMode2D.Impulse);
        }

        while (isTime <= NAttackTime)
        {
            isTime += Time.deltaTime;
            //�ǰݽ� ���� ĵ��

            if (isHurt)
            {
                isTime = NAttackTime;
                break;
            }
            
            //���� �����̰� ������ ���� ����
            if (isTime >= NAttackDelay && NomalAttack == null)
            {
                if (OnGround || OnPlatform)
                {
                    RBody.velocity = new Vector2(0, RBody.velocity.y); // ���� �� ����
                    RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;//�̲��� ���� ����
                }
                
                NomalAttack = Instantiate(NomalAttackPre); // �Ϲݰ��� �ø��� ����
                NomalAttack.GetComponent<Transform>().position = AttackPoint.position; // ������ ��ġ ����
                NomalAttack.GetComponent<Transform>().localScale = new Vector2(1 * this.transform.localScale.x, 1);//ĳ���� �ٶ󺸴� �������� �������� ����
                NomalAttack.transform.SetParent(this.transform); // �ش� �������� �÷��̾��� �ڽ����� ����
                NomalAttack.GetComponent<NomalAttack>().SkillDamage = (int)(Mathf.Round(AttackPower * NAttackCoefficient)); // �Ϲݰ��� �ø����� ���� ������ �� ����
                NomalAttack.GetComponent<NomalAttack>().EfArrow = this.transform.localScale.x; // ĳ���Ͱ� �ٷκ��� �������� ����Ʈ ������ ���� ������ ����
                NomalAttack.GetComponent<NomalAttack>().NColor = GameManager.instance.NColor;
            }
            yield return null;
        }
        
        //���� ���� �� ������ ���°� false
        if (NomalAttack != null)
            Destroy(NomalAttack);
        isAttack = false;
        isAniNAttack = false;
    }

    //ĳ���� ��ų ����
    public void ColorSkill(GameObject SkillObj, Color SkillColor, float SkillTime, float SkillDelay)
    {
        if (isHurt && !isSuperArmor) return; // ���۾Ƹ� ���°� �ƴ� �� �ǰ� ���� �� ��ų �Ⱦ�

        isAttack = true; // ���ݻ��� Ȱ��ȭ

        RBody.velocity = new Vector2(0, RBody.velocity.y); // ���� �� ����
        //���� ��ų
        if (SkillColor == Color.red)
            isRedSkillAni = true;
        else if (SkillColor == Color.green)
            isGreenSkillAni = true;
        else if (SkillColor == Color.blue)
            isBlueSkillAni = true;

        StartCoroutine(ProduceSkill(SkillObj, SkillColor, SkillTime, SkillDelay));//��ų���� �ڷ�ƾ ȣ��
    }
    //��ų���� �ڷ�ƾ
    IEnumerator ProduceSkill(GameObject SkillObj, Color SkillColor, float SkillTime, float SkillDelay)
    {
        GameObject ColorSkill = null;
        bool CreateObj = false; // ��ų ���� ����
        float isTime = 0f;

        while (isTime <= SkillTime)
        {
            isTime += Time.deltaTime;
            //�ǰݽ� ���� ĵ��
            if (isHurt && !isSuperArmor)
            {
                isTime = NAttackTime;
                break;
            }

            //���� �����̰� ������ ���� ����
            if (isTime >= SkillDelay && !CreateObj)
            {
                CreateObj = true; //��ų ���� 0
                ColorSkill = Instantiate(SkillObj);
                ColorSkill.GetComponent<Transform>().position = AttackPoint.position; // ������ ��ġ ����
                ColorSkill.GetComponent<Transform>().localScale = new Vector2(1 * this.transform.localScale.x, 1);//ĳ���� �ٶ󺸴� �������� �������� ����
                
                if (SkillColor == Color.red)
                {
                    //������ ����
                    ColorSkill.GetComponent<RedSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * RedSkillCoefficient));
                    //����ü ���� �� ����
                    ColorSkill.GetComponent<RedSkill>().UdateShootingSpeed = RedSkillShotingSpeed;
                    ColorSkill.GetComponent<RedSkill>().SpawnArrow = this.transform.localScale.x;
                }
                else if(SkillColor == Color.green)
                {
                    //��ų �������� �÷��̾��� �ڽ����� ����
                    ColorSkill.transform.SetParent(this.transform); // �ش� �������� �÷��̾��� �ڽ����� ����

                    //������ ����
                    ColorSkill.GetComponent<GreenSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * GreenSkillCoefficient));
                    //�˹� �Ŀ� ����
                    ColorSkill.GetComponent<GreenSkill>().UdateKnockBackPower = GreenSkillKnockBackPower;
                }
                else if (SkillColor == Color.blue)
                {
                    //�̲��� ���� ���� ���� ����
                    RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    //��ų �������� �÷��̾��� �ڽ����� ����
                    ColorSkill.transform.SetParent(this.transform); // �ش� �������� �÷��̾��� �ڽ����� ����
                    //���� �ʱ�ȭ
                    RBody.velocity = new Vector2(RBody.velocity.x, 0);
                    //������ ����
                    ColorSkill.GetComponent<BlueSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * BlueSkillCoefficient));
                    //���� �׼� ����
                    Vector2 AttackV = new Vector2(BlueSkillDashPower * this.GetComponent<Transform>().localScale.x, BlueSkillJumpPower);
                    RBody.AddForce(AttackV, ForceMode2D.Impulse);
                }
            }
            yield return null;
        }

        //���� ���� �� ������ ���°� false
        if (CreateObj && SkillColor != Color.red)
            Destroy(ColorSkill);
        isAttack = false;

        //���� ��ų �ִϸ��̼� ���°��� - ��Ȱ��ȭ
        if (SkillColor == Color.red)
            isRedSkillAni = false;
        else if (SkillColor == Color.green)
            isGreenSkillAni = false;
        else if (SkillColor == Color.blue) {
            isBlueSkillAni = false;
            RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //��ٸ�Ÿ�� ���� �Լ�
    public void PlayerClimb()
    {
        isClimb = true;
        UpperBodyInLadder = LadderSensor.colSencorState(); // ĳ���� ��ü ��ٸ� �浹 üũ ���� �ʱ�ȭ

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.layer = InLadderPlayerLayer;

        StartCoroutine(Climbing());
    }
    //��ٸ� ������ ���� ����
    IEnumerator Climbing()
    {
        while (isClimb == true)
        {
            if (collisionLadder == false) // ��ٸ� ������ ����� climb���� ����
                isClimb = false;
            else if (inputClimb < 0 && UpperBodyInLadder && (OnGround || OnPlatform)) // �������� �ٴ� ������ ���� ����
                isClimb = false;
            else if (inputClimb > 0 && !UpperBodyInLadder) // ��ü�� ��ٸ� ���ε� �ö󰥷��� �ϸ� climb���� ����
                isClimb = false;
            yield return null;

            if (isClimb)
            {
                Vector2 ClimbV = new Vector2(0, inputClimb * ClimbSpeed);
                gameObject.GetComponent<Transform>().position = new Vector3(LadderX, gameObject.transform.position.y, gameObject.transform.position.z);
                RBody.velocity = ClimbV;
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = RBodyGravity;
                gameObject.layer = PlayerLayer;
            }
        }
    }

    //PlayerDowJump ���� �ڷ�ƾ �Լ�
    IEnumerator PlayerDownJump()
    {
        isDownJump = true;
        this.gameObject.layer = DropPlayerLayer;
        Vector2 DownJumpV = new Vector2(RBody.velocity.x, -5);
        RBody.AddForce(DownJumpV, ForceMode2D.Impulse);

        float isTime = 0f;
        while (isTime <= DropTime)
        {
            isTime += Time.deltaTime;

            yield return null;
        }

        isDownJump = false;
        this.gameObject.layer = PlayerLayer;
    }

    //�÷��̾� �ǰ� �����Լ�
    public void Hurt(int damage, float KbPower, Transform ohterT)
    {
        isHurt = true;
        isHurtInvc = true;

        gameObject.layer = InvcPlayerLayer;// ���� ���̾�� ����

        GameManager.instance.PlayerHp = damage;

        int hp = GameManager.instance.PlayerHp;

        if (hp < 0)
        {

        }
        else
        {
            float x = transform.position.x - ohterT.position.x;
            float y = transform.position.y - ohterT.position.y;

            x = x < 0 ? -1 : 1;
            y = y < 0 ? -1 : 1;

            RBody.velocity = new Vector2(0, 0);

            if (!isSuperArmor){
                Vector2 KnockBackV = new Vector2(x, y);
                
                StartCoroutine(KnockBackTimer(KnockBackV, KbPower));
            }
            StartCoroutine(HurtInvcExit());
            StartCoroutine(Alphablink());
        }
    }
    //��Ʈ �� �˹� ����
    IEnumerator KnockBackTimer(Vector2 KnockBackV, float KbPower)
    {
        float isTime = 0f;
        float AddForceCnt = 0;
        while (isTime < 0.3f)
        {
            //���� �� ���� ������ �ٷ� ������ ���δ� ���¿��� AddForce�� �˹��� �ϸ� �˹��� ���̴� ���� �߻�(������ �Ⱦ���) -> �׷��� ī��Ʈ�� ���� 2���� ���� �˹� �����ϰ� ��
            if (AddForceCnt == 2)
                RBody.AddForce(KnockBackV * KbPower, ForceMode2D.Impulse);
            AddForceCnt++;
            isTime += Time.deltaTime;
            yield return null;
        }

        isHurt = false;
    }

    //���� �ð� ����� �÷��̾� ���̾�� ��ü
    IEnumerator HurtInvcExit()
    {
        yield return new WaitForSeconds(HurtInvcTime);
        if(!isDash)//�뽬 ���� ��� ���� ���� X
            gameObject.layer = PlayerLayer;
        isHurtInvc = false;
    }

    //�ǰ� �� �ִϸ��̼� (���� �ӽ÷� ���� ������)
    IEnumerator Alphablink()
    {
        while (isHurtInvc)
        {
            yield return new WaitForSeconds(0.1f);
            PlayerSfrite.transform.GetComponent<SpriteRenderer>().color = halfColor;
            yield return new WaitForSeconds(0.1f);
            PlayerSfrite.transform.GetComponent<SpriteRenderer>().color = fullColor;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (LadderLayer == other.gameObject.layer)
        {
            collisionLadder = true;

            LadderX = other.gameObject.GetComponent<Transform>().position.x;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (LadderLayer == other.gameObject.layer)
        {
            collisionLadder = false;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        //�÷��� �̵����� �÷��� �浹üũ
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform") && PlatformSensor.colSencorState())
        {
            OtherPlatform = other.gameObject;
        }
    }
}