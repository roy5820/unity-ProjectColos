using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockArray
{
    public GameObject[] Blocks;
}

public class ColoringController : MonoBehaviour
{
    //��ĥ�ؾ��ϴ� ��
    public BlockArray[] ColoringBlocks;
    //�������ϴ� ��
    public BlockArray[] ClearColorBlocks;
    //������ �۵��� ������Ʈ
    public GameObject ActObj;

    int DifferentCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ActColoringObj���� ����ĥ������ �ش� �� �Ǵ� �࿡ ���� �ϰ������� ĥ�ϱ� RowOrCol 0: Row 1: Col, LinePosition ��ĥ�� ���� ��ȣ 0���� ����
    public void ColoringTheBlock(int RowOrCol, int LinePosition, Color FillColor)
    {
        int maxCount = RowOrCol == 0 ? ColoringBlocks[LinePosition].Blocks.Length : ColoringBlocks.Length;
        Color ThisColor;
        Color ClearColor;
        for(int i = 0; i < maxCount; i++)
        {
            if(RowOrCol == 0)
            {
                Color oldColor = ColoringBlocks[LinePosition].Blocks[i].GetComponent<SpriteRenderer>().color;
                Color newColor = new Color(
                    Mathf.Clamp(oldColor.r + FillColor.r, 0f, 1f), // ������ �� ���ϱ�
                    Mathf.Clamp(oldColor.g + FillColor.g, 0f, 1f), // �ʷϻ� �� ���ϱ�
                    Mathf.Clamp(oldColor.b + FillColor.b, 0f, 1f), // �Ķ��� �� ���ϱ�
                    oldColor.a // ���� ���� �� ����
                );
                ColoringBlocks[LinePosition].Blocks[i].GetComponent<SpriteRenderer>().color = newColor;
            }
            else if(RowOrCol == 1)
            {
                Color oldColor = ColoringBlocks[i].Blocks[LinePosition].GetComponent<SpriteRenderer>().color;
                Color newColor = new Color(
                    Mathf.Clamp(oldColor.r + FillColor.r, 0f, 1f), // ������ �� ���ϱ�
                    Mathf.Clamp(oldColor.g + FillColor.g, 0f, 1f), // �ʷϻ� �� ���ϱ�
                    Mathf.Clamp(oldColor.b + FillColor.b, 0f, 1f), // �Ķ��� �� ���ϱ�
                    oldColor.a // ���� ���� �� ����
                );
                ColoringBlocks[i].Blocks[LinePosition].GetComponent<SpriteRenderer>().color = newColor;
            }
        }

        //Ŭ���� üũ
        DifferentCount = 0;
        for (int i = 0; i < ColoringBlocks.Length; i++)
        {
            for(int j = 0; j < ColoringBlocks[i].Blocks.Length; j++)
            {
                ThisColor = ColoringBlocks[i].Blocks[j].GetComponent<SpriteRenderer>().color;
                ClearColor = ClearColorBlocks[i].Blocks[j].GetComponent<SpriteRenderer>().color;

                if (ThisColor != ClearColor) DifferentCount++;
            }
        }
        if (DifferentCount == 0)
        {
            ActObj.gameObject.SendMessage("ForcedClear");
        }
    }


    public void ClearBlocks()
    {
        for (int i = 0; i < ColoringBlocks.Length; i++)
        {
            for (int j = 0; j < ColoringBlocks[i].Blocks.Length; j++)
            {
                ColoringBlocks[i].Blocks[j].GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
