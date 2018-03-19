using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPResponse : MonoBehaviour
{


    public TextMesh tm = null;
  //  public FakeGameManager fgm;


    public void ResponseToUDPPacket(string incomingIP, string incomingPort, byte[] data)
    {
        string messageSTRreceived= System.Text.Encoding.UTF8.GetString(data);
        if (tm != null) {
            tm.text = messageSTRreceived;
            //worked with public reff
            FakeGameManager.Instance.Call_IHeardOtherPlayerStreakMax();
        }
        if (messageSTRreceived.Length > 2) {
            if (messageSTRreceived[0] == '#') {


            }else
                 if (messageSTRreceived[0] == '$')
            {


            }

        }

#if !UNITY_EDITOR

        //ECHO 
        //UDPCommunication comm = UDPCommunication.Instance;
       // comm.SendUDPMessage(incomingIP, comm.GetExternalPort(), data);

#endif
    }
}
