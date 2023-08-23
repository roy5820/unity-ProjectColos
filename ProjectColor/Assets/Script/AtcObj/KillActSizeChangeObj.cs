using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillActSizeChangeObj : MonoBehaviour
{
    public Vector2 TargetSize; // 타겟 사이즈

    public float duration = 2f; // 변하는데 걸리는 시간

    public int TargetKill; // 목표 타겟 킬수
    bool isClear = false; // 클리어 여부
    
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트
    private BoxCollider2D boxCollider; // 박스 콜라이더 컴포넌트

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (TargetKill <= GameManager.instance.KillEnemyCnt && !isClear)
        {
            isClear = true;
            StartCoroutine(ScaleOverTime());
        }
    }

    // 특정 오브젝트를 설정한 값으로 서서히 교체하는 코루틴
    private IEnumerator ScaleOverTime()
    {
        float startTime = Time.time;
        Vector2 startSize = spriteRenderer.size; // 스프라이트의 사이즈로 시작 크기 설정

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            Vector2 currentSize = Vector2.Lerp(startSize, TargetSize, progress);
            spriteRenderer.size = currentSize;

            float lotationCross = (this.transform.rotation.z / 180) % 2 == 0 ? -1 : 1; 

            // 동시에 박스 콜라이더 크기와 오프셋도 조정
            if (boxCollider != null)
            {
                boxCollider.size = currentSize;
                boxCollider.offset = new Vector2(0f, (currentSize.y * 0.5f) * lotationCross); // 중심점 변경에 따른 offset 조정
            }

            yield return null; // 1 프레임 대기
        }

        spriteRenderer.size = TargetSize; // 정확한 크기로 조정

        // 마찬가지로 박스 콜라이더 크기와 오프셋도 조정
        if (boxCollider != null)
        {
            boxCollider.size = TargetSize;
            boxCollider.offset = new Vector2(0f, -TargetSize.y * 0.5f); // 중심점 변경에 따른 offset 조정
        }
    }
}
