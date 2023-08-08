using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarflChangScene : MonoBehaviour
{
    public string WarfSceneName;
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
            GameObject Manager = GameObject.Find("GameManager");
            if(Manager.GetComponent<GameManager>().PadeInScreen())
                SceneManager.LoadScene(WarfSceneName);
        }  
    }
}
