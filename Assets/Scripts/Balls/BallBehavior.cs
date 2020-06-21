using UnityEngine;
using System.Collections;

public abstract class BallBehavior
{
    public Ball ball;

    public abstract void HandleOnCollisionEnter2D(Collision2D other);

    public abstract void TransformsTo(BallType ballType);

    protected void changeMaxThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        // Debug.Log("here 1 "+Cells.Length.ToString() + " "+ DrawLine.MAX_SAFE_BALLS_FIXED);
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,DrawLine.safeBalls.Count + Cells.Length - 1);
        // Debug.Log("Here1 "+ DrawLine.MAX_SAFE_BALLS.ToString());
        if (DrawLine.safeBalls.Count > DrawLine.MAX_SAFE_BALLS)
        {
            if (DrawLine.safeBalls.Count > 0)
            {
                Ball notSafeBall = DrawLine.safeBalls[0];
                notSafeBall.ballBehavior.TransformsTo(BallType.NORMAL);
                DrawLine.safeBalls.RemoveAt(0);
            }
        }
    }

    protected void restoreOriginalThreshold()
    {
        GameObject[] Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        // Debug.Log("here 2 "+ Cells.Length.ToString() + " "+ DrawLine.MAX_SAFE_BALLS_FIXED);
        DrawLine.MAX_SAFE_BALLS = Mathf.Min(DrawLine.MAX_SAFE_BALLS_FIXED,DrawLine.safeBalls.Count + Cells.Length - 1);
        // Debug.Log("Here2 "+ DrawLine.MAX_SAFE_BALLS.ToString());
    }

    // protected Vector3 getRadius(){
        
    // }
}
