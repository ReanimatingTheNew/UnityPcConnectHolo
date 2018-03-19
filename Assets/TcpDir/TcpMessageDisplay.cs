using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpMessageDisplay : MonoBehaviour {

    public TextMesh messageTextMesh;

    public void DisplayReceivedMessage(string argStr) { messageTextMesh.text += "\n"+argStr; }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
