using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float JumpPower = 10f;
    Rigidbody2D Rbody;

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
            Rbody = other.gameObject.GetComponent<Rigidbody2D>();
            Rbody.velocity = new Vector2(Rbody.velocity.x, 0);
            Rbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
        }
    }
}
