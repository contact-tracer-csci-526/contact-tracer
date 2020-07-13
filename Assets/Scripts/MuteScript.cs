using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MuteScript : MonoBehaviour
{
  
     bool isMute;
    public Sprite musicOnImage;
    public Sprite musicOffImage;
    GameObject MuteButton;
 
    void Start() {
             MuteButton = GameObject.Find("MuteButton"); 
        }

    public void Mute (){
         isMute = ! isMute;
        AudioListener.volume =  isMute ? 0 : 1;
    
            if(isMute)
            {
             MuteButton.GetComponent<Image>().sprite = musicOffImage;

            }

            else
            {
            MuteButton.GetComponent<Image>().sprite = musicOnImage;
             }
        }
}
