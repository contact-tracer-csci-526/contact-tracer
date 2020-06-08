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

        if (ballType == BallType.VIRUS) {
            Sprite virusSprite = Resources.Load<Sprite>("Sprites/coronavirus");
            transform.gameObject.tag = VirusBallBehavior.TAG;
            ballCollider.radius = 0.6f;
            currentSprite.sprite = virusSprite;
            ball.ballBehavior = BallBehaviorMap.Get(BallType.VIRUS, ball);
            ball.ballType = BallType.VIRUS;
        }
    }
}
