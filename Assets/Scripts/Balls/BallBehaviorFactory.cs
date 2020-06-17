using UnityEngine;
using System.Collections;

class BallBehaviorFactory {
  public static BallBehavior Get(BallType ballType, Ball ball) {
    switch (ballType) {
      case BallType.VIRUS:
        return new VirusBallBehavior(ball);
      case BallType.BALL:
        return new NormalBallBehavior(ball);
      case BallType.SAFE:
        return new SafeBallBehavior(ball);
      case BallType.CURE:
        return new CureBallBehavior(ball);
      default:
        return new NormalBallBehavior(ball);
    }
  }
}
