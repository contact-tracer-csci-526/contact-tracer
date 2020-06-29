using UnityEngine;
using System.Collections;

public class BallTransform
{
    Ball ball;

    public BallTransform(Ball ball)
    {
        this.ball = ball;
    }

    public void TransformsToCureBall()
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();
        Sprite sprite = Resources.Load<Sprite>("Sprites/cureball");
        transform.gameObject.tag = VirusBallBehavior.TAG;
        ballCollider.radius = 0.6f;
        currentSprite.sprite = sprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.CURE, ball);
        ball.ballType = BallType.CURE;
    }

    public void TransformsToSafeBall()
    {
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

    public void TransformsToVirusBall()
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        Sprite virusSprite = Resources.Load<Sprite>("Sprites/coronavirus");
        transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        transform.gameObject.tag = VirusBallBehavior.TAG;
        ballCollider.radius = 0.6f;
        currentSprite.sprite = virusSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.VIRUS, ball);
        ball.ballType = BallType.VIRUS;
        changeMaxThreshold();
    }

    public void TransformsToNormalBall()
    {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        currentSprite.color = new Color(1f,1f,1f,1f);
        ball.gameObject.AddComponent<CircleCollider2D>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
        transform.gameObject.tag = NormalBallBehavior.TAG;
        ballCollider.radius = 0.19f;
        currentSprite.sprite = ballSprite;
        ball.ballBehavior = BallBehaviorFactory.Get(BallType.NORMAL, ball);
        ball.ballType = BallType.NORMAL;
    }

    protected void changeMaxThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,
                                            DrawLine.safeBalls.Count + Cells.Length - 1);
        if (DrawLine.safeBalls.Count > DrawLine.MAX_SAFE_BALLS) {
            if (DrawLine.safeBalls.Count > 0)
            {
                Ball notSafeBall = DrawLine.safeBalls[0];
                notSafeBall.ballTransform.TransformsToNormalBall();
                DrawLine.safeBalls.RemoveAt(0);
            }
        }
    }

    protected void restoreOriginalThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,
                                            DrawLine.safeBalls.Count + Cells.Length - 1);
    }
}
