// @Author Nabil Lamriben ©2018
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine.Events;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

[System.Serializable]
public class UDPMessageEvent : UnityEvent<string, string, byte[]>
{

}

public class UDPCommunication : MonoBehaviour
{
    //show to screen 
     public TextMesh MyInfo;
    private string internalPort = "na";
    private string externalIP = "na";
    private string externalPort = "na";
    private UDPmachine MyMachineType;
    private string MeTheHostMyName = "na";


    UDPInitSctuct _MyUdpTructure;

    public string GetExternalPort() { return externalPort; }
    public string GetExternalIP() { return externalIP; }

    [Tooltip("Conten of Ping")]
    public string PingMessage = "cliked ";

    [Tooltip("Function to invoke at incoming packet")]
    public UDPMessageEvent udpEvent = null;

    private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();



    //***********************************************************************************************************
    //   
    //
    //  
    //
    //***********************************************************************************************************




    void CalcMyMachineType() {
        //msipc
        //LAPTOP-SS5QVBK2
        //Holo-01
        //Holo-02
        Debug.Log("my host name is " + MeTheHostMyName);
        switch (MeTheHostMyName)
        {
            case "msipc":
                MyMachineType = UDPmachine.MSI_2;//bad
                break;
            case "LAPTOP-SS5QVBK2":
                MyMachineType = UDPmachine.Jalt_3;
                break;
            case "Holo-01":
                MyMachineType = UDPmachine.Holo_01;
                break;
            case "Holo-02":
                MyMachineType = UDPmachine.Holo_02;
                break;
            default:
                MyMachineType = UDPmachine.MSI_2;
                break;
        }
     
    }




    void BuildMyStructureNew()
    {
        Init_myInfo_and_Displayit();
    }

    void Init_myInfo_and_Displayit() {
        string _info = " I am using a ";
        if (GameSettings.Instance == null)
        {
            Debug.Log("no settings");
            _info = " i haz no settings !!! Will FAAAILLL ";
        }
        else

        {
            _info += MyMachineType.ToString() + " my ip should be "+ GameSettings.Instance.My_IP ;
            _info += " <-  my oponent is @--> " + GameSettings.Instance.My_Partner_IP;
            _info += " \n  my opponent is listenning on" + GameSettings.Instance.My_Partner_Listen_Port;
            _info += " \n  ----------------------------------- ";
            _info += " \n  I listen on port " + GameSettings.Instance.My_ListeningEar_Port ;
            _info += " \n  ----------------------------------- ";


            //msipc LAPTOP-SS5QVBK2  Holo-01  Holo-02
            string hostName = "na";
#if !UNITY_EDITOR

        var hostNames = NetworkInformation.GetHostNames();
          hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "???";
#endif

            externalIP = GameSettings.Instance.My_Partner_IP;
            externalPort = GameSettings.Instance.My_Partner_Listen_Port;
            internalPort= GameSettings.Instance.My_ListeningEar_Port;
            MeTheHostMyName = hostName;

        }
        MyInfo.text = _info;
    }

 
 

   

    private void Awake()
    {
        //BuildMyStructureNew();
    }

#if !UNITY_EDITOR

      private void OnEnable()
    {

        if (udpEvent == null)
        {
            udpEvent = new UDPMessageEvent();
            udpEvent.AddListener(UDPMessageReceived);
        }
    }

    private void OnDisable()
    {
        if (udpEvent != null)
        {
            
            udpEvent.RemoveAllListeners();
        }
    }



    //we've got a message (data[]) from (host) in case of not assigned an event
    void UDPMessageReceived(string host, string port, byte[] data)
    {
        Debug.Log("GOT MESSAGE FROM: " + host + " on port " + port + " " + data.Length.ToString() + " bytes ");
     Console3D.Instance.LOGit("GOT MESSAGE FROM: " + host + " on port " + port + " " + data.Length.ToString() + " bytes ");
    }

