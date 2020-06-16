using UnityEngine;
using System.Collections;

public abstract class BallBehavior
{
    public Ball ball;

    public abstract void HandleOnCollisionEnter2D(Collision2D other);

    public void TransformsTo(BallType ballType) {
        // Debug.Log(this.ball.ballType+ " "+ ballType);
        Transform transform = ball.transform;
        SpriteRenderer currentSprite = ball.gameObject.GetComponent<SpriteRenderer>();
        CircleCollider2D ballCollider = ball.GetComponent<CircleCollider2D>();

        if (this.ball.ballType == BallType.BALL && ballType == BallType.VIRUS) {
            // Case 1 : Normal Ball to Virus Ball when it collides with a virus
            // Debug.Log("Transform 1");
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
            // Case 2 : Virus Ball to Normal Ball when it collides with a cure ball
            // Debug.Log("Transform 2");
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
            // Case 3 : Normal Ball to Safe Ball when we draw a circle around it
            // Debug.Log("Transform 3");
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
            // Case 4 : Safe to Normal Ball when the time runs out or when he has reached the threshold        
            // Debug.Log("Transform 4");
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
        DrawLine.Cells = GameObject.FindGameObjectsWithTag("Cell");
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS,DrawLine.Cells.Length - 1);
        // Debug.Log("Here1 "+ DrawLine.MAX_SAFE_BALLS.ToString());
        if (DrawLine.safeBalls.Count >= DrawLine.MAX_SAFE_BALLS)
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
        DrawLine.Cells = GameObject.FindGameObjectsWithTag("Cell");
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,DrawLine.Cells.Length - 1);
        // Debug.Log("Here2 "+ DrawLine.MAX_SAFE_BALLS.ToString());
    }
}
