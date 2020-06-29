using UnityEngine;
using System.Collections;

public abstract class BallBehavior
{
    public Ball ball;

    public abstract void HandleOnCollisionEnter2D(Collision2D other);

    public abstract void HandleOnCollisionWith(BallType ballType);
}
