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
        Ball otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall != null) {
            if (this.ball.isOriginalVirus && otherBall.ballType == BallType.NORMAL) {
                Transform transform = ball.transform;
                float x = transform.localScale.x;
                float y = transform.localScale.y;
                transform.localScale = new Vector2(x < 2 ? x * 1.2f : x, y < 2 ? y * 1.2f : y);
            }
            bool isCureBallHit=otherBall.ballType==BallType.CURE;
            if (isCureBallHit && !this.ball.isOriginalVirus) {
                TransformsToNormalBall();
            }
        }
    }

    public override void TransformsTo(BallType ballType) {
        if (ballType == BallType.NORMAL) {
            TransformsToNormalBall();
        }
    }

    private void TransformsToNormalBall()
    {
        Transform transform = ball.transform;
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();

        Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
        transform.gameObject.tag = NormalBallBehavior.TAG;
        ballCollider.radius = 0.19f;
        currentSprite.sprite = ballSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.NORMAL, ball);
        ball.ballType = BallType.NORMAL;
        restoreOriginalThreshold();
    }
}
