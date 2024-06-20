using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Transform SpawnPoint;
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //플레이어 컨트롤러 연결
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            other.gameObject.GetComponent<Transform>().position = SpawnPoint.position;

            Transform thisT = gameObject.GetComponent<Transform>();

            playerController.Hurt(Damage, 0, thisT);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
