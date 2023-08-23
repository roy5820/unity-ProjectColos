using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillActSizeChangeObj : MonoBehaviour
{
    public Vector2 TargetSize; // Ÿ�� ������

    public float duration = 2f; // ���ϴµ� �ɸ��� �ð�

    public int TargetKill; // ��ǥ Ÿ�� ų��
    bool isClear = false; // Ŭ���� ����
    
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ ������Ʈ
    private BoxCollider2D boxCollider; // �ڽ� �ݶ��̴� ������Ʈ

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

    // Ư�� ������Ʈ�� ������ ������ ������ ��ü�ϴ� �ڷ�ƾ
    private IEnumerator ScaleOverTime()
    {
        float startTime = Time.time;
        Vector2 startSize = spriteRenderer.size; // ��������Ʈ�� ������� ���� ũ�� ����

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            Vector2 currentSize = Vector2.Lerp(startSize, TargetSize, progress);
            spriteRenderer.size = currentSize;

            float lotationCross = (this.transform.rotation.z / 180) % 2 == 0 ? -1 : 1; 

            // ���ÿ� �ڽ� �ݶ��̴� ũ��� �����µ� ����
            if (boxCollider != null)
            {
                boxCollider.size = currentSize;
                boxCollider.offset = new Vector2(0f, (currentSize.y * 0.5f) * lotationCross); // �߽��� ���濡 ���� offset ����
            }

            yield return null; // 1 ������ ���
        }

        spriteRenderer.size = TargetSize; // ��Ȯ�� ũ��� ����

        // ���������� �ڽ� �ݶ��̴� ũ��� �����µ� ����
        if (boxCollider != null)
        {
            boxCollider.size = TargetSize;
            boxCollider.offset = new Vector2(0f, -TargetSize.y * 0.5f); // �߽��� ���濡 ���� offset ����
        }
    }
}
