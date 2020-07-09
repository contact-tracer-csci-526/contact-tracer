﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public static float SPEED_RATE = 2.25f;
    public BallBehavior ballBehavior;
    public BallType ballType;
    public bool isOriginalVirus;
    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        ballBehavior = BallBehaviorFactory.Get(ballType, this);
    }

    void Update()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing) {
            Vector2 v = rigidbody.velocity.normalized;
            rigidbody.velocity = v * SPEED_RATE;
        }
    }

    public void StartBall()
    {
        if (MainMenu.level != 3) {
          rigidbody.velocity = new Vector2(1.0f, 1.0f);
        } else if (ballType == BallType.CURE) {
          rigidbody.velocity = new Vector2(-1.0f, 0.0f);
        } else if (ballType == BallType.VIRUS) {
          rigidbody.velocity = new Vector2(0.0f, 1.0f);
        } else {
          rigidbody.velocity = new Vector2(0.0f, -1.0f);
        }
        rigidbody.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (ballBehavior != null) {
            ballBehavior.HandleOnCollisionEnter2D(other);
        }
    }

    public void StopBall()
    {
        rigidbody.velocity = new Vector2(0.0f, 0.0f);
        rigidbody.isKinematic = true;
    }

    public void WARN__Initialize(BallType bt) {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        ballType = bt;
        ballBehavior = BallBehaviorFactory.Get(this.ballType, this);
    }
}
