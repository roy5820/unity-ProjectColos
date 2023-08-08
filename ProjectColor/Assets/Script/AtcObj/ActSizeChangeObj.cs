using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActSizeChangeObj : MonoBehaviour
{
    public Color ClearColor;
    public GameObject LinkObj;

    public Vector2 TartgetSize; //타겟 사이즈

    public float duration = 2f; // 변하는데 걸리는 시간

    public bool isReColoring;

    Color objColor;
    // Start is called before the first frame update
    void Start()
    {
        objColor = this.GetComponent<SpriteRenderer>().color;
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

    //외부에서 강제로 클리어 시키는 함수
    public void ForcedClear()
    {
        objColor = ClearColor;
        StartCoroutine(ScaleOverTime());
    }

    //특정 오브젝트를 설정한 값으로 서서히 교체하는 코루틴
    private IEnumerator ScaleOverTime()
    {
        float StartTime = Time.time;
        Vector2 StartSize = LinkObj.transform.localScale;

        while (Time.time - StartTime < duration)
        {
            float progress = (Time.time - StartTime) / duration;
            Vector2 currentSize = Vector2.Lerp(StartSize, TartgetSize, progress);
            LinkObj.transform.localScale = currentSize;
            yield return null; // 1 프레임 대기
        }

        LinkObj.transform.localScale = TartgetSize; // 정확한 크기로 조정
    }
}
