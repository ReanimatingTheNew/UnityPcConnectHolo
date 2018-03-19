// @Author Nabil LAamriben ©2017

using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class GlobalSettings : MonoBehaviour {

    TextMesh tm;
    public static GlobalSettings Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            InitDefaultValues();
            DontDestroyOnLoad(this.gameObject);
            Instance = this;

        }
        else
            Destroy(gameObject);
    }

 
    void InitDefaultValues() {
        Debug.Log("iniiting globals");
        _isVisibleGridPoints = false;

        _segmentDistance = 0.5f;
        _isInfinitAmmo = false;
        _gameDuration = 300;
        _isPowerUp = false;

        tm = GetComponentInChildren<TextMesh>();
        tm.text = "inited";
    }

    public void ListenSetSegDist(int argSegDist) {
        tm.text += "/n setdist to "+argSegDist;
        switch (argSegDist)
        {
            case 0:
                SegmentDistance = 0.25f;
                break;
            case 1:
                SegmentDistance = 0.35f;
                break;
            case 2:
                SegmentDistance = 0.4f;
                break;
            case 3:
                SegmentDistance = 0.5f;
                break;
            case 4:
                SegmentDistance = 1.0f;
                break;
            case 5:
                SegmentDistance = 1.25f;
                break;
            default:
                {
                    SegmentDistance = .25f;
                    break;
                }

        }
    }
    public void ListenSetVisGrid(int argIntforBool) {
        tm.text += "/n gridviz to " + argIntforBool;
        Debug.Log("SETGRIDD VIZ");
        if (argIntforBool == 1) IsVisibleGridPoints = true; else IsVisibleGridPoints = false;
    }
    public void ListenSetInfAmo(int argIntforBool) { if (argIntforBool == 1) IsInfinitAmmo = true; else IsInfinitAmmo = false; }
    public void ListenSetGameDur(int argGameDur) { GameDuration = argGameDur; }    
    public void ListenSetPowerup(int argIntforBool) { if (argIntforBool == 1) IsPowerUp = true; else IsPowerUp = false; }

    

    #region GeneralSettings
    private float _segmentDistance;
    public float SegmentDistance
    {
        get { return _segmentDistance; }
        set { _segmentDistance = value; }
    }

    private bool _isVisibleGridPoints;
    public bool IsVisibleGridPoints
    {
        get { return _isVisibleGridPoints; }
        set { _isVisibleGridPoints = value; }
    }

    private bool _isInfinitAmmo;
    public bool IsInfinitAmmo
    {
        get { return _isInfinitAmmo; }
        set { _isInfinitAmmo = value; }
    }

    private int _gameDuration;
    public int GameDuration
    {
        get { return _gameDuration; }
        set { _gameDuration = value; }
    }

    private bool _isPowerUp;

    public bool IsPowerUp
    {
        get { return _isPowerUp; }
        set { _isPowerUp = value; }
    }

    #endregion



    #region PublicFuncs

    public void SetSegmentDistance(float argSegDist) {
        _segmentDistance = argSegDist;
    }

    public void SetTestMode() { }

    #endregion

    #region AnchorPoint
    //public GameObject E_ConsoleObject;                   //obj.name=ConsoleObject_ES
    string AnchorName_ConsoleObject = "ARZConsoleObject";
    public string GetAnchorName_ConsoleObject() { return AnchorName_ConsoleObject; }

    //StemBase
    //public GameObject E_StemBase;                        //obj.name=StemBase_ES
    string AnchorName_StemBase = "ARZStemBase";
    public string GetAnchorName_StemBase() { return AnchorName_StemBase; }

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



    #endregion





}
