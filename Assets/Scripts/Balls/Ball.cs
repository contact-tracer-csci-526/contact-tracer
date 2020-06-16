using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public static float SPEED_RATE = 1.5f;
    public BallBehavior ballBehavior;
    public BallType ballType;
    public bool isOriginalVirus;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ballBehavior = BallBehaviorFactory.Get(ballType, this);
    }

    void Update()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 v = rb.velocity.normalized;
            rb.velocity = v * SPEED_RATE;
        }
    }

    public void StartBall()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 1.0f);
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        ballBehavior.HandleOnCollisionEnter2D(other);
    }

    public void StopBall()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }


}
