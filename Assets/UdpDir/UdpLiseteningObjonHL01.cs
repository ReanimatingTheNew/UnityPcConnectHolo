
 

    // @Author Nabil Lamriben ©2018
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Linq;
using MixedRealityNetworking;

public class UdpLiseteningObjonHL01 : MonoBehaviour
{

    // Use this for initialization
    TextDisplay TextDisplayer;
    public TextMesh textMeshBox;
#if !UNITY_EDITOR
    Action<NetworkMessage> Showit;
#endif
    byte _thIdtolistento;
    void Awake()
    {
        int x = 123;
        _thIdtolistento = Convert.ToByte(x);

#if !UNITY_EDITOR
        Showit=DotheShow;
        SocketClientManager.Port = 12346;
        SocketClientManager.Host = "192.168.1.8";
        SocketClientManager.Connect();
        SocketClientManager.Subscribe(_thIdtolistento, DotheShow);
#endif
    }
    private void OnEnable()
    {
        TextDisplayer = GetComponent<TextDisplay>();

    }
    string mesg = "";
#if !UNITY_EDITOR

    void DotheShow(NetworkMessage argMessage) {

        //TextDisplayer.DisplayToTextMesh(argMessage.Content.ToString());
      //  TextDisplayer.DisplayToTextMesh("dddd");
        mesg=System.Text.Encoding.UTF8.GetString(argMessage.Content);
      //  if (textMeshBox != null)
        //    textMeshBox.text = "dddd";// System.Text.Encoding.UTF8.GetString(argMessage.Content);
    }
#endif
    // Update is called once per frame
    void Update()
    {

        textMeshBox.text = mesg;
    }
}
