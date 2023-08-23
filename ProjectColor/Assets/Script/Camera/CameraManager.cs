using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 화면 이동 제한 값
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    //움직일 배경 지정
    public GameObject backGround;
    public GameObject subScreen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float x = player.transform.position.x;
            float y = player.transform.position.y + 5;
            float z = transform.position.z;

            if (x < leftLimit)
                x = leftLimit;
            else if (x > rightLimit)
                x = rightLimit;

            if (y > topLimit)
                y = topLimit;
            else if (y < bottomLimit)
                y = bottomLimit;

            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3((x-10) / 3.0f, y, z);
                subScreen.transform.position = v;
            }

            if (backGround != null)
            {
                y = backGround.transform.position.y;
                z = backGround.transform.position.z;
                Vector3 v = new Vector3(x / 1.5f, y, z);
                backGround.transform.position = v;
            }
        }
    }
}
