using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour
{
    private static float SPEED_RATE = 2.25f;

    public BallBehavior ballBehavior;
    public BallType ballType;
    public bool isOriginalVirus;

    private float squareBounceRandomVector = 0.2f;
    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        ballBehavior = BallBehaviorFactory.Get(ballType, this);
    }

    void Update()
    {
        if (GameManager.CurrentGameState == GameState.Playing) {
            Vector2 v = rigidbody.velocity.normalized;
            rigidbody.velocity = v * SPEED_RATE;
        }
    }

    public void StartBall(float x = 0, float y = 0)
    {
        System.Random random = new System.Random();
        float speed = Mathf.Sqrt(2);
        float angle = random.Next(0, 360);
        float _x = Mathf.Sin(Mathf.Deg2Rad * angle) * speed;
        float _y = Mathf.Cos(Mathf.Deg2Rad * angle) * speed;
        rigidbody.isKinematic = false;

        if (x == 0 && y == 0) {
            rigidbody.velocity = new Vector2(_x, _y);
        } else {
            rigidbody.velocity = new Vector2(x, y);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (ballBehavior != null) {
            ballBehavior.HandleOnCollisionEnter2D(other);

            float threshold = 0.01f;
            Vector2 vel = other.relativeVelocity;
            if (-threshold < vel.x && vel.x < threshold) {
                Vector2 desiredDirection = new Vector2(squareBounceRandomVector,
                                                       vel.y);
                rigidbody.velocity = desiredDirection;
                squareBounceRandomVector *= -1;
            } else if (-threshold < vel.y && vel.y < threshold) {
                Vector2 desiredDirection = new Vector2(vel.x,
                                                      squareBounceRandomVector);
                rigidbody.velocity = desiredDirection;
                squareBounceRandomVector *= -1;
            }
        }
    }

    public void StopBall()
    {
        rigidbody.velocity = new Vector2(0.0f, 0.0f);
        rigidbody.isKinematic = true;
    }

    public void WARN__Initialize(BallType bt) {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        ballType = bt;
        ballBehavior = BallBehaviorFactory.Get(this.ballType, this);
    }
}
