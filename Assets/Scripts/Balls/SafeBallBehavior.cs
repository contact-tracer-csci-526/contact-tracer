using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBallBehavior : BallBehavior
{
    public static string TAG = "SAFE_BALL";

    public SafeBallBehavior(Ball ball)
    {
        this.ball = ball;
    }

    public override void HandleOnCollisionEnter2D(Collision2D other)
    {
    }

    public override void HandleOnCollisionWith(BallType ballType) {
        if (ballType == BallType.NORMAL) {
            HandleOnCollisionWithNormalBall();
        }
    }

    private void HandleOnCollisionWithNormalBall() {
        ball.ballTransform.TransformsToNormalBall();
        ball.StartBall();
    }
}
