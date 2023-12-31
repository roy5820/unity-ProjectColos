using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarflChangScene : MonoBehaviour
{
    public string WarfSceneName; //이동할 씬 이름
    public bool GetItems = true; //아이템 획득 여부
    public bool isClear = false; //클리어 여부
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
            //클리어 여부 체크
            if (isClear)
            {
                Destroy(GameManager.instance.gameObject);
                Destroy(GameManager.instance.FindPlayer.gameObject);
                SceneManager.LoadScene(WarfSceneName);
            }
            else
            {
                //아이템 획득 여부에 다른 처리
                if (GetItems)
                {
                    //아이템 선택 함수 호출
                    StartCoroutine(GameManager.instance.SelectItemAndChangeScene(WarfSceneName));
                }
                else
                {
                    //씬 이동 함수 호출
                    GameManager.instance.ChangScen(WarfSceneName);
                }
            } 
        }  
    }
}