    //Send an UDP-Packet
    public async void SendUDPMessage(string HostIP, string HostPort, byte[] data)
    {
        await _SendUDPMessage(HostIP, HostPort, data);
    }



    DatagramSocket socket;

    async void Start()
    {
        BuildMyStructureNew();



        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        HostName IP = null;
        try
        {



            //last or default .. list is Holo-01, Holo-01 local, some weird ipv6, 192.168.1.201

            IP = NetworkInformation.GetHostNames().LastOrDefault(h =>
                    h.IPInformation != null &&
                    h.IPInformation.NetworkAdapter != null);

            Console3D.Instance.LOGit("-------------" + NetworkInformation.GetHostNames().Count);
            foreach (HostName hn in NetworkInformation.GetHostNames()) {
                Console3D.Instance.LOGit("fe netadaptor id = " + hn.RawName.ToString());
            }

            Console3D.Instance.LOGit("-------------");
            string ipAddress = IP.RawName; //XXX.XXX.XXX.XXX

            Console3D.Instance.LOGit("fe IPraw  = " + ipAddress);





            // var icp = NetworkInformation.GetInternetConnectionProfile();

            //Console3D.Instance.LOGit("icp.netadaptor id = " + icp.NetworkAdapter.NetworkAdapterId.ToString());

            //   IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            //   .FirstOrDefault(
            //       hn =>
            //           hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
            //           == icp.NetworkAdapter.NetworkAdapterId);
           // Console3D.Instance.LOGit(" my socket is = " + IP.ToString() + " or is it "+ ipAddress+"  p-> " + internalPort);
            await socket.BindEndpointAsync(IP, internalPort);




            //Windows.Networking.HostName serverHost = new Windows.Networking.HostName(MeTheHostMyName);

            //Console3D.Instance.LOGit("not even using gamesettings -> my socket is = " + serverHost.ToString() + " " + internalPort);

            //await socket.BindEndpointAsync(serverHost, internalPort);
        }
        catch (Exception e)
        {
            Debug.Log(" udpconn yo error1"+ e.ToString());
            Debug.Log(" udpconn yo error2"+ SocketError.GetStatus(e.HResult).ToString());
            return;
        }
        //SendUDPMessage(externalIP, externalPort, Encoding.UTF8.GetBytes(PingMessage));
    }




    private async System.Threading.Tasks.Task _SendUDPMessage(string argexternalIP, string argexternalPort, byte[] data)
    {
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(argexternalIP), argexternalPort))
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(stream))
            {
                writer.WriteBytes(data);
                await writer.StoreAsync();

            }
        }
    }


#else


    // to make Unity-Editor happy :-)
    void Start()
    {

    }

    public void SendUDPMessage(string HostIP, string HostPort, byte[] data)
    {

    }

#endif


    static MemoryStream ToMemoryStream(Stream input)
    {
        try
        {                                         // Read and write in
            byte[] block = new byte[0x1000];       // blocks of 4K. 1000
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                int bytesRead = input.Read(block, 0, block.Length);
                if (bytesRead == 0) return ms;
                ms.Write(block, 0, bytesRead);
            }
        }
        finally { }
    }

    // Update is called once per frame
    void Update()
    {
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();

        }
    }

#if !UNITY_EDITOR
    private void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
        Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
    {
     //   Console3D.Instance.LOGit("GOT MESSAGE FROM: " + args.RemoteAddress.DisplayName);
        //Read the message that was received from the UDP  client.
        Stream streamIn = args.GetDataStream().AsStreamForRead();
        MemoryStream ms = ToMemoryStream(streamIn);
        byte[] msgData = ms.ToArray();


        if (ExecuteOnMainThread.Count == 0)
        {
            ExecuteOnMainThread.Enqueue(() =>
            {
          //      Console3D.Instance.LOGit("ENQEUED ");
                if (udpEvent != null)
                    udpEvent.Invoke(args.RemoteAddress.DisplayName, internalPort, msgData);
            });
        }
    }


#endif
}
