// @Author Nabil Lamriben ©2018

using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class UdpRootConsole : MonoBehaviour
{

    public TextMesh tm;


    void Start()
    {
    }



    public void DoSendMessage_1()
    {
        tm.text += "\n Sending message 1";
        UDPcommMNGR.Instance.HelpSendMEssage("msg 1");

    }
    public void DoSendMessage_2()
    {
        tm.text += "\n Sending message 2";
        UDPcommMNGR.Instance.HelpSendMEssage("msg 2");
    }
    public void CallScreen(object o)
    {
        if (o is string)
        {
            if (o.ToString() == "Button_A_Clicked") { DoSendMessage_1(); }
            else
                 if (o.ToString() == "Button_B_Clicked") { DoSendMessage_2(); }
        }
    }
}



