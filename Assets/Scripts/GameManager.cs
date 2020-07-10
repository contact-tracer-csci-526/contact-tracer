using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameState CurrentGameState = GameState.Start;
    public static GameObject[] Cells;
    public static GameObject Virus;
    public static GameObject CureBall;
    public static GameObject tutorialLine;
    public static GameObject handObject;
    public static GameObject DYNAMIC__cureBall;
    public static DelayedStartScript CDS;

    public GameObject NextTutorial;
    public Text statusText;
    public Text scoreToPass;
    public int score = 0;
    public GameObject ballPrefab;
    public GameObject[] Uninfected;
    public GameObject GameOverWin;
    public GameObject GameOverLose;
    public Text scoreLose;
    public GameObject linePrefab;
    public Sprite handSprite;
    
    public Text timeText;
    public int sec;
    public int totalSeconds = 0;
    public int TOTAL_SECONDS = 0;
    public GameObject CureBallGameObject;
    public float fillamount;
    public int degrees = 0;
    public int cureBallLifeTime = 7;
    public int cureBallRegenerateInterval = 10;
    public int currentTime = 0;
    public int previousTime = 0;
    public bool shouldCureballRender = true;

   
    private int infectionLimit = 100;
    private const int INFECTION_RATIO_LIMIT = 100;
    private int frameCount = 0;
    private int expectedScore;
    private GameObject tutorialCircle;
    private LineRenderer lineRenderer;
    private Vector3 lineStart = new Vector3(-2, 0, 0);
    private Vector3 lineEnd = new Vector3(2, 0, 0);
    private int minX;
    private int minY;
    private int maxX;
    private int maxY;
    private float minDistance;
    private int screenHeight;
    private int screenWidth;

    void Start()
    {
        InitializeGameScene();
        CurrentGameState = GameState.Start;
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.TUTORIAL1_PRELIMINARY:
                lineRenderer.SetPosition(0, lineStart);
                lineRenderer.SetPosition(1, lineEnd);
                MoveHandInStraightLine();
                break;

            case GameState.TUTORIAL2_PRELIMINARY:
                MoveHandInCircularMotion();
                break;

            case GameState.TUTORIAL3_PRELIMINARY:
                MoveHandVertically();
                break;

            case GameState.Start:
                StartGame();
                break;

            case GameState.Playing:
                GameObject[] Uninfected = GameObject
                                         .FindGameObjectsWithTag("NORMAL_BALL");
                GameObject[] Frozen = GameObject
                                         .FindGameObjectsWithTag("SAFE_BALL");
                GameObject[] Infected = GameObject
                                         .FindGameObjectsWithTag("Virus");
                int infectionRatio = (100 * Infected.Length)
                        / (Uninfected.Length + Infected.Length + Frozen.Length);

                if (previousTime - currentTime >= cureBallLifeTime) {
                    if (CureBallGameObject != null) {
                        Destroy(CureBallGameObject, 0);
                        CureBallGameObject = null;
                    }
                    currentTime = sec;
                    previousTime = sec;
                }

                if ((sec) % (cureBallRegenerateInterval) == 0
                    && shouldCureballRender
                    && CureBallGameObject == null
                ) {
                    shouldCureballRender = false;
                    RenderCureBall();
                    StartCoroutine(StartBallLate());

                    currentTime = sec;
                    previousTime = sec;
                } else if ((sec) % (cureBallRegenerateInterval) != 0) {
                    shouldCureballRender = true;
                    currentTime = sec;
                }

                EndGame(infectionRatio);
                break;

            case GameState.Over:
                frameCount++;

                if (frameCount > 300)
                {
                    CurrentGameState = GameState.Start;
                    Time.timeScale = 1;
                    Uninfected = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                    statusText.text = "Score:" + Uninfected.Length;
                    //SceneManager.LoadScene(1);
                }
                break;

            case GameState.Restart:
                CurrentGameState = GameState.Start;
                Time.timeScale = 1;
                break;

            default:
                break;
      }
    }

    private IEnumerator second()
    {
        yield return new WaitForSeconds(1f);
        if (sec > 0)
            sec--;
        timeText.text = makePrintableTime(sec);
        StartCoroutine(second());
    }

    private IEnumerator StartBallLate()
    {
        yield return new WaitForSeconds(1);
        DYNAMIC__cureBall.GetComponent<Ball>().StartBall();
    }

    private void CreateBallsRandomly()
    {
        System.Random random = new System.Random();
        int level = MainMenu.level;
        int naivelyCalculatedBallCount = 3 * (level % 100);

        for (int i = 0; i < naivelyCalculatedBallCount; i++)
        {
            float x = Virus.transform.position.x;
            float y = Virus.transform.position.y;
            while (Vector2.Distance(new Vector2(x, y),
                                    new Vector2(Virus.transform.position.x,
                                                Virus.transform.position.y))
                   < this.minDistance) {
                x = random.Next(minX, maxX);
                y = random.Next(minY, maxY);
            }
            GameObject ball = Instantiate(ballPrefab, new Vector3(x, y, 0), Quaternion.identity);

            ball.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ball.transform.gameObject.tag = "NORMAL_BALL";
        }
    }

    private void SetSceneForTutorial1()
    {
        tutorialLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        GameObject ball = Instantiate(ballPrefab, new Vector3(-2, 2, 0), Quaternion.identity);
        ball.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball.transform.gameObject.tag = "NORMAL_BALL";
        handObject = new GameObject("Hand");
        handObject.transform.position = new Vector3(transform.position.x, -0.58f);
        SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
        renderer.sprite = handSprite;
    }

    private void SetSceneForTutorial2()
    {
        tutorialLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        CreateCircularPoints();
        GameObject ball1 = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject ball2 = Instantiate(ballPrefab, new Vector3(-2, 2, 0), Quaternion.identity);
        GameObject ball3 = Instantiate(ballPrefab, new Vector3(-2, -2, 0), Quaternion.identity);

        ball1.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball1.transform.gameObject.tag = "NORMAL_BALL";
        ball2.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball2.transform.gameObject.tag = "NORMAL_BALL";
        ball3.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball3.transform.gameObject.tag = "NORMAL_BALL";
        handObject = new GameObject("Hand");
        SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
        renderer.sprite = handSprite;
    }

    private void SetSceneForTutorial3()
    {
        tutorialLine = Instantiate(linePrefab, Vector3.zero,
                                               Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(0, -2));
        lineRenderer.SetPosition(1, new Vector2(0, 2));
        GameObject ball1 = Instantiate(ballPrefab, new Vector3(2, 0, 0),
                                                   Quaternion.identity);
        GameObject ball2 = Instantiate(ballPrefab, new Vector3(-2, 2, 0),
                                                   Quaternion.identity);
        GameObject ball3 = Instantiate(ballPrefab, new Vector3(-2, -2, 0),
                                                   Quaternion.identity);

        ball1.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball1.transform.gameObject.tag = "NORMAL_BALL";
        ball2.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball2.transform.gameObject.tag = "NORMAL_BALL";
        ball3.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ball3.transform.gameObject.tag = "NORMAL_BALL";
        handObject = new GameObject("Hand");
        handObject.transform.position = new Vector3(-0.1f, -0.58f);
        SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
        renderer.sprite = handSprite;
        RenderCureBall();

        GameObject[] cbs = GameObject.FindGameObjectsWithTag("Cure");
        if (cbs.Length > 0) {
            cbs[0].transform.position = new Vector3(2, 2f, 0);
        }

        GameObject[] vbs = GameObject.FindGameObjectsWithTag("Virus");
        if (vbs.Length > 0) {
            vbs[0].transform.position = new Vector3(2, -2f, 0);
        }
    }

    private String makePrintableTime(int sec)
    {
        int minutes = sec / 60;
        int _sec = sec % 60;
        return minutes + " : " + _sec;
    }

    private void CreateCircularPoints()
    {
        float x;
        float y;
        float z;
        float xradius = 1;
        float yradius = 1;
        float angle = 20f;
        int segments = 50;
        lineRenderer.positionCount = 0;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(i, new Vector2(x, y));
            angle += (360f / segments);
        }
    }

    private void MoveHandInStraightLine()
    {
        handObject.transform.position = new Vector3(
                                       handObject.transform.position.x + 0.01f,
                                       handObject.transform.position.y);
        if (handObject.transform.position.x > 2.0f)
        {
            handObject.transform.position = new Vector3(-2.0f,
                                               handObject.transform.position.y);
        }
    }

    private void MoveHandVertically()
    {
        handObject.transform.position = new Vector3(
                                                handObject.transform.position.x,
                                                handObject.transform.position.y
                                                + 0.01f);
        if (handObject.transform.position.y > 2.0f)
        {
            handObject.transform.position = new Vector3(
                                       handObject.transform.position.x, -2.0f);
        }
    }

    private void MoveHandInCircularMotion()
    {
        degrees += 2;
        float rads = Mathf.PI * degrees / 180;
        handObject.transform.position = new Vector3(Mathf.Cos(rads),
                                               Mathf.Sin(rads) - 0.58f, -0.58f);
    }

    private void CollideCureBallWithInfectedBall()
    {

    }

    private void RenderCureBall()
    {
        System.Random random = new System.Random();
        int x = random.Next(minX, maxX);
        int y = random.Next(minY, maxY);
        CureBallGameObject = Instantiate(ballPrefab, new Vector3(x, y, 0),
                                                     Quaternion.identity);
        Ball cureball = CureBallGameObject.GetComponent<Ball>();
        SpriteRenderer currentSprite = CureBallGameObject
                                            .GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>("Sprites/cureball");
        currentSprite.sprite = sprite;
        CureBallGameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        CureBallGameObject.transform.gameObject.tag = "Cure";
        cureball.WARN__Initialize(BallType.CURE);
        DYNAMIC__cureBall = CureBallGameObject;
    }

    private void InitializeGameScene() {
        Debug.LogFormat(
            "GameManager.InitializeGameScene(): MainMenu.level: {0}",
            MainMenu.level);

        CDS = GameObject.Find("DelayedStart")
                        .GetComponent<DelayedStartScript>();
        int level = MainMenu.level;

        minX = -2;
        minY = -5;
        maxX = 2;
        maxY = 5;
        minDistance = 0.4f;
        sec = 30;
        expectedScore = level > 100 ? (level % 100) * 10 : level * 10;

        Virus = GameObject.Find("Virus");
        Virus.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        GameObject CurrentLine = DrawLine.CurrentLine;
        Physics2D.IgnoreCollision(CurrentLine.GetComponent<Collider2D>(),
                                  Virus.GetComponent<Collider2D>());

        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        timeText.text = makePrintableTime(sec);

        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;

        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "Expected Score: " + expectedScore;

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        GameLevel gameLevel = (GameLevel) level;
        switch (gameLevel) {
            case GameLevel.TUTORIAL_1:
                SetSceneForTutorial1();
                sec = 05;
                timeText = GameObject.Find("TimeText").GetComponent<Text>();
                timeText.text = makePrintableTime(sec);
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

            case GameLevel.TUTORIAL_2:
                SetSceneForTutorial2();
                sec = 10;
                timeText = GameObject.Find("TimeText").GetComponent<Text>();
                timeText.text = makePrintableTime(sec);
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

            case GameLevel.TUTORIAL_3:
                SetSceneForTutorial3();
                sec = 20;
                timeText = GameObject.Find("TimeText").GetComponent<Text>();
                timeText.text = makePrintableTime(sec);
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

            case GameLevel.NORMAL_1:
                CreateBallsRandomly();
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

            case GameLevel.NORMAL_2:
                CreateBallsRandomly();
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

            case GameLevel.NORMAL_3:
                CreateBallsRandomly();
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                break;

        default:
            break;
        }
    }

    private void StartGame() {
        GameLevel gameLevel = (GameLevel) MainMenu.level;

        if (CDS.counterDownDone == true) {
            switch (gameLevel) {
                case GameLevel.TUTORIAL_1:
                    CurrentGameState = GameState.TUTORIAL1_PRELIMINARY;
                    break;

                case GameLevel.TUTORIAL_2:
                    CurrentGameState = GameState.TUTORIAL2_PRELIMINARY;
                    break;

                case GameLevel.TUTORIAL_3:
                    CurrentGameState = GameState.TUTORIAL3_PRELIMINARY;
                    break;

                case GameLevel.NORMAL_1:
                case GameLevel.NORMAL_2:
                case GameLevel.NORMAL_3:
                default:
                    for (int i = 0; i < Cells.Length; i++)
                    {
                        Cells[i].GetComponent<Ball>().StartBall();
                    }
                    Virus.GetComponent<Ball>().StartBall();
                    CurrentGameState = GameState.Playing;
                    break;
            }

            StartCoroutine(second());
        }
    }

    private void EndGame(int infectionRatio) {
        GameLevel gameLevel = (GameLevel) MainMenu.level;

        switch (gameLevel) {
        case GameLevel.TUTORIAL_1:
            if (sec == 0) {
                    timeText.text = "Time's Up!";
                    StopCoroutine(second());
                    NextTutorial.gameObject.SetActive(true);
            }
            break;

        case GameLevel.TUTORIAL_2:
            if (sec == 0) {
                timeText.text = "Time's Up!";
                StopCoroutine(second());
                NextTutorial.gameObject.SetActive(true);
            }
            break;
        case GameLevel.TUTORIAL_3:
            if (sec == 0) {
                timeText.text = "Time's Up!";
                StopCoroutine(second());
                SceneManager.LoadScene((int) GameSceneId.MENU);
            }
            break;
        case GameLevel.NORMAL_1:
        case GameLevel.NORMAL_2:
        case GameLevel.NORMAL_3:
        default:
            if (infectionRatio >= INFECTION_RATIO_LIMIT) {
                StopCoroutine(second());
                Time.timeScale = 0;
                int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL")
                                        .Length
                            + GameObject.FindGameObjectsWithTag("SAFE_BALL")
                                        .Length;
                GameOverLose.gameObject.SetActive(true);
                scoreLose = GameObject.Find("ScoreLose")
                                        .GetComponent<Text>();
                scoreLose.text = "SCORE: " + Score * 10;
            } else if (sec == 0) {
                CurrentGameState = GameState.Over;
                timeText.text = "Time's Up!";
                StopCoroutine(second());
                Time.timeScale = 0;
                int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL")
                                        .Length
                            + GameObject.FindGameObjectsWithTag("SAFE_BALL")
                                        .Length;

                if (Score * 10 >= expectedScore) {
                    GameOverWin.gameObject.SetActive(true);
                    scoreLose = GameObject.Find("ScoreWin")
                                            .GetComponent<Text>();
                    scoreLose.text = "SCORE: " + Score * 10;
                } else {
                    GameOverLose.gameObject.SetActive(true);
                    scoreLose = GameObject.Find("ScoreLose")
                                            .GetComponent<Text>();
                    scoreLose.text = "SCORE: " + Score * 10;
                }
                statusText.enabled = true;
            }
            break;
        }
    }
}
