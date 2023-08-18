using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 클래스 생성
[System.Serializable]
public class Item
{
    public string itemName;//아이템 명
    public string description; // 아이템 설명
    public Sprite icon;//아이템 아이콘;
    public string executionFunction;//실행함수
}

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items;
}
