using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActSizeChangeObj : MonoBehaviour
{
    public Color ClearColor;
    public GameObject LinkObj;

    public Vector2 TargetSize; //타겟 사이즈

    public float duration = 2f; // 변하는데 걸리는 시간

    public bool isReColoring;

    Color objColor;

    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트
    private BoxCollider2D boxCollider; // 박스 콜라이더 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        objColor = this.GetComponent<SpriteRenderer>().color;
        //크기 설정을 위한 값 초기화
        spriteRenderer = LinkObj.GetComponent<SpriteRenderer>();
        boxCollider = LinkObj.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //objColor를 오브젝트의 색상으로 바꾸기
        gameObject.GetComponent<SpriteRenderer>().color = objColor;
    }

    //색칠해질 때 작동하는 함수
    public void ActAction(Color nowColor)
    {
        if (objColor == ClearColor && !isReColoring)
        {
            return;
        }
        else
        {   
            objColor = nowColor;
            //색일치 시 오브젝트의 크기가 변하는 이벤트 처리
            if (objColor == ClearColor)
                StartCoroutine(ScaleOverTime());
            return;
        }
    }

    //강제 클리어 함수
    public void ForcedClear()
    {
        objColor = ClearColor;
        StartCoroutine(ScaleOverTime());
        return;
    }

    //크기 조절 코루틴
    private IEnumerator ScaleOverTime()
    {
        float startTime = Time.time;
        Vector2 startSize = spriteRenderer.size; // 스프라이트의 사이즈로 시작 크기 설정

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            Vector2 currentSize = Vector2.Lerp(startSize, TargetSize, progress);
            spriteRenderer.size = currentSize;
            
            // 동시에 박스 콜라이더 크기와 오프셋도 조정
            if (boxCollider != null)
            {
                boxCollider.size = currentSize;
                boxCollider.offset = new Vector2(0f, (currentSize.y * 0.5f)); // 중심점 변경에 따른 offset 조정
            }

            yield return null; // 1 프레임 대기
        }

        
    }
}
