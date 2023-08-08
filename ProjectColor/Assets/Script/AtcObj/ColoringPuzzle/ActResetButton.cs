using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActResetButton : MonoBehaviour
{
    public GameObject MainColoringController;

    public GameObject ActColoringObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActAction(Color nowColor)
    {
        int ChildCount = ActColoringObj.transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            ActColoringObj.transform.GetChild(i).GetComponent<ActColoringObj>().objColor = Color.white;
        }

        MainColoringController.GetComponent<ColoringController>().ClearBlocks();
    }
}
