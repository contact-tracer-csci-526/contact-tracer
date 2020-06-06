using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCountDownScript : MonoBehaviour
{
    private DelayedStartScript CDS;
    
    public void SetCountDownNow()
    {
       CDS = GameObject.Find ("DelayedStart").GetComponent<DelayedStartScript> ();
        CDS.counterDownDone = true;
        
    }
    
}
