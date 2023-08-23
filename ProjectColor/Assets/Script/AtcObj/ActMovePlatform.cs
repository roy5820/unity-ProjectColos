using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMovePlatform : MonoBehaviour
{
    public Color ClearColor;
    public GameObject LinkObj;

    //Ȱ��ȭ�� ���ĥ ���� ����
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
        //objColor�� ���� ������Ʈ ������ ����
        gameObject.GetComponent<SpriteRenderer>().color = objColor;

        //����ġ�� �̺�Ʈ
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

    //���� �浹 �� �ܺο��� ȣ��Ǵ� ��ĥ�ϴ� �Լ�
    public void ActAction(Color nowColor)
    {
        Debug.Log(nowColor == ClearColor);
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

    //�ܺο��� ������ Ŭ���� ��Ű�� �Լ�
    public void ForcedClear()
    {
        objColor = ClearColor;
    }
}
