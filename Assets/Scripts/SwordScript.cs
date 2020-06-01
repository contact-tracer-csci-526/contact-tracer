using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeeed = 300f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();   
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeeed, direction.y * moveSpeeed);
        }
        else {
            rb.velocity = Vector2.zero;
        }
    }
}
