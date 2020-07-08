using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{	
	public float INTERVAL=2; 
    // Start is called before the first frame update
    void Start()
    {
        if (transform.gameObject.tag == "Line")
        {
            Destroy(gameObject,INTERVAL);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}