﻿using UnityEngine;
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

    private GameObject DYNAMIC__cureBall;

    private DelayedStartScript CDS;
    private Loading Loading;
    private int infectionLimit = 100; //percent
    private int frameCount = 0;
    //private int timeRemaining = 40; //sec
    public int score = 0;
    private int expectedScore = 20;

    Text statusText;
    Text scoreToPass;

    public static int level;
    public GameObject ballPrefab;
    public GameObject[] Uninfected;

    public GameObject GameOverWin;
    public GameObject GameOverLose;
    public Text scoreLose;
    public GameObject linePrefab;
    public static GameObject tutorialLine;
    private GameObject tutorialCircle;
    public static GameObject handObject;
    private LineRenderer lineRenderer;
    //private List<Vector2> circlePositions;
    private Vector3 lineStart = new Vector3(-2,0,0);
    private Vector3 lineEnd = new Vector3(2,0,0);
    public Sprite handSprite;

    private int minX;
    private int minY;
    private int maxX;
    private int maxY;
    private float minDistance;

    private int screenHeight;
    private int screenWidth;
    public Image loading;
    public Text timeText;
    public int minutes;
    public int sec;
    int totalSeconds = 0;
    int TOTAL_SECONDS = 0;
    public  GameObject CureBallGameObject;
    float fillamount;
    int degrees = 0;

    int cureBallLifeTime=5;
    int cureBallRegenerateInterval = 10;
    int currentTime=0;
    int previousTime=0;
    bool shouldCureballRender=true;

    void Start()
    {
        CDS = GameObject.Find("DelayedStart").GetComponent<DelayedStartScript>();
        //Loading = GameObject.Find("TIMER").GetComponent<Loading>();
        level = MainMenu.level;
        this.minX = -2;
        this.minY = -5;
        this.maxX = 2;
        this.maxY = 5;
        this.minDistance = 0.4f;
        Virus = GameObject.Find("Virus");
       // CureBall = GameObject.FindWithTag("Cure");
        minutes=0;
        sec= 40;
        //change the size of virus ball and cure ballPrefab
        Virus.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        // CureBall.transform.localScale=new Vector3(0.8f,0.8f,0.8f);
        if (level >= 3) {
            Cells = new GameObject[3 * level];
            this.CreateBallsRandomly();
            Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        }
        //change the size of virus ball
        Virus.transform.localScale = new Vector3(0.8f,0.8f,0.8f);

        Cells = new GameObject[3 * level];
         this.CreateBallsRandomly();
        Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        timeText = GameObject.Find("TimeText").GetComponent<Text>();

        timeText.text = minutes + " : " + sec;
         if (minutes > 0)
            totalSeconds += minutes * 60;
        if (sec > 0)
             totalSeconds += sec;
            TOTAL_SECONDS = totalSeconds;

        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "Expected Score: " + expectedScore;

        screenHeight=Screen.height;
        screenWidth=Screen.width;
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Tutorial1:
                lineRenderer.SetPosition(0,lineStart);
                lineRenderer.SetPosition(1,lineEnd);
                MoveHandInStraightLine();
                break;
            case GameState.Tutorial2:
                MoveHandInCircularMotion();
                break;
            case GameState.Tutorial3:
                MoveHandVertically();
                break;
            case GameState.Start:

                if (CDS.counterDownDone == true)
                {
                    switch (level){
                        case 1:
                            SetSceneForTutorial1();
                            CurrentGameState = GameState.Tutorial1;
                            // dottedLine = new DottedLine();
                            // dottedLine.DrawDottedLine(start, end);
                            break;
                        case 2:
                            SetSceneForTutorial2();
                            CurrentGameState = GameState.Tutorial2;
                            break;
                        case 3:
                            SetSceneForTutorial3();
                            CurrentGameState = GameState.Tutorial3;
                            break;
                        default:
                            for (int i = 0; i < Cells.Length; i++) {
                                Cells[i].GetComponent<Ball>().StartBall();
                                Debug.Log(i);
                            }
                            Virus.GetComponent<Ball>().StartBall();
                            CureBall.GetComponent<Ball>().StartBall();
                            CurrentGameState = GameState.Playing;
                            break;
                    }
                    Virus.GetComponent<Ball>().StartBall();
                    CurrentGameState = GameState.Playing;

                     StartCoroutine (second ());
                }

                break;
            case GameState.Playing:
                //timerText.text = "Time Remaining: " + timeRemaining;
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                GameObject[] Frozen = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length)
                         / (Uninfected.Length + Infected.Length + Frozen.Length);

                if(previousTime-currentTime>=cureBallLifeTime){
                    Debug.Log("destroy");
                    if(CureBallGameObject!=null){
                        Destroy(CureBallGameObject,0);
                        CureBallGameObject=null;
                    }
                    currentTime=minutes*60+sec;
                    previousTime=minutes*60+sec;
                }
                if ((minutes * 60 + sec) % (cureBallRegenerateInterval) == 0
                    && shouldCureballRender
                    && CureBallGameObject==null
                ) {
                    shouldCureballRender=false;
                    StartCoroutine(StartBallLate());
                    RenderCureBall();

                    currentTime=(minutes*60+sec);
                    previousTime=minutes*60+sec;
                }
                else if ((minutes*60+sec)%(cureBallRegenerateInterval)!=0){
                    shouldCureballRender=true;
                    currentTime=minutes*60+sec;

                }
                if (IR >= infectionLimit) {
                     StopCoroutine(second());
                    Time.timeScale = 0;
                    int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL").Length
                                + GameObject.FindGameObjectsWithTag("SAFE_BALL").Length;
                    GameOverLose.gameObject.SetActive(true);
                    scoreLose = GameObject.Find("ScoreLose").GetComponent<Text>();
                    scoreLose.text = "SCORE: " + Score * 10;

                } else if (sec == 0 && minutes == 0) {
                    CurrentGameState = GameState.Over;
                    timeText.text = "Time's Up!";
                    StopCoroutine (second ());
                    Time.timeScale = 0;
                    int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL").Length + GameObject.FindGameObjectsWithTag("SAFE_BALL").Length;
                    if(Score * 10 >= expectedScore)
                    {
                        statusText.text = "Congrats!\n You survived! Score: " + Score * 10 + "\nExpected: " + expectedScore;
                    } else
                    {
                        statusText.text = "Level failed! \n Your Score: " + Score * 10 + "\nExpected: " + expectedScore;
                    }
                    statusText.enabled = true;
                }
                break;

            case GameState.Over:
                frameCount++;

                if (frameCount > 300) {
                  CurrentGameState =  GameState.Start;
                  Time.timeScale = 1;
                    Uninfected = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                    statusText.text = "Score:" + Uninfected.Length;
                    SceneManager.LoadScene(1);
                }
                break;

             case GameState.Restart:

                 CurrentGameState =GameState.Start;
                 Time.timeScale = 1;
                break;

            default:
                break;
        }
    }

    public enum GameState
    {
        Start,
        Playing,
        Over,
        Restart,
        Tutorial1,
        Tutorial2,
        Tutorial3
    }

    IEnumerator second()
    {
    yield return new WaitForSeconds (1f);
    if(sec > 0)
    sec--;
    if (sec == 0 && minutes != 0) {
    sec = 60;
    minutes--;
    }
    timeText.text = minutes + " : " + sec;
    //fillLoading ();
    StartCoroutine (second ());
    }

    // void fillLoading()
    // {
    // totalSeconds--;
    // float fill = (float)totalSeconds/TOTAL_SECONDS;
    // loading.fillAmount = fill;
    // }


     IEnumerator StartBallLate()
     {
        yield return new WaitForSeconds(1);
        DYNAMIC__cureBall.GetComponent<Ball>().StartBall();
     }

    private void CreateBallsRandomly(){
        System.Random random = new System.Random();

        for(int i = 0; i < 3 * level; i++){
            float x = Virus.transform.position.x;
            float y = Virus.transform.position.y;
            while (Vector2.Distance(new Vector2(x,y),new Vector2(Virus.transform.position.x,Virus.transform.position.y)) < this.minDistance)
            {
                x = random.Next(minX,maxX);
                y = random.Next(minY,maxY);
            }
            GameObject ball = Instantiate(ballPrefab, new Vector3(x,y,0), Quaternion.identity);

            ball.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            ball.transform.gameObject.tag = "NORMAL_BALL";
        }
    }
    private void SetSceneForTutorial1()
    {
        tutorialLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        GameObject ball = Instantiate(ballPrefab, new Vector3(-2,2,0), Quaternion.identity);
        ball.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball.transform.gameObject.tag = "NORMAL_BALL";
        handObject = new GameObject("Hand");
        SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
        renderer.sprite = handSprite;
    }

    private void SetSceneForTutorial2()
    {
        tutorialLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        CreateCircularPoints();
        GameObject ball1 = Instantiate(ballPrefab, new Vector3(0,0,0), Quaternion.identity);
        GameObject ball2 = Instantiate(ballPrefab, new Vector3(-2,2,0), Quaternion.identity);
        GameObject ball3 = Instantiate(ballPrefab, new Vector3(-2,-2,0), Quaternion.identity);

        ball1.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball1.transform.gameObject.tag = "NORMAL_BALL";
        ball2.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball2.transform.gameObject.tag = "NORMAL_BALL";
        ball3.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball3.transform.gameObject.tag = "NORMAL_BALL";
        handObject = new GameObject("Hand");
        SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
        renderer.sprite = handSprite;
    }

    private void SetSceneForTutorial3()
    {
      tutorialLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
      lineRenderer = tutorialLine.GetComponent<LineRenderer>();
      lineRenderer.SetPosition(0, new Vector2(0, -2));
      lineRenderer.SetPosition(1, new Vector2(0, 2));
      GameObject ball1 = Instantiate(ballPrefab, new Vector3(2,0,0), Quaternion.identity);
      GameObject ball2 = Instantiate(ballPrefab, new Vector3(-2,2,0), Quaternion.identity);
      GameObject ball3 = Instantiate(ballPrefab, new Vector3(-2,-2,0), Quaternion.identity);

      ball1.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
      ball1.transform.gameObject.tag = "NORMAL_BALL";
      ball2.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
      ball2.transform.gameObject.tag = "NORMAL_BALL";
      ball3.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
      ball3.transform.gameObject.tag = "NORMAL_BALL";
      handObject = new GameObject("Hand");
      SpriteRenderer renderer = handObject.AddComponent<SpriteRenderer>();
      renderer.sprite = handSprite;

      //reposition virus and cure ball1
      GameObject cb = GameObject.FindGameObjectsWithTag("Cure")[0];
      cb.transform.position = new Vector3(2,1.5f,0);
      GameObject v = GameObject.FindGameObjectsWithTag("Virus")[0];
      v.transform.position = new Vector3(2,-1.5f,0);

      //velocity to ball 1, virus, and Cure
      // Rigidbody2D b1RB = ball1.GetComponent<Rigidbody2D>();
      // Vector2 newBall1direction = new Vector2(0,-1);
      // ball1.GetComponent<Ball>().rigidbody.velocity = ball1.GetComponent<Ball>().rigidbody.velocity * newBall1direction.normalized;
      // Rigidbody2D cbRB = cb.GetComponent<Rigidbody2D>();
      // Vector2 newCBdirection = new Vector2(-1,0);
      // cb.GetComponent<Ball>().rigidbody.velocity = cb.GetComponent<Ball>().rigidbody.velocity * newCBdirection.normalized;
      //Rigidbody2D vRB = v.GetComponent<Rigidbody2D>();
      // Vector2 newVirusdirection = new Vector2(0,1);
      // v.GetComponent<Ball>().rigidbody.velocity = v.GetComponent<Ball>().rigidbody.velocity * newVirusdirection.normalized;
    }

     void CreateCircularPoints ()
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
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            Debug.Log(x+" "+y);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition (i,new Vector2(x,y));
            angle += (360f / segments);
        }
    }
   private void MoveHandInStraightLine()
   {
       handObject.transform.position = new Vector3(handObject.transform.position.x + 0.005f,handObject.transform.position.y);
       if (handObject.transform.position.x > 2.0f)
       {
           handObject.transform.position = new Vector3(-2.0f,handObject.transform.position.y);
       }
   }

   private void MoveHandVertically()
   {
      handObject.transform.position = new Vector3(handObject.transform.position.x, handObject.transform.position.y + 0.005f);
      if (handObject.transform.position.y > 2.0f)
      {
        handObject.transform.position = new Vector3(handObject.transform.position.x, -2.0f);
      }
   }

   private void MoveHandInCircularMotion()
   {
      degrees++;
      float rads = Mathf.PI * degrees / 180;
      handObject.transform.position = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads));
   }
   private void CollideCureBallWithInfectedBall()
   {

   }

    private void RenderCureBall(){

            System.Random random = new System.Random();
            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);
            CureBallGameObject = Instantiate(ballPrefab, new Vector3(x,y,0), Quaternion.identity);
            Ball cureball = CureBallGameObject.GetComponent<Ball>();
            SpriteRenderer currentSprite = CureBallGameObject.GetComponent<SpriteRenderer>();

            Debug.Log("cureball "+CureBallGameObject.ToString());

            Sprite sprite = Resources.Load<Sprite>("Sprites/cureball");
            currentSprite.sprite = sprite;
            CureBallGameObject.transform.localScale=new Vector3(0.8f, 0.8f,0.8f);
            CureBallGameObject.transform.gameObject.tag = "Cure";

            cureball.WARN__Initialize(BallType.CURE);
            DYNAMIC__cureBall = CureBallGameObject;
    }
}
