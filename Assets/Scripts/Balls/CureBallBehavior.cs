using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureBallBehavior : BallBehavior
{
	public static string TAG="Cure";
   
	public CureBallBehavior(Ball ball){
	   this.ball=ball;
       //Debug.Log(this.ball.currentTime);
	}


     public override void HandleOnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("collision "+this.ball.currentTime);
        Ball otherBall=other.gameObject.GetComponent<Ball>();
        if(otherBall!=null){
            bool isVirusHit=otherBall.ballType==BallType.VIRUS;
            if(isVirusHit){
                TransformsTo(BallType.CURE);
            }
        }
    }
 
}