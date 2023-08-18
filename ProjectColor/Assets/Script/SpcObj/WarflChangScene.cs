using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarflChangScene : MonoBehaviour
{
    public string WarfSceneName; //�̵��� �� �̸�
    public bool GetItems = true; //������ ȹ�� ����
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
            GameManager Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            //������ ȹ�� ���ο� �ٸ� ó��
            if (GetItems)
            {
                //������ ���� �Լ� ȣ��
                StartCoroutine(Manager.SelectItemAndChangeScene(WarfSceneName));
            }
            else
            {
                //�� �̵� �Լ� ȣ��
                Manager.ChangScen(WarfSceneName);
            }
            
                
        }  
    }
}
