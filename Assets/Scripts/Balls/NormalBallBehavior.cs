using UnityEngine;
using System.Collections;

public class NormalBallBehavior : BallBehavior
{
    public static string TAG = "NORMAL_BALL";

    public NormalBallBehavior(Ball ball)
    {
        this.ball = ball;
    }

    public override void HandleOnCollisionEnter2D(Collision2D other)
    {
        Ball otherBall = other.gameObject.GetComponent<Ball>();

        if (otherBall != null) {
            BallType ballType = otherBall.ballType;
            if (otherBall.ballType == BallType.VIRUS) {
                HandleOnCollisionWith(BallType.VIRUS);
            }
        }
    }

    public override void HandleOnCollisionWith(BallType ballType)
    {
        if (ballType == BallType.SAFE) {
            HandleOnCollisionWithSafeBall();
        } else if (ballType == BallType.VIRUS) {
            HandleOnCollisionWithVirusBall();
        }
    }

    private void HandleOnCollisionWithSafeBall()
    {
        ball.ballTransform.TransformsToSafeBall();
    }

    private void HandleOnCollisionWithVirusBall()
    {
        ball.ballTransform.TransformsToVirusBall();
    }
}
