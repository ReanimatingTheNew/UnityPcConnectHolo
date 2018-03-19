// @Author Nabil Lamriben ©2018
using UnityEngine;

public class UdpListenerMT : MonoBehaviour {


    TextMesh _tm;
    private void OnEnable()
    {
        _tm = GetComponentInChildren<TextMesh>();
        FakeGameManager.OtherPlayerStreakHandeler += JustDO;
    }

    private void OnDisable()
    {
        FakeGameManager.OtherPlayerStreakHandeler -= JustDO;
    }

    int x = 0;

    private void Start()
    {
        _tm.text += " \n initialized ";
    }
    void JustDO()
    {
        x++;
        _tm.text = "i heard it " + x;

    }
}
