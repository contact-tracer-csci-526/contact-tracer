using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    // Start is called before the first frame update
    // following variable stores the prefab of our line object
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider2D;

    // following variable keeps track of the fingure position of the player
    public List<Vector2> fingerPositions;

    private float lineLength;

    public const float MAX_LENGTH = 6.0f;
    public const string LINE = "Line";
    public float angle;
    public bool isCircle;
    public float centroid_x;
    public float centroid_y;
    public float radius;
    public int MIN_ANGLE = 276;

    public static List<Ball> safeBalls;
    public static int MAX_SAFE_BALLS = 2;
    // this will be fixed for a level and when number of balls get increased we need to restore the dynamic threshold above back to this value
    public static int MAX_SAFE_BALLS_FIXED = 2;
    private static GameObject[] Cells;
    
     private DelayedStartScript CDS;
    void Start()
    {
        CDS = GameObject.Find("DelayedStart").GetComponent<DelayedStartScript>();
        lineLength = 0;
        angle = 0;
        isCircle = false;
        safeBalls = new List<Ball>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (CDS.counterDownDone == true)
        {
            if(!currentLine) {
            lineLength = 0.0f;
            angle = 0;
            isCircle = false;
            CreateLine();
             }
        // we will call the CreateLine function when the player has touched the screen 
        // the following condition checks for the left mouse button
                if(Input.GetMouseButtonDown(0)){
            // before creating the line we need to remove the previously added line
                    if (currentLine){
                Destroy (currentLine);
                lineLength = 0.0f;
                angle = 0;
                isCircle = false;
                 }
            CreateLine();
                }
        // this checks if we are continuously holding it down or not
        if(Input.GetMouseButton(0)){
            Vector2 tempFingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newAddedLength = Vector2.Distance(tempFingerPosition, fingerPositions[fingerPositions.Count - 1]);
            lineLength += newAddedLength;
            if (lineLength > MAX_LENGTH) {
                CheckCircle();
                return;
            }
            // we need to check if this finger position and the previous finger position is greater than a set buffer value
            if(newAddedLength > 0.08f)
            {
                UpdateLine(tempFingerPosition);
            }
        }
        if (Input.GetMouseButtonUp(0)){
            CheckCircle();
        }
        }
    }

    // the following function creates a new line and save that line to a different variable and 
    // it will set first two points of the line renderer and edge collider components
    void CreateLine()
    {
        // as soon as the user touches the screen we create a new game object from our line prefab
        // we also instantiate its corresponding edge collider and line renderer
        currentLine =  Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.transform.gameObject.tag = LINE;
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider2D = currentLine.GetComponent<EdgeCollider2D>();
        
        // we also clear the finger positions list containing the positions for previous line
        fingerPositions.Clear();
        
        // we need to add the coordinates of the screen where the user touched in the finger positions list
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // we add the same point because currently the start and end of the line is the same point
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        // now we set the first two positions of the line renderer component 
        lineRenderer.SetPosition(0,fingerPositions[0]);
        lineRenderer.SetPosition(1,fingerPositions[1]);

        // we also update the edge collider
        edgeCollider2D.points = fingerPositions.ToArray();
    }

    // the following function is used to add new points to the line as the user moves the fingers
    void UpdateLine(Vector2 newFingerPosition)
    {
        fingerPositions.Add(newFingerPosition);
        
        // we update the line renderer
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,newFingerPosition);
        
        // we also update the edge collider
        edgeCollider2D.points = fingerPositions.ToArray();

    }

    private void CheckCircle(){
        if (fingerPositions.Count <= 3){
            isCircle = false;
            radius = 0;
        }
        else
        {    // we first find the centroid from all the points
            centroid_x = 0;
            centroid_y = 0;
            radius = 0;
            for (int i = 0; i < fingerPositions.Count ; i++ ){
                centroid_x += fingerPositions[i].x;
                centroid_y += fingerPositions[i].y;
            }
            centroid_x /= fingerPositions.Count;
            centroid_y /= fingerPositions.Count;
            // now we calculate sum of all the angles from the center between adjacent pair of points
            float angle_sum = 0;
            for (int i = 0; i < fingerPositions.Count - 1 ; i++ ){
                float x1 = (float)fingerPositions[i].x - centroid_x;
                float y1 = (float)fingerPositions[i].y - centroid_y;
                float mag1 = Mathf.Sqrt(x1*x1 + y1*y1);
                float x2 = (float)fingerPositions[i+1].x - centroid_x;
                float y2 = (float)fingerPositions[i+1].y - centroid_y;
                float mag2 = Mathf.Sqrt(x2*x2 + y2*y2);
                //float temp = Mathf.Atan2((float)(y2 - y1),(float)(x2 - x1)) * Mathf.Rad2Deg;
                float temp = Mathf.Acos(Mathf.Min(1,(x1*x2 + y1*y2) / (mag1 * mag2))) * Mathf.Rad2Deg;
                float distance = Mathf.Sqrt((fingerPositions[i].x - centroid_x) * (fingerPositions[i].x - centroid_x) + (fingerPositions[i].y - centroid_y) * (fingerPositions[i].y - centroid_y));
                if (radius < distance){
                    radius = distance;
                }                
                angle_sum += temp;
            }

            if (angle_sum >= MIN_ANGLE){
                isCircle = true;
                // now we need to find a ball which is enclosed in the circle 
                Ball enclosedBall = null;
                Cells = GameObject.FindGameObjectsWithTag("NORMAL_BALL");
                for (int i = 0; i < Cells.Length; i++){
                    float ball_x = Cells[i].transform.position.x;
                    float ball_y = Cells[i].transform.position.y;
                    float distance = Mathf.Sqrt((ball_x - centroid_x) * (ball_x - centroid_x) + (ball_y - centroid_y) * (ball_y - centroid_y));
                    if (distance <= radius){
                        enclosedBall = Cells[i].GetComponent<Ball>();
                        break;
                    }
                }
                if (enclosedBall != null){
                    bool containsItem = false;
                    if (safeBalls != null && safeBalls.Count > 0){
                        containsItem = safeBalls.Contains(enclosedBall);
                    }
                    if (!containsItem)
                    {
                        safeBalls.Add(enclosedBall);
                        enclosedBall.ballBehavior.TransformsTo(BallType.SAFE);
                    }
                    if (safeBalls.Count > MAX_SAFE_BALLS){
                        if (safeBalls.Count > 0)
                        {
                            Ball safeBall = safeBalls[0];
                            safeBall.ballBehavior.TransformsTo(BallType.NORMAL);
                            safeBalls.RemoveAt(0);
                        }
                    }
                }
            }
            else {
                isCircle = false;
            }
        }
    }
}
