using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MuteScript : MonoBehaviour
{
    public Sprite musicOnImage;
    public Sprite musicOffImage;
    public GameObject MuteButton;
    public bool isMute;

    void Start() {
        MuteButton = GameObject.Find("MuteButton");
    }

    public void Mute () {
        isMute = !isMute;
        AudioListener.volume = isMute ? 0 : 1;

        if (isMute) {
            MuteButton.GetComponent<Image>().sprite = musicOffImage;
        } else {
            MuteButton.GetComponent<Image>().sprite = musicOnImage;
        }
    }
}
