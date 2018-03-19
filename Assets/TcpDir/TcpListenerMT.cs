using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpListenerMT : MonoBehaviour {

    TextMesh _tm;
    private void OnEnable()
    {
        _tm = GetComponentInChildren<TextMesh>();
        FakeGameManager.OtherPlayerStreakHandeler += JustDOTCP;
    }

    private void OnDisable()
    {
        FakeGameManager.OtherPlayerStreakHandeler -= JustDOTCP;
    }

    int x = 0;

    private void Start()
    {
        _tm.text += " \n initialized ";
    }
    void JustDOTCP()
    {
        x++;
        _tm.text = "i heard tcp it " + x;

    }
}
