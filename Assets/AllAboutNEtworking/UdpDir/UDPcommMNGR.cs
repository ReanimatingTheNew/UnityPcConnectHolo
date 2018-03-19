using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UDPcommMNGR : Singleton<UDPcommMNGR> {

    public UDPCommunication UdpCommunicator;

    int MessageSentID = -1;
    public void HelpSendMEssage(string argMessage) {
        MessageSentID++;
        UdpCommunicator.SendUDPMessage(UdpCommunicator.GetExternalIP(), UdpCommunicator.GetExternalPort(), Encoding.UTF8.GetBytes("#" + MessageSentID.ToString()+"#" + argMessage));

    }

}
