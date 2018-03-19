// @Author Nabil Lamriben ©2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings Instance = null;
    private void Awake()
    {

        if (Instance == null)
        {
            
            DontDestroyOnLoad(this.gameObject); 
            Instance = this;

        }
        else
            Destroy(gameObject);
    }


    #region Anchornames
    //ConsoleObject
    //public GameObject E_ConsoleObject;                   //obj.name=ConsoleObject_ES
    string AnchorName_ConsoleObject = "ARZConsoleObject";
    public string GetAnchorName_ConsoleObject() { return AnchorName_ConsoleObject; }

    //StemBase
    //public GameObject E_StemBase;                        //obj.name=StemBase_ES
    string AnchorName_StemBase = "ARZStemBase";
    public string GetAnchorName_StemBase() { return AnchorName_StemBase; }

    //MetalBarrel
    string AnchorName_MetalBarrel = "ARZMetalBarrel";
    public string GetAnchorName_MetalBarrel() { return AnchorName_MetalBarrel; }

    //MetalBarrel
    string AnchorName_RoomModel = "ARZRoomModel";
    public string GetAnchorName_RoomModel() { return AnchorName_RoomModel; }

    //SpawnPoint
    // public GameObject E_SpawnPoint;                     //obj.name=SpawnPoint_ES
    string AnchorName_SpawnPoint = "ARZSpawnPoint";
    public string GetAnchorName_SpawnPoint() { return AnchorName_SpawnPoint; }



    //SpawnPointDummy
    // public GameObject E_SpawnPointDummy;                     //obj.name=SpawnPointDummy_ES
    string AnchorName_SpawnPointDummy = "ARZSpawnPointDummy";
    public string GetAnchorName_SpawnPointDummy() { return AnchorName_SpawnPointDummy; }


    //Barrier
    //public GameObject E_Barrier;                         //obj.name=Barrier_ES
    string AnchorName_Barrier = "ARZBarrier";
    public string GetAnchorName_Barrier() { return AnchorName_Barrier; }


    //ScoreBord
    //public GameObject E_ScoreBoard;                     //obj.name=ScoreBoard_ES
    string AnchorName_ScoreBoard = "ARZScoreBoard";
    public string GetAnchorName_ScoreBoard() { return AnchorName_ScoreBoard; }


    //WeaponRack
    //public GameObject E_WeaponRack;                     //obj.name=WeaponRack_ES
    string AnchorName_WeaponRack = "ARZWeaponRack";
    public string GetAnchorName_WeaponRack() { return AnchorName_WeaponRack; }


    //PistoleMag
    //public GameObject E_PistoleMag;                     //obj.name=PistoleMag_ES
    string AnchorName_PistoleMag = "ARZPistoleMag";
    public string GetAnchorName_PistoleMag() { return AnchorName_PistoleMag; }


    //AmmoBox
    //public GameObject E_AmmoBox;                           //obj.name=AmmoBox_ES
    string AnchorName_AmmoBox = "ARZAmmoBox";
    public string GetAnchorName_AmmoBox() { return AnchorName_AmmoBox; }


    //AmmoBoxInfinite
    //public GameObject E_AmmoBoxInfinite;                 //obj.name=AmmoBoxInfinite_ES
    string AnchorName_AmmoBoxInfinite = "ARZAmmoBoxInfinite";
    public string GetAnchorName_AmmoBoxInfinite() { return AnchorName_AmmoBoxInfinite; }


    //PathFinder
    //public GameObject E_PathFinder;                    //obj.name=PathFinder_ES
    string AnchorName_PathFinder = "ARZPathFinder";
    public string GetAnchorName_PathFinder() { return AnchorName_PathFinder; }


    //WalkieTalkie
    //public GameObject E_WalkieTalkie;                    //obj.name=WalkieTalkie_ES
    string AnchorName_WalkieTalkie = "ARZWalkieTalkie";
    public string GetAnchorName_WalkieTalkie() { return AnchorName_WalkieTalkie; }


    //MistEmitter
    //public GameObject E_MistEmitter;                     //obj.name=MistEmitter_ES
    string AnchorName_MistEmitter = "ARZMistEmitter";
    public string GetAnchorName_MistEmitter() { return AnchorName_MistEmitter; }


    //mISTeND
    //public GameObject E_MistEnd;                        //obj.name=MistEnd_ES
    string AnchorName_MistEnd = "ARZMistEnd";
    public string GetAnchorName_MistEnd() { return AnchorName_MistEnd; }

    //HotSpot
    //public GameObject E_HotSpot;                        //obj.name=HotSpot_ES
    string AnchorName_HotSpot = "ARZHotSpot";
    public string GetAnchorName_HotSpot() { return AnchorName_HotSpot; }


    //AirStrikeStart
    //public GameObject E_AirStrikeStart;                        //obj.name=AirStrikeStart_ES
    string AnchorName_AirStrikeStart = "ARZAirStrikeStart";

    public string GetAnchorName_AirStrikeStart() { return AnchorName_AirStrikeStart; }

    //AirStrikeEnd
    //public GameObject E_AirStrikeEnd;                        //obj.name=AirStrikeEnd_ES
    string AnchorName_AirStrikeEnd = "ARZAirStrikeEnd";
    public string GetAnchorName_AirStrikeEnd() { return AnchorName_AirStrikeEnd; }

    string AnchorName_Target = "ARZTarget";
    public string GetAnchorName_Target() { return AnchorName_Target; }



    string AnchorName_ZombiePregame = "ARZZombiePregame";
    public string GetAnchorName_ZombiePregame() { return AnchorName_ZombiePregame; }


    string AnchorName_StartButton = "ARZStartButton";
    public string GetAnchorName_StartButton() { return AnchorName_StartButton; }


    #endregion



    #region UDPnetworking
       string _ip_Holo_01 = "192.168.1.201";
   // string _ip_Holo_01 = "10.0.0.201";
    public string GetIP_Holo_01() { return _ip_Holo_01; }

     string _ip_Holo_02 = "192.168.1.202";
   // string _ip_Holo_02 = "10.0.0.202";

    public string GetIP_Holo_02() { return _ip_Holo_02; }

    string _ip_MSI = "192.168.1.2";
    public string GetIP_MSI() { return _ip_MSI; }

    string _ip_Jul = "192.168.1.3";
    public string GetIP_JUL() { return _ip_Jul; }

    string _LowIp_ListenPort = "12345";
    public string GetPort_LowIp_Listen() { return _LowIp_ListenPort; }

    string _HighIp_ListenPort = "12346";
    public string GetPort_HighIp_Listen() { return _HighIp_ListenPort; }

    //********************************************************************************************************
    private string _my_ip;
    public string My_IP
    {
        get { return _my_ip; }
        set { _my_ip = value; }
    }

    private string _my_listen_port;
    public string My_ListeningEar_Port
    {
        get { return _my_listen_port; }
        set { _my_listen_port = value; }
    }



    private string _my_partner_ip;
    public string My_Partner_IP
    {
        get { return _my_partner_ip; }
        set { _my_partner_ip = value; }
    }

    private string _my_partner_listen_port;
    public string My_Partner_Listen_Port
    {
        get { return _my_partner_listen_port; }
        set { _my_partner_listen_port = value; }
    }
    //********************************************************************************************************

    public UDPmachine _ThisMyMachineType = UDPmachine.MSI_2;
 
    public UDPmachine ThisMyMachineType
    {
        get { return _ThisMyMachineType; }
        set { _ThisMyMachineType = value; }
    }
    #endregion



    public float targetwaitTime=35.0f;

    //waveManagerBeginNextWave
    public float  FirstBuffer= 6.0f ;

    // WavetimeinSeconds + 20

    public float FadeInStartIn = 1.0f;
    // then it takes 1 second to be completely alpha=1
    public float FadeOutStartIn = 1.0f;
    // then it takes 1 second to be completly alpha=0

    // wave completed , start Grapgics I II III IV  in 
    public float StartRomanIn = 3.2f;

    // waveManager  WaveCompleted_soPopANewOne ->waveManagerBeginNextWave
    public float NextBuffer = 6.0f;


    public float StartResetWaveIn = 6.0f;

    public float StartWaveAgain = 10.0f;


    public ARZGameModes _gameMode=ARZGameModes.GameRight;

    //the game mode for this hololens
    public ARZGameModes GameMode
    {
        get { return _gameMode; }
        set { _gameMode = value; }
    }



    //waveManagerBeginNextWave
    //float GlobalGameMasterTime ;
    private float _GlobalGameMasterTimemy;//240f;

    public float GlobalGameMasterTime
    {
        get { return _GlobalGameMasterTimemy; }
        set { _GlobalGameMasterTimemy = value; }
    }

    public bool IsBloodOn = true; 

    public bool IsGameLong = true;

    public bool IsHideBulletsPaths = true;

    public bool IsSecurityOn = true;

    public void Set_LongGameOn() { GlobalGameMasterTime = 240f; IsGameLong = true; }

    public void Set_ShortGame() { GlobalGameMasterTime = 120f; IsGameLong = false; }

    public bool IsStaticHitPointsON = false;

    public bool IsTestModeON = false;

    public bool IsRightHandedPlayer = true;

    public bool IsActiveReload = false;

    public bool IsAllowVibrate = false;

    public bool IsZombieRootMotionOn = false;

    public ARZReloadLevel ReloadDifficulty = ARZReloadLevel.EASY;

    void Start () {

         


    }
 
}
