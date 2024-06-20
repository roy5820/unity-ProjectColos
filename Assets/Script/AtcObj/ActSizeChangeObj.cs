using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActSizeChangeObj : MonoBehaviour
{
    public Color ClearColor;
    public GameObject LinkObj;

    public Vector2 TargetSize; //Ÿ�� ������

    public float duration = 2f; // ���ϴµ� �ɸ��� �ð�

    public bool isReColoring;

    Color objColor;

    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ ������Ʈ
    private BoxCollider2D boxCollider; // �ڽ� �ݶ��̴� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        objColor = this.GetComponent<SpriteRenderer>().color;
        //ũ�� ������ ���� �� �ʱ�ȭ
        spriteRenderer = LinkObj.GetComponent<SpriteRenderer>();
        boxCollider = LinkObj.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //objColor�� ������Ʈ�� �������� �ٲٱ�
        gameObject.GetComponent<SpriteRenderer>().color = objColor;
    }

    //��ĥ���� �� �۵��ϴ� �Լ�
    public void ActAction(Color nowColor)
    {
        if (objColor == ClearColor && !isReColoring)
        {
            return;
        }
        else
        {   
            objColor = nowColor;
            //����ġ �� ������Ʈ�� ũ�Ⱑ ���ϴ� �̺�Ʈ ó��
            if (objColor == ClearColor)
                StartCoroutine(ScaleOverTime());
            return;
        }
    }

    //���� Ŭ���� �Լ�
    public void ForcedClear()
    {
        objColor = ClearColor;
        StartCoroutine(ScaleOverTime());
        return;
    }

    //ũ�� ���� �ڷ�ƾ
    private IEnumerator ScaleOverTime()
    {
        float startTime = Time.time;
        Vector2 startSize = spriteRenderer.size; // ��������Ʈ�� ������� ���� ũ�� ����

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            Vector2 currentSize = Vector2.Lerp(startSize, TargetSize, progress);
            spriteRenderer.size = currentSize;
            
            // ���ÿ� �ڽ� �ݶ��̴� ũ��� �����µ� ����
            if (boxCollider != null)
            {
                boxCollider.size = currentSize;
                boxCollider.offset = new Vector2(0f, (currentSize.y * 0.5f)); // �߽��� ���濡 ���� offset ����
            }

            yield return null; // 1 ������ ���
        }

        
    }
}
