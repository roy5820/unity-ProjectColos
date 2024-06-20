using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    private List<Item> availableItems;
    private PlayerController playerController;//�÷��̾� ��Ʈ�ѷ� ������ ���� ���� ����

    private void Start()
    {
        ItemSeting();
    }

    private void Update()
    {
        if (playerController == null)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    // ��� ������ ������ ����Ʈ �ʱ�ȭ �Լ�
    public void ItemSeting()
    {
        availableItems = new List<Item>(itemDatabase.items);
    }

    //������ �����ͺ��̽����� ���� ������ �����ϱ�
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

    //������ȿ�� ���� �κ�

    //�Ķ��Ķ� ��ų ����
    public void UpgradeBlueSkill()
    {
        playerController.BlueSkillCoefficient += 0.5f; // ������ ��� ����
        playerController.BlueSkillJumpPower += 3f; //������ ����
    }

    //�������� ��ų ����
    public void UpgradeRedSkill()
    {
        playerController.RedSkillCoefficient += 0.3f;
        playerController.RedSkillStaminaCoat += 5;
    }

    //�ʷ��ʷ� ��ų ����
    public void UpgradeGreenSkill()
    {
        playerController.GreenSkillCoefficient += 0.3f;
        playerController.GreenSkillKnockBackPower += 3.5f;
    }

    //���ƶ󳯾� ��ų ����
    public void UpgradeExtraJump()
    {
        playerController.extraJumpMaxCnt += 1;
    }
}