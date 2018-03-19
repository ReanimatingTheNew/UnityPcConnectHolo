using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsplat : Singleton<playsplat> {

    public AudioClip otherClip;
    public AudioClip otherClip2;
    AudioSource audio;

  
    void Start () {
        audio = GetComponent<AudioSource>();
        if (audio == null)
        {
          //  Debug.Log("no audosource");
            audio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

        }
        else
            Debug.Log("found audio source");
    }

    public void PlaySplatSound() {

        audio.clip = otherClip;
        audio.Play();
    }
 
}
