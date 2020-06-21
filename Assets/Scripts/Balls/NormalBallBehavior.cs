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
                TransformsToVirusBall();
            }
        }
    }

    public override void TransformsTo(BallType ballType) {
        if (ballType == BallType.SAFE) {
            TransformsToSafeBall();
        } else if (ballType == BallType.VIRUS) {
            TransformsToVirusBall();
        }
    }

    private void TransformsToSafeBall() {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        currentSprite.color = new Color(1f,1f,1f,.1f);
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        Sprite safeSprite = Resources.Load<Sprite>("Sprites/safe");
        transform.gameObject.tag = SafeBallBehavior.TAG;

        ballCollider.radius = 0.46f;
        
        currentSprite.sprite = safeSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.SAFE, ball);
        ball.ballType = BallType.SAFE;
        ball.StopBall();
    }

    private void TransformsToVirusBall()
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        Sprite virusSprite = Resources.Load<Sprite>("Sprites/coronavirus");
        transform.gameObject.tag = VirusBallBehavior.TAG;
        ballCollider.radius = 0.6f;
        currentSprite.sprite = virusSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.VIRUS, ball);
        ball.ballType = BallType.VIRUS;
        changeMaxThreshold();
    }
}
