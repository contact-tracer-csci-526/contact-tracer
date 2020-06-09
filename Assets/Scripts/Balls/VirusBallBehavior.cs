using UnityEngine;
using System.Collections;

public class VirusBallBehavior : BallBehavior
{
    public static string TAG = "Virus";

    public VirusBallBehavior(Ball ball)
    {
        this.ball = ball;
    }

    public override void HandleOnCollisionEnter2D(Collision2D other)
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        Ball otherBall = other.gameObject.GetComponent<Ball>();

        if (otherBall != null) {
            // todo: this may be extended later
        }
    }
}
