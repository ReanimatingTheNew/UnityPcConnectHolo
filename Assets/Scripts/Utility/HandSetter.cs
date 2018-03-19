using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSetter : MonoBehaviour {

    public Text text;

  
     
	// Use this for initialization
	void Start () {

        DisplayText();


    }
    void DisplayText()
    {
        if (GameSettings.Instance.IsRightHandedPlayer)
        {
            text.text = "RIGHT handed";
        }

        else
        {
            text.text = "Lefty";
        }
    }

    public void ToggleHand()
    {
        GameSettings.Instance.IsRightHandedPlayer = !GameSettings.Instance.IsRightHandedPlayer;
        DisplayText();
    }
}
    