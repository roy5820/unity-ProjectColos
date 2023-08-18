using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ Ŭ���� ����
[System.Serializable]
public class Item
{
    public string itemName;//������ ��
    public string description; // ������ ����
    public Sprite icon;//������ ������;
    public string executionFunction;//�����Լ�
}

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items;
}
