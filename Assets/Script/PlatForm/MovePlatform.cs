using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //이동 여부
    public bool isMove = false;

    //색칠 여부


    //이동 속도
    public float moveSpeed = 3;
    public int startPoint = 0;
    public Transform[] points;

    float thisX;
    float thisY;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        if (points != null && points[i] != null && points.Length > 0)
        {
            transform.position = points[startPoint].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (points != null && points.Length > 0 && isMove)
        {
            if (points[i] != null)
            {
                if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
                {
                    i++;
                    if (i >= points.Length)
                    {
                        i = 0;
                    }
                }
                transform.position = Vector2.MoveTowards(transform.position, points[i].position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
