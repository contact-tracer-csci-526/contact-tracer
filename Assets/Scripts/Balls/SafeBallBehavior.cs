﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBallBehavior : BallBehavior
{
    public static string TAG = "Safe";
    
     public SafeBallBehavior(Ball ball)
    {
        this.ball = ball;
    }
    public override void HandleOnCollisionEnter2D(Collision2D other)
    {
    }
}