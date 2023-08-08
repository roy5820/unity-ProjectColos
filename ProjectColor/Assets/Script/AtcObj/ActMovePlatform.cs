using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMovePlatform : MonoBehaviour
{
    public Color ClearColor;
    public GameObject LinkObj;

    //활성화시 재색칠 가능 여부
    public bool isReColoring = false;

    Color objColor;
    // Start is called before the first frame update
    void Start()
    {
        objColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        //objColor의 색을 오브젝트 색으로 변경
        gameObject.GetComponent<SpriteRenderer>().color = objColor;

        //색일치시 이벤트
        if (objColor == ClearColor)
        {
            if (!isReColoring)
                this.GetComponent<BoxCollider2D>().enabled = false;
             LinkObj.GetComponent<MovePlatform>().isMove = true;
        }
        else
        {
            LinkObj.GetComponent<MovePlatform>().isMove = false;

        }
    }

    //공격 충돌 시 외부에서 호출되는 색칠하는 함수
    public void ActAction(Color nowColor)
    {
        if (objColor == ClearColor && !isReColoring)
        {
            return;

        }
        else
        {
            objColor = nowColor;
            return;
        }
    }

    //외부에서 강제로 클리어 시키는 함수
    public void ForcedClear()
    {
        objColor = ClearColor;
    }
}
