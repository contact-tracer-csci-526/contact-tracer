using UnityEngine;
using System.Collections;

class GameConfigFactory {
  public static GameConfig Get(GameLevel gameLevel) {
    switch (gameLevel) {
      case GameLevel.Level_1:
      default:
        Debug.Log("232323");
        return new GameLevel1Conf();
    }
  }
}
