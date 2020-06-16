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

     private int timeRemaining = 45; //sec


    Text statusText;
    Text timerText;

    public static int level;
    public GameObject ballPrefab;

    private int minX;
    private int minY;
    private int maxX;
    private int maxY;
    private float minDistance;
    // Use this for initialization
    void Start()
    {
        CDS = GameObject.Find ("DelayedStart").GetComponent<DelayedStartScript> ();
        
        level = MainMenu.level;
        // Debug.Log(level);
        this.minX = -3;
        this.minY = -6;
        this.maxX = 3;
        this.maxY = 6;
        this.minDistance = 0.4f;
        Virus = GameObject.Find("Virus");
        //currently I am declaring the cure ball statically here
        CureBall = GameObject.Find("Cure");

        Cells = new GameObject[3 * level];
        this.CreateBallsRandomly();
        Cells = GameObject.FindGameObjectsWithTag("Cell");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.text = "" + timeRemaining;
    }


//    private bool InputTaken()
//    {
//        return Input.touchCount > 0 || Input.GetMouseButtonUp(0);
//    }


    // Update is called once per frame
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
                    //CureBall.GetComponent<Ball>().StartBall();
                    CurrentGameState = GameState.Playing;
                    StartCoroutine(operateTimer());
                }
                break;
            case GameState.Playing:
                timerText.text = "" + timeRemaining;
                //calc infection rate
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("Cell");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length);
                if (IR >= infectionLimit) { //end condition
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.enabled = true;
                    //save time/score
                } else if (timeRemaining == 0) { //win condition
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.text = "Congrats!\n You survived!";
                    statusText.enabled = true;
                }
                break;

            case GameState.Over:
                //display "Game Over: Infection rate exceeded" (flash on every 10th frame?)
                //display score/time-survived
                frameCount++;

                //return to start scene (wait on button input for direction to switch out of Game over state)
                if (frameCount > 300) {
                  //StartCoroutine(ToSplash());
                  //print("transition started");
                  CurrentGameState =  GameState.Start;
                  Time.timeScale = 1;
                  SceneManager.LoadScene(1);
                  //transitionStarted = true;
                  //Time.timeScale = 1;
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

    // IEnumerator ToSplash()
    //  {
    //      //yield return new WaitForSeconds(5);
    //      SceneManager.LoadScene(0);
    //  }

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
            // we will check if the new cell is closer to the virus we will create new coordinates for it 
            while (Vector2.Distance(new Vector2(x,y),new Vector2(Virus.transform.position.x,Virus.transform.position.y)) < this.minDistance)
            {
                x = random.Next(minX,maxX);
                y = random.Next(minY,maxY);
            }
            GameObject ball = Instantiate(ballPrefab, new Vector3(x,y,0), Quaternion.identity);
            ball.transform.gameObject.tag = "Cell";
        }
    }

}
