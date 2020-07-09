using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
	public float INTERVAL=2;

    void Start()
    {
        if (transform.gameObject.tag == "Line")
        {
            Destroy(gameObject,INTERVAL);
        }
    }

    void Update()
    {

    }
}
