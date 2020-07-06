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
    private DelayedStartScript CDS;
    private Loading Loading;
    private int infectionLimit = 100; //percent
    private int frameCount = 0;
    //private int timeRemaining = 40; //sec
    public int score = 0;
    private int expectedScore = 20;

    Text statusText;
    Text scoreToPass;

    public Text scoreWin;

    public Text scoreLose;

    public static int level;
    public GameObject ballPrefab;
    public GameObject[] Uninfected;

    public GameObject GameOverWin;
    public GameObject GameOverLose;
    public GameObject linePrefab;
    public static GameObject tutorialLine;
    private GameObject tutorialCircle;
    public static GameObject handObject;
    private LineRenderer lineRenderer;
    private List<Vector2> linePositions;
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
    float fillamount;
    int degrees = 0;


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
        CureBall = GameObject.FindWithTag("Cure");
        minutes=0;
        sec= 40;
        //change the size of virus ball and cure ballPrefab
        Virus.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        CureBall.transform.localScale=new Vector3(0.8f,0.8f,0.8f);
        if(level >= 3){
            Cells = new GameObject[3 * level];
            this.CreateBallsRandomly();
            Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        }
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        //loading = GameObject.Find("fg").GetComponent<Image>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();

        timeText.text = minutes + " : " + sec;
         if (minutes > 0)
            totalSeconds += minutes * 60;
        if (sec > 0)
             totalSeconds += sec;
            TOTAL_SECONDS = totalSeconds;

        //timerText = GameObject.Find("Timer").GetComponent<Text>();
        //timerText.text = "Time Remaining: " + timeRemaining;
        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "EXPECTED SCORE: " + expectedScore;

        screenHeight=Screen.height;
        screenWidth=Screen.width;

        Debug.Log(screenWidth+" "+screenHeight);
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Tutorial1:
                lineRenderer.SetPosition(0,lineStart);
                lineRenderer.SetPosition(1,lineEnd);
                MoveHandInStraightLine();
                //MoveHandInCircularMotion();
                break;
            case GameState.Tutorial2:
                MoveHandInCircularMotion();
                break;
            case GameState.Tutorial3:
                CollideCureBallWithInfectedBall();
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
                            //drawCircle();
                            CurrentGameState = GameState.Tutorial2;
                            break;
                        // case 3:
                        //     CurrentGameState = GameState.Tutorial3;
                        //     break;
                        default:
                            for (int i = 0; i < Cells.Length; i++) {
                                Cells[i].GetComponent<Ball>().StartBall();
                            }
                            Virus.GetComponent<Ball>().StartBall();
                            CureBall.GetComponent<Ball>().StartBall();
                            CurrentGameState = GameState.Playing;
                            break;
                    }
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
                if (IR >= infectionLimit) {
                    //currentGameState = GameState.Over;
                    StopCoroutine(second());
                    Time.timeScale = 0;
                    //statusText.enabled = true;
                    int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL").Length
                                + GameObject.FindGameObjectsWithTag("SAFE_BALL").Length;
                     GameOverLose.gameObject.SetActive(true);
                        scoreLose = GameObject.Find("ScoreLose").GetComponent<Text>();
                        scoreLose.text = "SCORE: " + Score * 10;
                } else if (sec == 0 && minutes == 0) {
                    //currentGameState = GameState.Over;
                    timeText.text = "Time's Up!";
                    StopCoroutine(second());
                    Time.timeScale = 0;
                    int Score = GameObject.FindGameObjectsWithTag("NORMAL_BALL").Length
                                + GameObject.FindGameObjectsWithTag("SAFE_BALL").Length;
                    if (Score * 10 >= expectedScore) {
                        // statusText.text = "Congrats!\n You survived! Score: "
                        //                   + Score * 10
                        //                   + "\nExpected: "
                        //                   + expectedScore;
                        GameOverWin.gameObject.SetActive(true);
                        scoreWin = GameObject.Find("ScoreWin").GetComponent<Text>();
                        scoreWin.text = "SCORE: " + Score * 10;
                        //Debug.Log(scoreWin.text);

                    } else {
                        // statusText.text = "Level failed! \n Your Score: "
                        //                   + Score * 10
                        //                   + "\nExpected: "
                        //                   + expectedScore;
                        GameOverLose.gameObject.SetActive(true);
                        scoreLose = GameObject.Find("ScoreLose").GetComponent<Text>();
                        scoreLose.text = "SCORE: " + Score * 10;
                    }
                    //statusText.enabled = true;
                }
                break;

            case GameState.Over:
                frameCount++;

                if (frameCount > 300) {
                  CurrentGameState =  GameState.Start;
                  Time.timeScale = 1;
                    // Uninfected = GameObject.FindGameObjectsWithTag("SAFE_BALL");
                    // statusText.text = "Score:" + Uninfected.Length;
                    // SceneManager.LoadScene(1);
                    GameOverLose.gameObject.SetActive(true);
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

 void fillLoading()
 {
  totalSeconds--;
  float fill = (float)totalSeconds/TOTAL_SECONDS;
  loading.fillAmount = fill;
 }

    private void CreateBallsRandomly(){
        System.Random random = new System.Random();

        for(int i = 0; i < 3 * level; i++){
            float x = Virus.transform.position.x;
            float y = Virus.transform.position.y;
         //   ball.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            while (Vector2.Distance(new Vector2(x,y),new Vector2(Virus.transform.position.x,Virus.transform.position.y)) < this.minDistance)
            {
                x = random.Next(minX,maxX);
                y = random.Next(minY,maxY);
            }
            GameObject ball = Instantiate(ballPrefab, new Vector3(x,y,0), Quaternion.identity);

            //found this piece of code online

            ball.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            ball.transform.gameObject.tag = "NORMAL_BALL";
        }
    }
    private void SetSceneForTutorial1()
    {
        tutorialLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        GameObject ball = Instantiate(ballPrefab, new Vector3(-2,2,0), Quaternion.identity);
        //found this piece of code online
        ball.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball.transform.gameObject.tag = "NORMAL_BALL";
        handObject = Instantiate(ballPrefab, new Vector3(-2.0f,-0.5f), Quaternion.identity);
        handObject.GetComponent<SpriteRenderer>().sprite = handSprite;
    }
    private void SetSceneForTutorial2()
    {
        tutorialLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = tutorialLine.GetComponent<LineRenderer>();
        GameObject ball1 = Instantiate(ballPrefab, new Vector3(0,0,0), Quaternion.identity);
        GameObject ball2 = Instantiate(ballPrefab, new Vector3(-2,2,0), Quaternion.identity);
        GameObject ball3 = Instantiate(ballPrefab, new Vector3(-2,-2,0), Quaternion.identity);


        //found this piece of code online
        ball1.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball1.transform.gameObject.tag = "NORMAL_BALL";
        ball2.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball2.transform.gameObject.tag = "NORMAL_BALL";
        ball3.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        ball3.transform.gameObject.tag = "NORMAL_BALL";
        handObject = Instantiate(ballPrefab, new Vector3(-1,-1), Quaternion.identity);
        handObject.GetComponent<SpriteRenderer>().sprite = handSprite;
    }
   private void MoveHandInStraightLine()
   {
       handObject.transform.position = new Vector3(handObject.transform.position.x + 0.005f,handObject.transform.position.y);
       if (handObject.transform.position.x > 2.0f)
       {
           handObject.transform.position = new Vector3(-2.0f,handObject.transform.position.y);
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
   // private void drawCircle()
   // {
   //
   // }
}
