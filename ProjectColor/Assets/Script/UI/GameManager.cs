using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //��Ŭ�� ����ȭ
    public static GameManager instance = null;

    // �� ��ȯ �� üũ�� �̺�Ʈ ����
    public event Action OnSceneChanged;

    // ���� ���� ����
    public Image nowColor;
    public Slider HealthBar;
    public Slider StaminaGage;

    bool inputNcolor;

    int NcolorCode = 3;// �������
    public Color NColor;
    
    //ü�¹�
    public int MaxHealth = 100;
    public int NowHealth;

    // ���¹̳� ����
    public int MaxStamina = 100;
    public int NowStamina;
    float staminaTimer;
    public float recoveryCycle = 0.2f; //���¹̳� ȸ���ֱ�
    public int recoveryAmount = 3; //ȸ����

    //���̵� �� �ƿ� ��ũ��
    public GameObject PadeScreen;
    public float PadeInTime = 1.5f;
    public float PadeOutTime = 1.5f;

    //������ �޴���
    private ItemManager itemManager;
    public GameObject itemPanel;//������ ���� �˾� ���
    bool isSelectItem = false; // ������ ���� ����

    //�÷��̾� ���� ����
    public GameObject PlayerPre;
    private GameObject SpawnPoint;
    GameObject FindPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //������ �޴��� �ʱ�ȭ
        itemManager = this.GetComponent<ItemManager>();

        //���� ü��, ���¹̳� �ִ�ü��, ���¹̳��� ����
        NowHealth = MaxHealth;
        NowStamina = MaxStamina;

        //�� ���� �� ���̵� �ƿ� ����
        StartCoroutine(PadeImageAndChangeScene(PadeOutTime, null));

        //�÷��̾� ����
        FindPlayer = GameObject.FindWithTag("Player");
        SpawnPoint = GameObject.Find("SpawnPoint");
        if (FindPlayer == null)
        {
            FindPlayer = Instantiate(PlayerPre);
            FindPlayer.GetComponent<Transform>().position = SpawnPoint.GetComponent<Transform>().position;
        }
        
    }

    private void Awake()
    {
        // �ٸ� ������ �Ѿ���� �� ��ü�� �����ϱ� ���� �̱��� ������ ���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �ִٸ� �� ��ü �ı�
        }
    }

    // �ٸ� ������ �̵��� �� �̺�Ʈ ȣ��
    public void ChangeScene()
    {
        //�� ���� �� ���̵� �ƿ� ����
        StartCoroutine(PadeImageAndChangeScene(PadeOutTime, null));
        SpawnPoint = GameObject.Find("SpawnPoint");

        //�÷��̾� ����
        if (FindPlayer != null)
        {
            FindPlayer.SetActive(true);
            FindPlayer.GetComponent<Transform>().position = SpawnPoint.GetComponent<Transform>().position;
        }

        // �̺�Ʈ �߻�
        OnSceneChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        inputNcolor = Input.GetKeyDown(KeyCode.C);
        
        if (inputNcolor)
        {
            NcolorCode += 3;

            if (NcolorCode > 10)
            {
                NcolorCode -= 9;
            }
        }

        //Ncolor�� ���� �� ����
        switch (NcolorCode)
        {
            case 3:
                NColor = Color.red;
                nowColor.color = NColor;
                break;
            case 6:
                NColor = Color.green;
                nowColor.color = NColor;
                break;
            case 9:
                NColor = Color.blue;
                nowColor.color = NColor;
                break;
        }

        //���¹̳� ����
        staminaTimer += Time.deltaTime;
        if (staminaTimer > recoveryCycle)
        {
            
            if (NowStamina >= 100) NowStamina = 100;
            else NowStamina += recoveryAmount;
            staminaTimer = 0f;
        }

        StaminaGage.value = NowStamina;
        StaminaGage.maxValue = MaxStamina;

        //ü�¹� ���� PlayerMove���� ü�� ����
        HealthBar.value = NowHealth;
        HealthBar.maxValue = MaxHealth;
    }

    //���¹̳� ����
    public int PlayerStamina
    {
        get
        {
            return NowStamina;
        }
        set
        {
            NowStamina -= value;
        }


    }

    //ü�� �Ӽ�
    public int PlayerHp
    {
        get
        {
            return NowHealth;
        }
        set
        {
            NowHealth -= value;
            if (NowHealth < 0) NowHealth = 0;
        }
    }
    //�� ü���� �Լ�
    public bool ChangScen(string SceneName)
    {
        
        StartCoroutine(PadeImageAndChangeScene(PadeInTime, SceneName));
        return true;
    }


    //�� ��ȯ �� �� ��ȯ �� ���̵� ��, �ƿ��� �ִ� �ڷ�ƾ 
    //LimitTime: ���̵��� ������ �ɸ��� �ð�, SceneName: �̵��� �� �̸� ������, ���̵� �ƿ� ����, GetItem: ������ ȹ�� ����
    IEnumerator PadeImageAndChangeScene(float LimitTime, string SceneName)
    {
        float isTime = 0f;//�ð����� ģ��
        Color StartColor = PadeScreen.GetComponent<Image>().color; //���� ���� ����
        //�̵��� �� ���� ���� ������ ��ǥ �� ����
        Color AColor = StartColor;

        if(SceneName == null)
            AColor.a = 0f;
        else
            AColor.a = 1f;
        while (isTime <= LimitTime)
        {
            
            float NomalizedTime = isTime / LimitTime;

            PadeScreen.GetComponent<Image>().color = Color.Lerp(StartColor, AColor, NomalizedTime);
            isTime += Time.deltaTime;
            yield return null;
        }
        //���̵� ���� ���� �� �̵�
        if(SceneName != null)
            SceneManager.LoadScene(SceneName);
    }

    //������ ����â ���� �ڷ�ƾ
    public IEnumerator SelectItemAndChangeScene(string SceneName)
    {
        itemPanel.SetActive(true);//������ ��� Ȱ��ȭ
        GameObject.FindWithTag("Player").SetActive(false); //�÷��̾� ��Ȱ��ȭ

        //������ �г��� �ڽĵ��� for������ �迭�� �Ҵ�
        int childCnt = itemPanel.transform.childCount;
        GameObject[] items = new GameObject[childCnt];
        for (int i = 0; i < childCnt; i++)
            items[i] = itemPanel.transform.GetChild(i).gameObject;

        //������ ������ ���̽����� ������ ��������
        foreach(GameObject item in items)
        {
            Item randomItem = itemManager.GetRandomItem();//������ ������ ���̽����� ������ ���� ����

            if (randomItem == null)//������ ���̽��� �������� ������ break
                break;
            
            item.transform.GetChild(0).GetComponent<Image>().sprite = randomItem.icon;//������ ����
            item.transform.GetChild(1).GetComponent<Text>().text = randomItem.itemName;//������ �̸� ����
            item.transform.GetChild(2).GetComponent<Text>().text = randomItem.description;//������ ����
            item.transform.GetChild(3).GetComponent<Text>().text = randomItem.executionFunction;//������ �Լ� ����
        }

        //������ ���� �Ҷ����� ���� ����
        while (!isSelectItem)
        {
            yield return null;
        }

        StartCoroutine(PadeImageAndChangeScene(PadeInTime, SceneName));
    }

    //������ ���� �Լ�
    public void SelectItem(GameObject thisObj)
    {
        itemPanel.SetActive(false);//������ ��� ��Ȱ��ȭ
        isSelectItem = true;//������ ���� ���� Ȱ��ȭ

        this.SendMessage(thisObj.transform.GetChild(3).GetComponent<Text>().text);
    }
}
