using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    private List<Item> availableItems;

    private void Start()
    {
        // ��� ������ ������ ����Ʈ �ʱ�ȭ
        availableItems = new List<Item>(itemDatabase.items);
    }

    public Item GetRandomItem()
    {
        if (availableItems.Count == 0)
        {
            Debug.LogWarning("No more available items.");
            return null;
        }

        // �����ϰ� ������ ����
        int randomIndex = Random.Range(0, availableItems.Count);
        Item selected = availableItems[randomIndex];

        // ������ ������ ����
        availableItems.RemoveAt(randomIndex);

        return selected;
    }
}
