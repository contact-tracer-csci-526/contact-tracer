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

    public override void TransformsTo(BallType ballType) {
        if (ballType == BallType.NORMAL) {
            TransformsToNormalBall();
        }
    }

    private void TransformsToNormalBall() {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
        transform.gameObject.tag = NormalBallBehavior.TAG;
        ballCollider.radius = 0.19f;
        currentSprite.sprite = ballSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.NORMAL, ball);
        ball.ballType = BallType.NORMAL;
        ball.StartBall();
    }
}
