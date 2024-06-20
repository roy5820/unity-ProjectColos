using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActColoringObj : MonoBehaviour
{
    public GameObject MainColoringController;

    public int RowOrCol = 0; // 0: row, 1: col
    public int LinePosition = 1; // ��� °���� ��ĥ �� ������ 0���� ����

    public Color objColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�� �����ϱ�
        gameObject.GetComponent<SpriteRenderer>().color = objColor;

        //���� ������ �ݶ��̴� ��Ȱ��ȭ
        if (objColor != Color.white)
            //��ĥ �� �� �ݶ��̴� ��Ȱ��ȭ 
            this.GetComponent<BoxCollider2D>().enabled = false;
        else
            //��ĥ �� �� �ݶ��̴� ��Ȱ��ȭ 
            this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ActAction(Color nowColor)
    {
        

        objColor = nowColor;

         MainColoringController.GetComponent<ColoringController>().ColoringTheBlock(RowOrCol, LinePosition, objColor);
         return;
    }
}
