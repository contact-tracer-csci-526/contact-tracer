using UnityEngine;
using System.Collections;

class BallBehaviorMap {
  public static BallBehavior Get(BallType ballType, Ball ball) {
    switch (ballType) {
      case BallType.VIRUS:
        return new VirusBallBehavior(ball);
      case BallType.BALL:
      default:
        return new NormalBallBehavior(ball);
    }
  }
}
