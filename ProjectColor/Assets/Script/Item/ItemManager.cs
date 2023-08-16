using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    private List<Item> availableItems;

    private void Start()
    {
        // 사용 가능한 아이템 리스트 초기화
        availableItems = new List<Item>(itemDatabase.items);
    }

    public Item GetRandomItem()
    {
        if (availableItems.Count == 0)
        {
            Debug.LogWarning("No more available items.");
            return null;
        }

        // 랜덤하게 아이템 선택
        int randomIndex = Random.Range(0, availableItems.Count);
        Item selected = availableItems[randomIndex];

        // 선택한 아이템 제외
        availableItems.RemoveAt(randomIndex);

        return selected;
    }
}
