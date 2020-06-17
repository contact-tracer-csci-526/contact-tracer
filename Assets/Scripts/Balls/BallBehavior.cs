using UnityEngine;
using System.Collections;

public abstract class BallBehavior
{
    public Ball ball;
    public abstract void HandleOnCollisionEnter2D(Collision2D other);

    public void TransformsTo(BallType ballType) {
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        if (this.ball.ballType == BallType.BALL && ballType == BallType.VIRUS) {
            Sprite virusSprite = Resources.Load<Sprite>("Sprites/coronavirus");
            transform.gameObject.tag = VirusBallBehavior.TAG;
            ballCollider.radius = 0.6f;
            currentSprite.sprite = virusSprite;
            ball.ballBehavior = BallBehaviorFactory.Get(BallType.VIRUS, ball);
            ball.ballType = BallType.VIRUS;
            changeMaxThreshold();
        }
        else if(this.ball.ballType == BallType.VIRUS && ballType == BallType.BALL)
        {
            Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
            transform.gameObject.tag = NormalBallBehavior.TAG;
            ballCollider.radius = 0.23f;
            currentSprite.sprite = ballSprite;
            ball.ballBehavior = BallBehaviorFactory.Get(BallType.BALL, ball);
            ball.ballType = BallType.BALL;
            restoreOriginalThreshold();
        }
        else if(this.ball.ballType == BallType.BALL && ballType == BallType.SAFE)
        {
            Sprite safeSprite = Resources.Load<Sprite>("Sprites/safe");
            transform.gameObject.tag = SafeBallBehavior.TAG;
            ballCollider.radius = 0.48f;
            currentSprite.sprite = safeSprite;
            ball.ballBehavior = BallBehaviorFactory.Get(BallType.SAFE, ball);
            ball.ballType = BallType.SAFE;
            ball.StopBall();
        }
        else if (this.ball.ballType == BallType.SAFE && ballType == BallType.BALL)
        {
            Sprite ballSprite = Resources.Load<Sprite>("Sprites/ball");
            transform.gameObject.tag = NormalBallBehavior.TAG;
            ballCollider.radius = 0.23f;
            currentSprite.sprite = ballSprite;
            ball.ballBehavior = BallBehaviorFactory.Get(BallType.BALL, ball);
            ball.ballType = BallType.BALL;
            ball.StartBall();
        }
    }

    public void changeMaxThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("Cell");
        // Debug.Log("here 1 "+Cells.Length.ToString() + " "+ DrawLine.MAX_SAFE_BALLS_FIXED);
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,DrawLine.safeBalls.Count + Cells.Length - 1);
        // Debug.Log("Here1 "+ DrawLine.MAX_SAFE_BALLS.ToString());
        if (DrawLine.safeBalls.Count > DrawLine.MAX_SAFE_BALLS)
        {
            if (DrawLine.safeBalls.Count > 0)
            {
                Ball notSafeBall = DrawLine.safeBalls[0];
                notSafeBall.ballBehavior.TransformsTo(BallType.BALL);
                DrawLine.safeBalls.RemoveAt(0);
            }
        }
    }
    public void restoreOriginalThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("Cell");
        // Debug.Log("here 2 "+ Cells.Length.ToString() + " "+ DrawLine.MAX_SAFE_BALLS_FIXED);
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,DrawLine.safeBalls.Count + Cells.Length - 1);
        // Debug.Log("Here2 "+ DrawLine.MAX_SAFE_BALLS.ToString());
    }
}
