using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarflChangScene : MonoBehaviour
{
    public string WarfSceneName; //�̵��� �� �̸�
    public bool GetItems = true; //������ ȹ�� ����
    public bool isClear = false; //Ŭ���� ����
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D ohter)
    {
         if(ohter.tag == "Player")
        {
            //Ŭ���� ���� üũ
            if (isClear)
            {
                Destroy(GameManager.instance.gameObject);
                Destroy(GameManager.instance.FindPlayer.gameObject);
                SceneManager.LoadScene(WarfSceneName);
            }
            else
            {
                //������ ȹ�� ���ο� �ٸ� ó��
                if (GetItems)
                {
                    //������ ���� �Լ� ȣ��
                    StartCoroutine(GameManager.instance.SelectItemAndChangeScene(WarfSceneName));
                }
                else
                {
                    //�� �̵� �Լ� ȣ��
                    GameManager.instance.ChangScen(WarfSceneName);
                }
            } 
        }  
    }
}
