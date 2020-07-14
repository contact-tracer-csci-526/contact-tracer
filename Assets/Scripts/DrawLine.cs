using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public static List<Ball> safeBalls;
    public static int MAX_SAFE_BALLS = 2;
    public static int MAX_SAFE_BALLS_FIXED = 2;
    public static GameObject CurrentLine;
    private static float RADIUS_COEFICIENT = 1.3f;

    public const float MAX_LENGTH = 6.0f;
    public const string LINE = "Line";
    public const int LINE_DURATION = 2;
    public const int MIN_ANGLE = 274;
    public LineRenderer drawLineRenderer;
    public GameObject linePrefab;
    public EdgeCollider2D edgeCollider2D;
    public List<Vector2> fingerPositions;
    public float angle;
    public bool isCircle;
    public float centroid_x;
    public float centroid_y;
    public float radius;

    private float lineLength;
    private DelayedStartScript CDS;
    private GameObject[] Cells;
    private String lineId;

    void Start()
    {
        lineLength = 0;
        angle = 0;
        isCircle = false;
        safeBalls = new List<Ball>();
        CurrentLine = Instantiate(linePrefab, Vector3.zero,
                                              Quaternion.identity);
        CurrentLine.transform.gameObject.tag = LINE;
        edgeCollider2D = CurrentLine.GetComponent<EdgeCollider2D>();
        drawLineRenderer = CurrentLine.GetComponent<LineRenderer>();
        drawLineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (CDS == null) {
            CDS = GameManager.CDS;
        }

        if (CDS != null && CDS.counterDownDone == true) {
            if (!CurrentLine) {
                lineLength = 0.0f;
                angle = 0;
                isCircle = false;
                CreateLine();
            }

            if (Input.GetMouseButtonDown(0)) {
                if (CurrentLine) {
                    drawLineRenderer.positionCount = 0;
                    fingerPositions.Clear();
                    edgeCollider2D.points = new []{ new Vector2(),
                                                    new Vector2() };
                    StopCoroutine(HideLine(lineId));
                    lineLength = 0.0f;
                    angle = 0;
                    isCircle = false;
                }
                CreateLine();
            }

            if (Input.GetMouseButton(0) && fingerPositions.Count > 0) {
                Vector2 tempFingerPosition = Camera.main.ScreenToWorldPoint(
                                                         Input.mousePosition);
                float newAddedLength = Vector2.Distance(tempFingerPosition,
                                    fingerPositions[fingerPositions.Count - 1]);
                lineLength += newAddedLength;

                if (lineLength > MAX_LENGTH) {
                    CheckCircle();
                    return;
                }

                if (newAddedLength > 0.08f) {
                    UpdateLine(tempFingerPosition);
                }
            }
            if (Input.GetMouseButtonUp(0)) {
                CheckCircle();
            }
        }
    }

    void CreateLine()
    {
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main
                                  .ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main
                                  .ScreenToWorldPoint(Input.mousePosition));

        drawLineRenderer.positionCount += 2;
        drawLineRenderer.SetPosition(0, fingerPositions[0]);
        drawLineRenderer.SetPosition(1, fingerPositions[1]);

        edgeCollider2D.points = fingerPositions.ToArray();
        String timestamp = System.DateTime.Now.ToString();
        lineId = timestamp;
        StartCoroutine(HideLine(timestamp));
    }

    void UpdateLine(Vector2 newFingerPosition)
    {
        fingerPositions.Add(newFingerPosition);

        drawLineRenderer.positionCount += 1;
        drawLineRenderer.SetPosition(drawLineRenderer.positionCount - 1,
                                     newFingerPosition);

        edgeCollider2D.points = fingerPositions.ToArray();

        if (GameManager.tutorialLine != null) {
            GameLevel gameLevel = (GameLevel) MainMenu.level;

            switch (gameLevel) {
            case GameLevel.TUTORIAL_1:
                Destroy(GameManager.tutorialLine);
                Destroy(GameManager.handObject);
                Cells =  GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                for (int i = 0; i < Cells.Length; i++) {
                    Cells[i].GetComponent<Ball>().StartBall();
                }
                GameManager.Virus.GetComponent<Ball>().StartBall();
                GameManager.CurrentGameState = GameState.Playing;
                break;

            case GameLevel.TUTORIAL_3:
                Destroy(GameManager.tutorialLine);
                Destroy(GameManager.handObject);
                Cells =  GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                for (int i = 0; i < Cells.Length; i++) {
                    Cells[i].GetComponent<Ball>().StartBall(0, -1);
                }
                GameManager.Virus.GetComponent<Ball>().StartBall(0, 1);
                GameManager.CurrentGameState = GameState.Playing;
                GameManager.DYNAMIC__cureBall.GetComponent<Ball>()
                                             .StartBall(-1, 0);
                break;

            default:
                break;
            }
        }
    }

    private void CheckCircle()
    {
        if (fingerPositions.Count <= 3) {
            isCircle = false;
            radius = 0;
        } else {
            centroid_x = 0;
            centroid_y = 0;
            radius = 0;
            for (int i = 0; i < fingerPositions.Count; i++) {
                centroid_x += fingerPositions[i].x;
                centroid_y += fingerPositions[i].y;
            }
            centroid_x /= fingerPositions.Count;
            centroid_y /= fingerPositions.Count;
            float angle_sum = 0;
            for (int i = 0; i < fingerPositions.Count - 1; i++) {
                float x1 = (float) fingerPositions[i].x - centroid_x;
                float y1 = (float) fingerPositions[i].y - centroid_y;
                float mag1 = Mathf.Sqrt(x1 * x1 + y1 * y1);
                float x2 = (float) fingerPositions[i+1].x - centroid_x;
                float y2 = (float) fingerPositions[i+1].y - centroid_y;
                float mag2 = Mathf.Sqrt(x2 * x2 + y2 * y2);
                float temp = Mathf.Acos(Mathf.Min(1,(x1 * x2 + y1 * y2)
                                             / (mag1 * mag2))) * Mathf.Rad2Deg;
                float distance = Mathf.Sqrt((fingerPositions[i].x - centroid_x)
                  * (fingerPositions[i].x - centroid_x)
                  + (fingerPositions[i].y - centroid_y)
                  * (fingerPositions[i].y - centroid_y));
                if (radius < distance) {
                    radius = distance;
                }
                angle_sum += temp;
            }
            if (angle_sum >= MIN_ANGLE) {
                isCircle = true;
                Ball enclosedBall = null;
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                float ballRadius = 0;

                if (Cells.Length > 0) {
                    ballRadius = Cells[0].GetComponent<CircleCollider2D>()
                                         .radius;
                }

                for (int i = 0; i < Cells.Length; i++) {
                    float ball_x = Cells[i].transform.position.x;
                    float ball_y = Cells[i].transform.position.y;
                    float distance = Mathf.Sqrt((ball_x - centroid_x)
                                              * (ball_x - centroid_x)
                                              + (ball_y - centroid_y)
                                              * (ball_y - centroid_y));
                    if (distance + ballRadius <= RADIUS_COEFICIENT * radius) {
                        enclosedBall = Cells[i].GetComponent<Ball>();
                        break;
                    }
                }

                if (enclosedBall != null) {
                    bool containsItem = false;
                    if (safeBalls != null && safeBalls.Count > 0){
                        containsItem = safeBalls.Contains(enclosedBall);
                    }

                    if (!containsItem) {
                        Destroy(enclosedBall.GetComponent<CircleCollider2D>());
                        safeBalls.Add(enclosedBall);
                        enclosedBall.ballBehavior.TransformsTo(BallType.SAFE);
                        if (MainMenu.level == (int) GameLevel.TUTORIAL_2
                            && GameManager.tutorialLine != null
                            && GameState.TUTORIAL2_PRELIMINARY.CompareTo(
                                           GameManager.CurrentGameState) == 0) {
                            Destroy(GameManager.tutorialLine);
                            Destroy(GameManager.handObject);
                            Cells =  GameObject
                                        .FindGameObjectsWithTag("NORMAL_BALL");

                            for (int i = 0; i < Cells.Length; i++) {
                                Cells[i].GetComponent<Ball>().StartBall();
                            }

                            GameManager.Virus.GetComponent<Ball>().StartBall();
                            GameManager.CurrentGameState = GameState.Playing;
                        }
                    }

                    if (safeBalls.Count > MAX_SAFE_BALLS) {
                        if (safeBalls.Count > 0) {
                            Ball safeBall = safeBalls[0];
                            safeBall.ballBehavior.TransformsTo(BallType.NORMAL);
                            safeBalls.RemoveAt(0);
                        }
                    }
                }
            } else {
                isCircle = false;
            }
        }
    }

    private IEnumerator HideLine(string _lineId)
    {
        yield return new WaitForSeconds(LINE_DURATION);

        if (transform.gameObject.tag == "Line" && lineId == _lineId) {
            drawLineRenderer.positionCount = 0;
            fingerPositions.Clear();
            edgeCollider2D.points = new []{ new Vector2(), new Vector2() };
        }
    }
}
