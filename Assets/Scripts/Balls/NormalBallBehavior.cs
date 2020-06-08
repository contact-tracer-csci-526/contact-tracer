using UnityEngine;
using System.Collections;

public class NormalBallBehavior : BallBehavior
{
    public static string TAG = "Ball";

    public NormalBallBehavior(
        Ball ball
    )
    {
        this.ball = ball;
    }

    public override void HandleOnCollisionEnter2D(Collision2D other)
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        Ball otherBall = other.gameObject.GetComponent<Ball>();

        if (otherBall != null) {
            bool isVirusHit = otherBall.ballType == BallType.VIRUS;
            if (isVirusHit) {
                TransformTo(BallType.VIRUS);
            }
        }
    }
}
