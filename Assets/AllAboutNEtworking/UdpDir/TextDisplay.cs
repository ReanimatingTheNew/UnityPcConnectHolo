using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour {

    public TextMesh tm;

    public void DisplayToTextMesh(string argSTR) {
        tm.text = argSTR;
    }
    
}
