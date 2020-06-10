using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public static int SPEED_RATE = 3;
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        ballBehavior.HandleOnCollisionEnter2D(other);
    }

}
