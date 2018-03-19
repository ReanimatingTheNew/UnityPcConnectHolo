// @Author Jeffrey M. Paquette ©2016
// @Author Nabil Lamriben ©2017
//todonabil , decide if we want a startreset on the stem . right now we just instanticate it in gamenanger lol its sooo effing huge ... gotta  break it up at some point

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemOrientation : MonoBehaviour {

    public GameObject[] lefthand;
    public GameObject[] rightHand;
   // public GameObject startResetLocal;
	// Use this for initialization
	private void Awake () {
	    if (PlayerPrefs.HasKey("hand"))
        {
            if (PlayerPrefs.GetInt("hand") == 1)
            {
                foreach(GameObject o in rightHand)
                {
                    o.SetActive(false);
                }
                foreach(GameObject o in lefthand)
                {
                    o.SetActive(true);
                }
                return;
            }
        }

        // else
        foreach (GameObject o in rightHand)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in lefthand)
        {
            o.SetActive(false);
        }

    }
	
}
