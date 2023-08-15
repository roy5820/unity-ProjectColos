using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    // üũ�� ���̾�
    public int chkLayer;

    //�浹 üũ ��
    private bool chkSensor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool colSencorState()
    {
        return chkSensor;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (chkLayer == other.gameObject.layer)
        {
            chkSensor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (chkLayer == other.gameObject.layer)
        {
            chkSensor = false;
        }
    }
}