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
    private int infectionLimit = 100; //percent
    private int frameCount = 0;
    private int timeRemaining = 40; //sec
    public int score = 0;
    private int expectedScore = 20;

    Text statusText;
    Text timerText;
    Text scoreToPass;

    public static int level;
    public GameObject ballPrefab;
    public GameObject[] Uninfected;

    private int minX;
    private int minY;
    private int maxX;
    private int maxY;
    private float minDistance;

    void Start()
    {
        CDS = GameObject.Find("DelayedStart").GetComponent<DelayedStartScript>();
        level = MainMenu.level;
        this.minX = -3;
        this.minY = -6;
        this.maxX = 3;
        this.maxY = 6;
        this.minDistance = 0.4f;
        Virus = GameObject.Find("Virus");
        CureBall = GameObject.FindWithTag("Cure");

        Cells = new GameObject[3 * level];
        this.CreateBallsRandomly();
        Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.text = "" + timeRemaining;
        scoreToPass = GameObject.Find("ExpectedScore").GetComponent<Text>();
        scoreToPass.text = "" + expectedScore;
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
                    StartCoroutine(operateTimer());
                }
                break;
            case GameState.Playing:
                timerText.text = "" + timeRemaining;
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length);
                if (IR >= infectionLimit) {
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.enabled = true;
                } else if (timeRemaining == 0) {
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    Uninfected = GameObject.FindGameObjectsWithTag("NORMAL_BALL") + GameObject.FindGameObjectsWithTag("SAFE_BALL");
                    statusText.text = "Congrats!\n You survived! Score: " + Uninfected.Length + "\nExpected: " + expectedScore;
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

    IEnumerator operateTimer()
    {
      while (true) {
        yield return new WaitForSeconds(1);
        timeRemaining--;
      }
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
            ball.transform.gameObject.tag = "NORMAL_BALL";
        }
    }
}
