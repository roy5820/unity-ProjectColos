using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    private List<Item> availableItems;
    private PlayerController playerController;//플레이어 컨트롤러 연결을 위한 변수 선언

    private void Start()
    {
        // 사용 가능한 아이템 리스트 초기화
        availableItems = new List<Item>(itemDatabase.items);
    }

    private void Update()
    {
        if (playerController == null)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    //아이템 데이터베이스에서 랜덤 아이템 추출하기
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

    //아이템효과 구현 부분

    //파랑파랑 스킬 구현
    public void UpgradeBlueSkill()
    {
        playerController.BlueSkillCoefficient += 0.3f; // 데미지 계수 증가
        playerController.BlueSkillJumpPower += 3f; //점프력 증가
    }

    //빨강빨강 스킬 구현
    public void UpgradeRedSkill()
    {
        playerController.RedSkillCoefficient += 0.3f;
        playerController.RedSkillStaminaCoat += 5;
    }

    //초록초록 스킬 구현
    public void UpgradeGreenSkill()
    {
        playerController.GreenSkillCoefficient += 0.3f;
        playerController.GreenSkillKnockBackPower += 10f;
    }
}
