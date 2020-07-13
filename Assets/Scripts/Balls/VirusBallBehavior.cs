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
        LineScript line = other.gameObject.GetComponent<LineScript>();
        if (otherBall != null) {
            bool isCureBallHit = otherBall.ballType == BallType.CURE;
            if (isCureBallHit && !ball.isOriginalVirus) {
                TransformsToNormalBall();
            }
        }
    }

    public override void TransformsTo(BallType ballType)
    {
        if (ballType == BallType.NORMAL) {
            TransformsToNormalBall();
        }
    }

    public IEnumerator TurnsIntoNormalLooking(int ticks = 5)
    {
        Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
        Sprite virusSprite = Resources.Load<Sprite>("Sprites/original-virus");

        for (int i = 0; i < ticks; i += 1) {
            yield return new WaitForSeconds(0.5f);

            SpriteRenderer currentSprite = ball.gameObject
                                            .GetComponent<SpriteRenderer>();
            currentSprite.sprite = i % 2 == 0 ? ballSprite : virusSprite;
        }
    }

    private void TransformsToNormalBall()
    {
        Transform transform = ball.transform;
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();
        SpriteRenderer currentSprite = ball.gameObject
                                           .GetComponent<SpriteRenderer>();

        Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
        transform.gameObject.tag = NormalBallBehavior.TAG;
        ballCollider.radius = 0.6f;
        currentSprite.sprite = ballSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.NORMAL, ball);
        ball.ballType = BallType.NORMAL;
        restoreOriginalThreshold();
    }
}
