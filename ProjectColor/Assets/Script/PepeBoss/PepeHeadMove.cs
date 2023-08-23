using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepeHeadMove : MonoBehaviour
{
    Rigidbody2D Rbody;

    public float MoveSpeed = 10f;//�����̴� �ӵ�
    public float WaitTime = 5.0f; //��� �ð�

    // Start is called before the first frame update
    void Start()
    {
        Rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        WaitTime -= Time.deltaTime;
        if(WaitTime <= 0)
            Rbody.velocity = new Vector2(MoveSpeed, 0);
    }
}
