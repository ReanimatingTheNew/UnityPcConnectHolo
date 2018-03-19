using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpConnStringMNGR : MonoBehaviour {


    public TextMesh _myIpText;
    public TextMesh _myInternalListenPortText;
    public TextMesh _myPartenersIPText;
    public TextMesh _myPartenersListenPortText;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //to keep track of doubl click problem 
    int click_IP_cnt = 0;
    int click_intPrt_cnt = 0;
    int click_exIP_cnt = 0;
    int click_exPrt_cnt = 0;

    //to display and set udp sockets stuffs 
    public void Set_My_IP(string argstr)
    {
        click_IP_cnt++;
        if (GameSettings.Instance == null)
        {
            Debug.Log("!!!!!!!no gamesettings !!!!!!");
            _myIpText.text = "no settings";
        }
        else
        {
            Debug.Log("."+click_IP_cnt.ToString()+". gamesttings.My_IP = " + argstr);
            GameSettings.Instance.My_IP = argstr;
            _myIpText.text = GameSettings.Instance.My_IP + "   ." + click_IP_cnt.ToString() + ".";
        }       
    }

    public void Set_My_Listen_Port(string argstr)
    {
        click_intPrt_cnt++;
        if (GameSettings.Instance == null)
        {
            Debug.Log("!!!!!!!no gamesettings !!!!!!");
            _myInternalListenPortText.text = "no settings";
        }
        else
        {
            Debug.Log("." + click_intPrt_cnt.ToString() + ".gamesttings.My_ListenPort = " + argstr);
            GameSettings.Instance.My_ListeningEar_Port = argstr;
            _myInternalListenPortText.text = GameSettings.Instance.My_ListeningEar_Port+"    ." + click_intPrt_cnt.ToString() + ".";
        }
    }

    public void Set_My_Partner_IP(string argstr)
    {
        click_exIP_cnt++;
        if (GameSettings.Instance == null)
        {
            Debug.Log("!!!!!!!no gamesettings !!!!!!");
            _myPartenersIPText.text = "no settings";
        }
        else
        {
            Debug.Log("." + click_exIP_cnt.ToString() + ".gamesttings.My_Partner_IP = " + argstr);
            GameSettings.Instance.My_Partner_IP = argstr;
            _myPartenersIPText.text = GameSettings.Instance.My_Partner_IP+ "  ." + click_exIP_cnt.ToString() + ".";
        }
    }

    public void Set_My_Partner_Listen_Port(string argstr)
    {
        click_exPrt_cnt++;
        if (GameSettings.Instance == null)
        {
            Debug.Log("!!!!!!!no gamesettings !!!!!!");
            _myPartenersListenPortText.text = "no settings";
        }
        else
        {
            Debug.Log("." + click_exPrt_cnt.ToString() + ".gamesttings.My_Partner's Listen_Port = " + argstr);
            GameSettings.Instance.My_Partner_Listen_Port = argstr;
            _myPartenersListenPortText.text = GameSettings.Instance.My_Partner_Listen_Port + "  ." + click_exPrt_cnt.ToString() + ".";
        }
    }
}
