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

    public static int level;
    public GameObject ballPrefab;
    public GameObject[] Uninfected;
    public GameObject[] Frozen;

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
    int goToMenu = 0;
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

        Cells = new GameObject[3 * level];
        this.CreateBallsRandomly();
        Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
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
           
        //timerText = GameObject.Find("Timer").GetComponent<Text>();
        //timerText.text = "Time Remaining: " + timeRemaining;
        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "Expected Score: " + expectedScore;

        screenHeight=Screen.height;
        screenWidth=Screen.width;

        StartCoroutine(sceneLoader());

        Debug.Log(screenWidth+" "+screenHeight);
    }

    IEnumerator sceneLoader() {
        yield return new WaitUntil(() => goToMenu == 1);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Start:
                if (CDS.counterDownDone == true)
                {
                    for (int i = 0; i < Cells.Length; i++) {
                        Cells[i].GetComponent<Ball>().StartBall();
                    }
                    Virus.GetComponent<Ball>().StartBall();
                    CureBall.GetComponent<Ball>().StartBall();
                    CurrentGameState = GameState.Playing;
                    //Loading.StartCoroutine (second ());
                    //StartCoroutine(operateTimer());
                     StartCoroutine (second ());
                    
                }
                break;
            case GameState.Playing:
                //timerText.text = "Time Remaining: " + timeRemaining;
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                GameObject[] Frozen = GameObject.FindGameObjectsWithTag("SAFE_BALL"); 
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length + Frozen.Length);
                if (IR >= infectionLimit) {
                    CurrentGameState = GameState.Over;
                    StopCoroutine (second ());
                    //StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.enabled = true;
                } else if (sec == 0 && minutes == 0) {
                    CurrentGameState = GameState.Start;
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
                    goToMenu = 1;
                    statusText.enabled = true;
                }
                break;

            case GameState.Over:
                frameCount++;

                if (frameCount > 300) {
                    CurrentGameState =  GameState.Start;
                    Time.timeScale = 0;
                    Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                    Frozen = GameObject.FindGameObjectsWithTag("SAFE_BALL"); 
                    int Score = Uninfected.Length + Frozen.Length;
                    statusText.text = "Score: " + Score*10;
                    goToMenu = 1;
                }
                break;

            default:
                break;
        }
    }

    public enum GameState
    {
        Start,
        Playing,
        Over
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
  fillLoading ();
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
}
