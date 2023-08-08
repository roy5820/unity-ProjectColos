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
    //색칠해야하는 블럭
    public BlockArray[] ColoringBlocks;
    //만들어야하는 블럭
    public BlockArray[] ClearColorBlocks;
    //성공시 작동할 오브젝트
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
    //ActColoringObj에서 색이칠해지면 해당 열 또는 행에 색을 일괄적으로 칠하기 RowOrCol 0: Row 1: Col, LinePosition 색칠할 줄의 번호 0부터 시작
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
                    Mathf.Clamp(oldColor.r + FillColor.r, 0f, 1f), // 빨간색 값 더하기
                    Mathf.Clamp(oldColor.g + FillColor.g, 0f, 1f), // 초록색 값 더하기
                    Mathf.Clamp(oldColor.b + FillColor.b, 0f, 1f), // 파란색 값 더하기
                    oldColor.a // 원래 알파 값 유지
                );
                ColoringBlocks[LinePosition].Blocks[i].GetComponent<SpriteRenderer>().color = newColor;
            }
            else if(RowOrCol == 1)
            {
                Color oldColor = ColoringBlocks[i].Blocks[LinePosition].GetComponent<SpriteRenderer>().color;
                Color newColor = new Color(
                    Mathf.Clamp(oldColor.r + FillColor.r, 0f, 1f), // 빨간색 값 더하기
                    Mathf.Clamp(oldColor.g + FillColor.g, 0f, 1f), // 초록색 값 더하기
                    Mathf.Clamp(oldColor.b + FillColor.b, 0f, 1f), // 파란색 값 더하기
                    oldColor.a // 원래 알파 값 유지
                );
                ColoringBlocks[i].Blocks[LinePosition].GetComponent<SpriteRenderer>().color = newColor;
            }
        }

        //클리어 체크
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
