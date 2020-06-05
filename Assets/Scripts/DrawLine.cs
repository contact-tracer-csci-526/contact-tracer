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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // we will call the CreateLine function when the player has touched the screen 
        // the following condition checks for the left mouse button
        if(Input.GetMouseButtonDown(0)){
            // before creating the line we need to remove the previously added line
            if (currentLine == true){
                Destroy (currentLine);
            }
            CreateLine();
        }
        // this checks if we are continuously holding it down or not
        if(Input.GetMouseButton(0)){
            Vector2 tempFingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // we need to check if this finger position and the previous finger position is greater than a set buffer value
            if(Vector2.Distance(tempFingerPosition, fingerPositions[fingerPositions.Count - 1]) > 0.1f)
            {
                UpdateLine(tempFingerPosition);
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
}
