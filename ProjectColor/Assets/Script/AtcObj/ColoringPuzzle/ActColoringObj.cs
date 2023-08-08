using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActColoringObj : MonoBehaviour
{
    public GameObject MainColoringController;

    public int RowOrCol = 0; // 0: row, 1: col
    public int LinePosition = 1; // 몇번 째줄을 색칠 할 것인지 0부터 시작

    public Color objColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //색 적용하기
        gameObject.GetComponent<SpriteRenderer>().color = objColor;

        //색이 있으면 콜라이더 비활성화
        if (objColor != Color.white)
            //색칠 될 시 콜라이더 비활성화 
            this.GetComponent<BoxCollider2D>().enabled = false;
        else
            //색칠 될 시 콜라이더 비활성화 
            this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ActAction(Color nowColor)
    {
        

        objColor = nowColor;

         MainColoringController.GetComponent<ColoringController>().ColoringTheBlock(RowOrCol, LinePosition, objColor);
         return;
    }
}
