using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.Start;

    private List<GameObject> balls;
    private int level;
    private GameObject[] Uninfected;
    private Image loading;
    private Text timeText;
    private Loading Loading;
    private DelayedStartScript CDS;
    private Text statusText;
    private Text scoreToPass;
    private int score = 0;
    private int minutes;
    private int sec;
    private int infectionLimit = 100;
    private int frameCount = 0;
    private int expectedScore = 20;
    private int minX;
    private int minY;
    private int maxX;
    private int maxY;
    private float minDistance;
    private int screenHeight;
    private int screenWidth;
    private int totalSeconds = 0;
    private int TOTAL_SECONDS = 0;
    private float fillamount;

    public static GameManager getInstance() {
        return instance;
    }

    public GameManager() {
        balls = new List<GameObject>();
        instance = this;
    }

    void Start()
    {
        CDS = GameObject.Find("DelayedStart").GetComponent<DelayedStartScript>();
        level = MainMenu.level;
        minX = -2;
        minY = -5;
        maxX = 2;
        maxY = 5;
        minDistance = 0.4f;
        minutes = 0;
        sec = 40;

        CreateBallsRandomly();
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        loading = GameObject.Find("fg").GetComponent<Image>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        timeText.text = minutes + " : " + sec;

        if (minutes > 0)
            totalSeconds += minutes * 60;
        if (sec > 0)
            totalSeconds += sec;

        TOTAL_SECONDS = totalSeconds;
        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "Expected Score: " + expectedScore;
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    void Update()
    {
        switch (currentGameState) {
            case GameState.Start:
                if (CDS.counterDownDone == true) {
                    for (int i = 0; i < balls.Count; i += 1) {
                        balls[i].GetComponent<Ball>().StartBall();
                    }
                    currentGameState = GameState.Playing;
                    StartCoroutine(second());
                }

                break;
            case GameState.Playing:
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                GameObject[] Frozen = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");

                int IR = (100 * Infected.Length)
                         / (Uninfected.Length + Infected.Length + Frozen.Length);
                if (IR >= infectionLimit) {
                    currentGameState = GameState.Over;
                    StopCoroutine(second());
                    Time.timeScale = 0;
                    statusText.enabled = true;
                } else if (sec == 0 && minutes == 0) {
                    currentGameState = GameState.Over;
                    timeText.text = "Time's Up!";
                    StopCoroutine(second());
                    Time.timeScale = 0;
                    int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL").Length
                                + GameObject.FindGameObjectsWithTag("SAFE_BALL").Length;
                    if (Score * 10 >= expectedScore) {
                        statusText.text = "Congrats!\n You survived! Score: "
                                          + Score * 10
                                          + "\nExpected: "
                                          + expectedScore;
                    } else {
                        statusText.text = "Level failed! \n Your Score: "
                                          + Score * 10
                                          + "\nExpected: "
                                          + expectedScore;
                    }
                    statusText.enabled = true;
                }
                break;

            case GameState.Over:
                frameCount++;

                if (frameCount > 300) {
                    currentGameState =  GameState.Start;
                    Time.timeScale = 1;
                    Uninfected = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                    statusText.text = "Score:" + Uninfected.Length;
                    SceneManager.LoadScene(1);
                }
                break;

             case GameState.Restart:
                 currentGameState =GameState.Start;
                 Time.timeScale = 1;
                break;

            default:
                break;
        }
    }

    IEnumerator second()
    {
        yield return new WaitForSeconds (1f);
        if (sec > 0) {
            sec--;
        }
        if (sec == 0 && minutes != 0) {
            sec = 60;
            minutes--;
        }
        timeText.text = minutes + " : " + sec;
        fillLoading();
        StartCoroutine(second());
    }

    private void fillLoading()
    {
        totalSeconds--;
        float fill = (float)totalSeconds/TOTAL_SECONDS;
        loading.fillAmount = fill;
    }

    // todos: WARNING: This contains hardcoded values, and SHOULD be changed later to be having
    // dynamically generated ones.
    private void CreateBallsRandomly() {
        GameObject ballPrefab = Resources.Load("Prefabs/Ball") as GameObject;
        System.Random random = new System.Random();
        float HARDCODED__virusBallX = 2.17f;
        float HARDCODED__virusBallY = -2.61f;
        int HARDCODED__cureBallCount = 1;
        int HARDCODED__virusBallCount = 1;
        int totalBallCount = 3 * level + HARDCODED__cureBallCount + HARDCODED__virusBallCount;

        for (int i = 0; i < totalBallCount; i++) {
            float x = HARDCODED__virusBallX;
            float y = HARDCODED__virusBallY;
            float distance = Vector2.Distance(new Vector2(x, y),
                                              new Vector2(HARDCODED__virusBallX,
                                                          HARDCODED__virusBallY));

            while (distance < minDistance) {
                x = random.Next(minX, maxX);
                y = random.Next(minY, maxY);
                distance = Vector2.Distance(new Vector2(x, y),
                                            new Vector2(HARDCODED__virusBallX,
                                                        HARDCODED__virusBallY));
            }

            GameObject ball = Instantiate(ballPrefab, new Vector3(x, y, 0), Quaternion.identity);
            Ball ballComponent = ball.GetComponent<Ball>();

            if (i == totalBallCount - 1) {
                ballComponent.ballTransform.TransformsToCureBall();
            } else if (i == 0) {
                ballComponent.ballTransform.TransformsToVirusBall();
                ballComponent.isOriginalVirus = true;
            } else {
                ballComponent.ballTransform.TransformsToNormalBall();
            }
            balls.Add(ball);
        }
    }
}
