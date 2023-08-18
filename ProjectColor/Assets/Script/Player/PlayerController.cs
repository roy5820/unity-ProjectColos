
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //초기 변수 선언
    Rigidbody2D RBody;
    float RBodyGravity;

    //플레이어 스프라이트
    public GameObject PlayerSfrite;

    //플렛폼 이동관련 충돌된 플렛폼 저장
    GameObject OtherPlatform = null;

    //충돌 체크 관련 변수 선언
    private Sensor GroundSensor; // 땅체크
    private Sensor PlatformSensor;
    private Sensor LadderSensor;

    //플레이어 상태 레이어 변수
    public int LadderLayer; //사다리 레이어
    public int InLadderPlayerLayer; //사다리 타기 상태
    public int DropPlayerLayer; //밑에 점프 상태 - 블럭 오브젝트 충돌 X
    public int InvcPlayerLayer; // 무적 상태 - 애너미 오브젝트 충돌 X
    public int PlayerLayer; // 일반적인 플레이어 상태

    //플레이어 센서별 상태
    bool OnGround = false; // 땅체크
    bool UpperBodyInLadder = false; //캐릭터 상체가 사다리에 닿았는지 체크
    bool OnPlatform = false; // 플렛폼 체크

    //Run 관련 변수 선언
    float inputRun = 0.0f;
    bool isRun = false; //Run 입력 처리
    bool isAniRun = false; //Run 애니메이션 처리
    public float runSpeed = 3.0f;

    //Jump 관련 변수 선언
    bool inputJump = false;
    bool isJump = false;
    bool isAniJump = false;
    public float jumpPower = 12.0f;
    int extraJumpCnt = 0; //현재 점프 횟수
    public int extraJumpMaxCnt = 2; //최대 점프 횟수
    bool isAniFlight = false;

    //dash 관련 변수 선언
    bool inputDash = false;
    bool isDash = false;
    bool isAniDash = false;
    public float DashTime = 0.8f;
    public float DashPower = 16.0f;
    public int coatDash = 30; // 구르기 스태미나 소모량

    //PlayerClimb 관련 변수
    float inputClimb = 0.0f;
    bool isClimb = false;
    public float ClimbSpeed = 5;
    bool collisionLadder = false;
    float LadderX = 0.0f;

    //PlayerDownJump 관련 변수
    bool isDownJump = false;
    public float DropTime = 0.5f;

    //Player Hurt 관련 변수 선언
    private bool isHurt = false;
    bool isHurtInvc = false;
    public float HurtInvcTime = 1f;

    Color halfColor = new Color(1, 1, 1, 0.5f);
    Color fullColor = new Color(1, 1, 1, 1f);

    //공격 관런
    public Transform AttackPoint; // 공격 포인트
    public float AttackPower = 10; //플레이어 공격력
    bool isAttack = false; //공격 상태
    bool isSuperArmor = false; // 공격시 슈퍼아머 상태인지

    //nomalAttack 관련 변수 선언
    public GameObject NomalAttackPre;
    bool inputNAttack = false;
    bool isAniNAttack = false;
    public float NAttackTime = 0.4f; // 공격 시간
    public float NAttackDelay = 0.15f; // 공격 선딜
    public float NAttackDashPower = 7; // 이동 공격 시 선딜동안 이동할 거리
    public int NAttackStaminaCost = 10; //NAttack 시전 시 소모 스태미나
    public float NAttackCoefficient = 1.0f; //Nattack 계수

    //ColorSkill 관련 변수 선언
    bool inputSkill = false;
    bool isSkillAni = false;

    //RedSkill 관련 변수 선언
    public GameObject RedSkillPre;
    public int RedSkillStaminaCoat = 20;
    bool isRedSkillAni = false;
    public float RedSkilTime = 0.3f;
    public float RedSkillDelay = 0.2f;
    public float RedSkillCoefficient = 0.3f;
    public float RedSkillShotingSpeed = 10f;

    //BlueSkill 관련 변수 선언
    public GameObject BlueSkillPre;
    public int BlueSkillStaminaCoat = 60;
    public float BlueSkillDashPower = 2;
    public float BlueSkillJumpPower = 25;
    public float BlueSkilTime = 0.5f;
    public float BlueSkillDelay = 0.2f;
    public float BlueSkillCoefficient = 1.2f;
    bool isBlueSkillAni = false;

    //GreenSkill 관련 변수 선언
    public GameObject GreenSkillPre;
    public int GreenSkillStaminaCoat = 45;
    public float GreenSkillKnockBackPower = 7f;
    public float GreenSkilTime = 0.7f;
    public float GreenSkillDelay = 0.4f;
    public float GreenSkillCoefficient = 1.5f;
    bool isGreenSkillAni = false;

    //색깔 반응데미지 관련 변수
    public float PurpleReactionDamage = 1.5f;
    public float YellowReactionDamage = 1.5f;
    public float SkyblueReactionDamage = 2.0f;

    //애니메이션 변수 선언
    Animator PlayerAnimation;
    string nowAni = "None";

    // Start is called before the first frame update
    void Start()
    {
        //바닥체크 센서
        GroundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        PlatformSensor = transform.Find("PlatformSensor").GetComponent<Sensor>();
        LadderSensor = transform.Find("LadderSensor").GetComponent<Sensor>();

        //변수 초기화
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
        //센서별 상태 업데이트
        OnPlatform = PlatformSensor.colSencorState();
        OnGround = GroundSensor.colSencorState();
        

        //입력처리
        inputRun = Input.GetAxisRaw("Horizontal");
        inputJump = Input.GetButtonDown("Jump");
        inputDash = Input.GetButtonDown("Fire3");
        inputNAttack = Input.GetKeyDown(KeyCode.Z);
        inputSkill = Input.GetKeyDown(KeyCode.X);
        inputClimb = Input.GetAxisRaw("Vertical");

        //플레이어 행동 일괄 처리
        // 달리기
        if (!isDash && inputRun != 0 && !isAttack && !isClimb && !isHurt) // 달리기 불가 상태 체크
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
                //스테미나 소모
                GameManager.instance.NowStamina -= BlueSkillStaminaCoat;
                ColorSkill(BlueSkillPre, isColor, BlueSkilTime, BlueSkillDelay);
            }
            else if(isColor == Color.green && (OnGround || OnPlatform) && isStamina >= GreenSkillStaminaCoat)
            {
                //스테미나 소모
                GameManager.instance.NowStamina -= GreenSkillStaminaCoat;
                ColorSkill(GreenSkillPre, isColor, GreenSkilTime, GreenSkillDelay);
                SFXManager.Instance.PlaySound(SFXManager.Instance.playerGreenSkill); // 사운드 재생
            }
            else if(isColor == Color.red && isStamina >= RedSkillStaminaCoat)
            {
                //스테미나 소모
                GameManager.instance.NowStamina -= RedSkillStaminaCoat;
                ColorSkill(RedSkillPre, isColor, RedSkilTime, RedSkillDelay);
            }
            
        }
        else if (inputNAttack && !isAttack && !isDash && !isHurt && GameManager.instance.NowStamina >= NAttackStaminaCost && !isHurt)
        {
            PlayerNomalAttack();
            SFXManager.Instance.PlaySound(SFXManager.Instance.playerNomalAttack);//사운드 재생
        }
        else if (inputJump && inputClimb < 0 && !isHurt && OnPlatform)
        {
            StartCoroutine(PlayerDownJump());
        }
        else if (extraJumpCnt < extraJumpMaxCnt && inputJump && !isDash && !isHurt)
            PlayerJump();
        else if (inputClimb != 0 && collisionLadder && !isHurt)
            PlayerClimb();



        //상태 관리

        //점프관련 상태관리
        if (OnGround || OnPlatform || isClimb || isAttack)
        {
            extraJumpCnt = 0;
            isAniFlight = false;//체공상태 관리
        }
        else
        {
            isAniFlight = true;//체공애니 상태 관리
        }


        //Platform 이동관련 상태관리
        if (!isClimb && OnPlatform)
        {
            if (OtherPlatform != null) transform.SetParent(OtherPlatform.transform);
        }  
         else
            transform.SetParent(null);

        //경사면 미끄럼 방지 코드
        if((OnGround || OnPlatform) && !isClimb && !isDash  && !isAniRun && !isJump && !isHurt && !isAttack)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if(!isAttack)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //애니메이션 일괄 처리
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

        //구르기 애니 처리
        if (nowAni == "Rolling")
        {
            PlayerAnimation.SetBool("goRolling", true);
        }
        else
        {
            PlayerAnimation.SetBool("goRolling", false);
        }
        //빨간색 스킬 애니 처리
        if (nowAni == "RedSkill")
        {
            PlayerAnimation.SetBool("goRSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goRSkill", false);
        }
        //초록색 스킬 애니 처리
        if (nowAni == "GreenSkill")
        {
            PlayerAnimation.SetBool("goGSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goGSkill", false);
        }
        //파란색 스킬 애니 처리
        if (nowAni == "BlueSkill")
        {
            PlayerAnimation.SetBool("goBSkill", true);
        }
        else
        {
            PlayerAnimation.SetBool("goBSkill", false);
        }
        //노말공격 애니 처리
        if (nowAni == "NomalAttack")
        {
            PlayerAnimation.SetBool("goNAttack", true);
        }
        else
        {
            PlayerAnimation.SetBool("goNAttack", false);
        }

        //점프 애니 처리
        if (nowAni == "Jump")
        {
            PlayerAnimation.SetTrigger("goJump");
            isAniJump = false;
        }

        //체공시 애니 처리
        if(nowAni == "Flight")
        {
            PlayerAnimation.SetBool("goFlight", true);
        }
        else
        {
            PlayerAnimation.SetBool("goFlight", false);
        }

        //달리기 애니 처리
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
        //달리기 구현 함수 호출
        PlayerRun();
    }

    //달리기 구현 함수
    public void PlayerRun()
    {
        //달리기 상태인지 체크
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

        //이동 방향의 따른 이미지 반전
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

    //점프 구현 함수
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

    //대쉬 구현 함수
    public void PlayerDash()
    {
        isDash = true;
        GameManager.instance.PlayerStamina = coatDash;
        gameObject.layer = InvcPlayerLayer; // 무적 레이어로 변경
        float moveArrow = transform.localScale.x;


        RBody.velocity = new Vector2(0f, RBody.velocity.y);
        RBody.AddForce(Vector2.right * moveArrow * DashPower, ForceMode2D.Impulse);

        isAniDash = true;

        StartCoroutine(DashTimer());
    }
    //대쉬 타이머
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

    //일반공격 구현 함수
    public void PlayerNomalAttack()
    {
        if (isHurt) return;
        GameManager.instance.PlayerStamina = NAttackStaminaCost;

        isAniNAttack = true; // 공격 애니 활성화
        isAttack = true;//공격 상태 활성화

        //공격 구현 코루틴 호출
        StartCoroutine(ProduceAttack());
    }
    //공격 구현 코루틴
    IEnumerator ProduceAttack()
    {
        GameObject NomalAttack = null;
        float isTime = 0f;

        //땅위에서 앞으로 이동하면서 공격시 공격시 전진 공격
        if ((OnGround || OnPlatform) && inputRun != 0)
        {
            RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            RBody.velocity = new Vector2(0, RBody.velocity.y); // 공격 시 정지
            float moveArrow = transform.localScale.x;
            
            RBody.AddForce(Vector2.right * moveArrow * NAttackDashPower, ForceMode2D.Impulse);
        }

        while (isTime <= NAttackTime)
        {
            isTime += Time.deltaTime;
            //피격시 공격 캔슬

            if (isHurt)
            {
                isTime = NAttackTime;
                break;
            }
            
            //공격 딜레이가 끝난뒤 공격 구현
            if (isTime >= NAttackDelay && NomalAttack == null)
            {
                if (OnGround || OnPlatform)
                {
                    RBody.velocity = new Vector2(0, RBody.velocity.y); // 공격 시 정지
                    RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;//미끄럼 방지 뻘짓
                }
                
                NomalAttack = Instantiate(NomalAttackPre); // 일반공격 플리펩 생성
                NomalAttack.GetComponent<Transform>().position = AttackPoint.position; // 프리펩 위치 설정
                NomalAttack.GetComponent<Transform>().localScale = new Vector2(1 * this.transform.localScale.x, 1);//캐릭터 바라보는 방향으로 설정으로 생성
                NomalAttack.transform.SetParent(this.transform); // 해당 프리펩을 플레이어의 자식으로 설정
                NomalAttack.GetComponent<NomalAttack>().SkillDamage = (int)(Mathf.Round(AttackPower * NAttackCoefficient)); // 일반공격 플리펩의 공격 데미지 값 설정
                NomalAttack.GetComponent<NomalAttack>().EfArrow = this.transform.localScale.x; // 캐릭터가 바로보는 방향으로 이펙트 생성을 위한 정보값 전달
                NomalAttack.GetComponent<NomalAttack>().NColor = GameManager.instance.NColor;
            }
            yield return null;
        }
        
        //공격 종류 시 기존의 상태값 false
        if (NomalAttack != null)
            Destroy(NomalAttack);
        isAttack = false;
        isAniNAttack = false;
    }

    //캐릭터 스킬 구현
    public void ColorSkill(GameObject SkillObj, Color SkillColor, float SkillTime, float SkillDelay)
    {
        if (isHurt && !isSuperArmor) return; // 슈퍼아머 상태가 아닐 시 피격 했을 때 스킬 안씀

        isAttack = true; // 공격상태 활성화

        RBody.velocity = new Vector2(0, RBody.velocity.y); // 공격 시 정지
        //색깔별 스킬
        if (SkillColor == Color.red)
            isRedSkillAni = true;
        else if (SkillColor == Color.green)
            isGreenSkillAni = true;
        else if (SkillColor == Color.blue)
            isBlueSkillAni = true;

        StartCoroutine(ProduceSkill(SkillObj, SkillColor, SkillTime, SkillDelay));//스킬구현 코루틴 호출
    }
    //스킬구현 코루틴
    IEnumerator ProduceSkill(GameObject SkillObj, Color SkillColor, float SkillTime, float SkillDelay)
    {
        GameObject ColorSkill = null;
        bool CreateObj = false; // 스킬 생성 여부
        float isTime = 0f;

        while (isTime <= SkillTime)
        {
            isTime += Time.deltaTime;
            //피격시 공격 캔슬
            if (isHurt && !isSuperArmor)
            {
                isTime = NAttackTime;
                break;
            }

            //공격 딜레이가 끝난뒤 공격 구현
            if (isTime >= SkillDelay && !CreateObj)
            {
                CreateObj = true; //스킬 생성 0
                ColorSkill = Instantiate(SkillObj);
                ColorSkill.GetComponent<Transform>().position = AttackPoint.position; // 프리펩 위치 설정
                ColorSkill.GetComponent<Transform>().localScale = new Vector2(1 * this.transform.localScale.x, 1);//캐릭터 바라보는 방향으로 설정으로 생성
                
                if (SkillColor == Color.red)
                {
                    //데미지 전달
                    ColorSkill.GetComponent<RedSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * RedSkillCoefficient));
                    //투사체 방향 값 전달
                    ColorSkill.GetComponent<RedSkill>().UdateShootingSpeed = RedSkillShotingSpeed;
                    ColorSkill.GetComponent<RedSkill>().SpawnArrow = this.transform.localScale.x;
                }
                else if(SkillColor == Color.green)
                {
                    //스킬 프리펩을 플레이어의 자식으로 설정
                    ColorSkill.transform.SetParent(this.transform); // 해당 프리펩을 플레이어의 자식으로 설정

                    //데미지 전달
                    ColorSkill.GetComponent<GreenSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * GreenSkillCoefficient));
                    //넉백 파워 전달
                    ColorSkill.GetComponent<GreenSkill>().UdateKnockBackPower = GreenSkillKnockBackPower;
                }
                else if (SkillColor == Color.blue)
                {
                    //미끄럼 방지 땜에 생긴 뻘짓
                    RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    //스킬 프리펩을 플레이어의 자식으로 설정
                    ColorSkill.transform.SetParent(this.transform); // 해당 프리펩을 플레이어의 자식으로 설정
                    //백터 초기화
                    RBody.velocity = new Vector2(RBody.velocity.x, 0);
                    //데미지 전달
                    ColorSkill.GetComponent<BlueSkill>().UdateDamege = (int)(Mathf.Round(AttackPower * BlueSkillCoefficient));
                    //점프 액션 구현
                    Vector2 AttackV = new Vector2(BlueSkillDashPower * this.GetComponent<Transform>().localScale.x, BlueSkillJumpPower);
                    RBody.AddForce(AttackV, ForceMode2D.Impulse);
                }
            }
            yield return null;
        }

        //공격 종류 시 기존의 상태값 false
        if (CreateObj && SkillColor != Color.red)
            Destroy(ColorSkill);
        isAttack = false;

        //색깔별 스킬 애니메이션 상태관리 - 비활성화
        if (SkillColor == Color.red)
            isRedSkillAni = false;
        else if (SkillColor == Color.green)
            isGreenSkillAni = false;
        else if (SkillColor == Color.blue) {
            isBlueSkillAni = false;
            RBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //사다리타기 관련 함수
    public void PlayerClimb()
    {
        isClimb = true;
        UpperBodyInLadder = LadderSensor.colSencorState(); // 캐릭터 상체 사다리 충돌 체크 변수 초기화

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.layer = InLadderPlayerLayer;

        StartCoroutine(Climbing());
    }
    //사다리 오르기 상태 관련
    IEnumerator Climbing()
    {
        while (isClimb == true)
        {
            if (collisionLadder == false) // 사다리 밖으로 벗어나면 climb상태 해제
                isClimb = false;
            else if (inputClimb < 0 && UpperBodyInLadder && (OnGround || OnPlatform)) // 내려가다 바닥 다으면 상태 해제
                isClimb = false;
            else if (inputClimb > 0 && !UpperBodyInLadder) // 상체가 사다리 밖인데 올라갈려고 하면 climb상태 해제
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

    //PlayerDowJump 구현 코루틴 함수
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

    //플레이어 피격 구현함수
    public void Hurt(int damage, float KbPower, Transform ohterT)
    {
        isHurt = true;
        isHurtInvc = true;

        gameObject.layer = InvcPlayerLayer;// 무적 레이어로 변경

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
    //히트 시 넉백 구현
    IEnumerator KnockBackTimer(Vector2 KnockBackV, float KbPower)
    {
        float isTime = 0f;
        float AddForceCnt = 0;
        while (isTime < 0.3f)
        {
            //웬지 모를 버그 때문에 바로 가만히 서인는 상태에서 AddForce로 넉백을 하면 넉백이 씹이는 현상 발생(가끔씩 안씹임) -> 그래서 카운트를 세서 2번쨰 부터 넉백 구현하게 함
            if (AddForceCnt == 2)
                RBody.AddForce(KnockBackV * KbPower, ForceMode2D.Impulse);
            AddForceCnt++;
            isTime += Time.deltaTime;
            yield return null;
        }

        isHurt = false;
    }

    //무적 시간 종료시 플레이어 레이어로 교체
    IEnumerator HurtInvcExit()
    {
        yield return new WaitForSeconds(HurtInvcTime);
        if(!isDash)//대쉬 중일 경우 무적 해제 X
            gameObject.layer = PlayerLayer;
        isHurtInvc = false;
    }

    //피격 시 애니메이션 (현재 임시로 색만 조정함)
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
        //플렛폼 이동관련 플렛폼 충돌체크
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform") && PlatformSensor.colSencorState())
        {
            OtherPlatform = other.gameObject;
        }
    }
}