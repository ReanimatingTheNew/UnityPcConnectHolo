using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCTRL : MonoBehaviour , IPlayerHandsCTRL{


    #region PublicProps
    MainHandScript MAinHandScript;
    OffHandScript OFFhandScript;
    public TextMesh _meshGunStatus;
    #endregion
    #region PrivateProps
    GunType _CurWave_Guntype;
    Ammunition _CurWave_Ammotype;
    bool isPlayerAllowedToUseStemInput = false;
    int _curGunenumIndex = 1;
    int _latestUnlockedGunEnumIndex;
    HoloToolkit.Unity.UAudioManager _uaudio;
    bool _CanStartLocatingTrackers = false;
    bool _allowExtraButtonUsage = true;
    bool _hasSelectedNextGun = false;
    bool _hasSelectedPreviousGun = false;
    bool _hasShotOnceAlready = false;
    float _StemTriggerPullTimer = 0.0f;

    private IGun _curEquippedIGun;
    //public IGun CurEquippedIGun
    //{
    //    get { return _curEquippedIGun; }
    //    set { _curEquippedIGun = value; }
    //}

    #endregion


    public void InitPlayerHandsScripts(MainHandScript argMAIN, OffHandScript argOFF)
    {
        MAinHandScript = argMAIN;
        OFFhandScript = argOFF;
    }


    #region Events
    private void OnEnable()
    {
        //isPlayerAllowedToUseStemInput = false;
        //StemKitMNGR.OnSTEMCONNECTED += Handle_StemConnected;
        //StemKitMNGR.OnSTEM_DIS_CONNECTED += Handle_StemDisconnected; 
        StemKitMNGR.OnOffhandTouchedSMTHN += Handle_OffHandCollision;
        StemKitMNGR.OnOffhandTouchedGUNMAG += Handle_OffHandCollidesWithMag;
        StemKitMNGR.OnUpdateCurIGun += Handle_SetCurEquippedGun;
        StemKitMNGR.OnGunSetChanged += Handle_GunSetChanged;
        StemKitMNGR.OnUICellFilled += Handle_Segment1Ended_AutoClearOffHand;
        StemKitMNGR.OnUpdateAvailableGUnIndex += Handle_UpdateLatestUnlockedGunEnumIndex;
        StemKitMNGR.OnToggleONOFFExtraButtons += Handle_ToggleAllowExtraButtons;
        StemKitMNGR.OnToggleInputs += Handle_ToggleAllowStemInputs;

        GameManager.OnGamePaused += Handle_ToggleAllowStemInputs;
        GameManager.OnGameContinue += Handle_ToggleAllowStemInputs;
    }
    private void OnDisable()
    {
        //StemKitMNGR.OnSTEMCONNECTED -= Handle_StemConnected;
        //StemKitMNGR.OnSTEM_DIS_CONNECTED -= Handle_StemDisconnected;
        StemKitMNGR.OnOffhandTouchedSMTHN -= Handle_OffHandCollision;
        StemKitMNGR.OnOffhandTouchedGUNMAG -= Handle_OffHandCollidesWithMag;
        StemKitMNGR.OnGunSetChanged -= Handle_GunSetChanged;
        StemKitMNGR.OnUpdateCurIGun -= Handle_SetCurEquippedGun;
        StemKitMNGR.OnUICellFilled -= Handle_Segment1Ended_AutoClearOffHand;
        StemKitMNGR.OnUpdateAvailableGUnIndex -= Handle_UpdateLatestUnlockedGunEnumIndex;
        StemKitMNGR.OnStartFindingTrackers -= Handle_StartFindingTrackers;
        StemKitMNGR.OnToggleONOFFExtraButtons -= Handle_ToggleAllowExtraButtons;
        StemKitMNGR.OnToggleInputs -= Handle_ToggleAllowStemInputs;

        GameManager.OnGamePaused -= Handle_ToggleAllowStemInputs;
        GameManager.OnGameContinue -= Handle_ToggleAllowStemInputs;
    }
    #endregion

    #region EventsHandlers
    ////tracker will broadcast using stemkitmngr Event
    //void Handle_StemDisconnected(TrackerID argid) { Debug.Log("yo ," + argid.ToString() + "is off line");
    // //   DoGame = false; _meshConnectionStatus.text = " Disconnection Heard";
    //}

    //void Handle_StemConnected(TrackerID argid) { Debug.Log("yo ," + argid.ToString() + "is ONLINE");
    //   // DoGame = true; _meshConnectionStatus.text = " Connected Heard";
    //}

    //colliders ontrigger will broadcast
    void Handle_OffHandCollision(string argTouchedTag)
    {


        // Debug.Log("yo i heard oFFhand touched -> tag" + argTouchedTag);
        if (argTouchedTag == "Ammo")
        {
            if (!OFFhandScript.MyBun.IsMyThingShowing())
            {
                _uaudio.PlayEvent("_EquipAmmo");

                OFFhandScript.MyBun.ShowMyCurrBunThing();
                OFFhandScript.AnimateHoldAmmo();
            }

            //todo, sound should come from ammo box , and only if ammo can be grabbed


        }
        if (argTouchedTag == "ShootyHand")
        {
            if (GameSettings.Instance.ReloadDifficulty!=ARZReloadLevel.EASY)
            {
                if (OFFhandScript.MyBun.IsMyThingShowing())
                {
                    if (MAinHandScript.MyBun.IsMyThingShowing())//not really needed 
                    {

                        PrivatTapintoCOLLISION();

                    }

                }
            }



        }



    }

    //colliders ontrigger will broadcast
    void Handle_OffHandCollidesWithMag()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            if (OFFhandScript.MyBun.IsMyThingShowing())
            {
                if (MAinHandScript.MyBun.IsMyThingShowing())//not really needed 
                {
                    if (_curEquippedIGun.GUN_GET_BOOLS().CanAcceptNewClip)
                    {
                        PrivatTapintoCOLLISION();
                    }

                }

            }
        }

    }

    

    //StemkitMNGR.FullInit()-> SetGuns() 
    //GameManager.Setgun()  
    //this.SelectNextGun()
    //this.SelectPreviousGun()
    void Handle_GunSetChanged(GunType gunType)
    {
        Debug.Log("C H A N G E ");
        Equips_GunInMainHand(gunType);
        Equips_MagInOffHand(gunType);
        Fixs_OffhandRotation();
    }

    //only if segment 1 finished so we can auto put mag in gun, and clear offhand
    void Handle_Segment1Ended_AutoClearOffHand(int IdOfFilledCell)
    {
        if (IdOfFilledCell == 1)
        {
            //_IseeThisGun.MANUAL_INJECT_MAG_IN();
            OFFhandScript.MyBun.HideMyCurrBunThing();
            OFFhandScript.Animate_OpenHand();
        }
    }

    //this.Handle_GunSetChanged(ttupe)->  this.Equips_GunInMainHand(type)  GunsBundles.Show/or/hideMyCurrBunThing() ->  Set_EquipedGunObject_Visible()
    void Handle_SetCurEquippedGun(IGun argIgun)
    {
        _curEquippedIGun = argIgun;
    }

    //WaveStandard.StartWave()  CALL_UpdateAvailableGUnIndex(cureWavenumber)
    //GameManager.SetGun()      CALL_UpdateAvailableGUnIndex(3)
    void Handle_UpdateLatestUnlockedGunEnumIndex(int argindex)
    {
        if (argindex > 0 && argindex < 4)
        {
            _latestUnlockedGunEnumIndex = argindex;
        }
        else
            _latestUnlockedGunEnumIndex = 1;
    }

    //StemkitMNGR.Start() -> INitHandsBundles()
    void Handle_StartFindingTrackers(bool argBool)
    {
        _CanStartLocatingTrackers = argBool;
    }

    void Handle_ToggleAllowExtraButtons(bool argOnOff)
    {
        _allowExtraButtonUsage = argOnOff;
    }

    //wavestandard.wavestarted  Toggle(true)
    //GameManager.resetWave()   Toggle(true)
    //GameManager.Setgun()      Toggle(true)
    //GameManager.TimeIsUp()    Toggle(false)
    //GameManager.PlayerDied()  Toggle(false)
    //GameManager.HardStop()    Toggle(false)
    void Handle_ToggleAllowStemInputs(bool argb) { isPlayerAllowedToUseStemInput = argb; }


    #endregion

    void PrivatTapintoCOLLISION()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            StemKitMNGR.Call_OVR_Cell_ID(1);

            _curEquippedIGun.GunInjstantiateMagANDSLIDEINanim();
            OFFhandScript.MyBun.HideMyCurrBunThing();
            OFFhandScript.Animate_OpenHand();
        }
        else
        {
            _curEquippedIGun.MagicReloadBulletCount();
            OFFhandScript.MyBun.HideMyCurrBunThing();
            OFFhandScript.Animate_OpenHand();

        }

    }

    private void Start()
    {
        isPlayerAllowedToUseStemInput = false;
        _uaudio = GetComponent<UAudioManager>();
        _latestUnlockedGunEnumIndex = 3;

    }

    // tracking meter // evrytime i put a gun in my hand(bundle gun enable) -> I endup making gunbundles pass a reff to metergo to the gun activated
    void Equips_GunInMainHand(GunType gunType)
    {
        MAinHandScript.Equip_byEquipmentIndex((int)gunType);
    }
    void Equips_MagInOffHand(GunType gunType) { OFFhandScript.Equip_byEquipmentIndex((int)gunType); }
    void Fires_EquippedGun()
    {
        _curEquippedIGun.GUN_FIRE();
        //  Debug.Log("fire");
    }
    void FireStops_EquippedGun()
    {
        _curEquippedIGun.GUN_STOP_FIRE();
        // Debug.Log("STOPfire");
    }
    void Ejects_LoadedMag()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            if (!_curEquippedIGun.GUN_GET_BOOLS().CanAcceptNewClip)
                StemKitMNGR.Call_OVR_Cell_ID(-1);
        }
        //pressing button only skips seg0 , i will check curr state in meterCTRL, if we are i seg 0 we skip, if not do nothing
        //_IseeThisGun.MANUAL_EJECT_MAG_OUT();
    }
    void Hammers_GunSlider()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            StemKitMNGR.Call_OVR_Cell_ID(2);
        }
    }

    void SelectNextGun()
    {
        if (_curEquippedIGun.GUN_GET_BOOLS().ThisGunIsReloading) return;
        if (_hasSelectedNextGun) return;
        _curGunenumIndex = _curEquippedIGun.GetCurGunIndex();
        _curGunenumIndex++;
        if (_curGunenumIndex > _latestUnlockedGunEnumIndex)
        {
            _curGunenumIndex = _latestUnlockedGunEnumIndex;
        };
        StemKitMNGR.Call_GunSetChangeTo((GunType)_curGunenumIndex);
        _hasSelectedNextGun = true;
    }
    void SelectPreviousGun()
    {
        if (_curEquippedIGun.GUN_GET_BOOLS().ThisGunIsReloading) return;
        if (_hasSelectedPreviousGun) return;

        _curGunenumIndex = _curEquippedIGun.GetCurGunIndex();
        _curGunenumIndex--;
        if (_curGunenumIndex < 1) { _curGunenumIndex = 1; };
        StemKitMNGR.Call_GunSetChangeTo((GunType)_curGunenumIndex);
        _hasSelectedPreviousGun = true;
    }
    
    void ReadKeyBoardInput(bool argAllowExtra)
    {

            if (isPlayerAllowedToUseStemInput)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { SelectPreviousGun(); }
                if (Input.GetKeyDown(KeyCode.RightArrow)) { SelectNextGun(); }

                if (Input.GetMouseButtonDown(0)) { Fires_EquippedGun(); }
                if (Input.GetMouseButtonUp(0)) { FireStops_EquippedGun(); }

                //disable laser from here instead of gun
                //if ((Input.GetKeyDown(KeyCode.UpArrow)) ){
                //if (argAllowExtra) _curEquippedIGun.ToggleLazerOnOff(); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                //Ejects_LoadedMag();
                StemKitMNGR.CALL_ResetGunAndMeter();
                }
            }


        

    }

    void TextStatus()
    {
        if (_curEquippedIGun != null)
        {
            string magstat = _curEquippedIGun.GetGunMagMngr().IsMagPlaced() == true ? " ON" : "___";
            string bulletstat = _curEquippedIGun.GetGunMagMngr().IsThereBulletsInCurmag() == true ? " ON" : "___";
            string acceptclipStat = _curEquippedIGun.GUN_GET_BOOLS().CanAcceptNewClip == true ? " ON" : "___";
            string reloadingtimer = _curEquippedIGun.GUN_GET_BOOLS().ThisGunIsReloading == true ? " ON" : "___";

            string mystatusline = "";
            mystatusline += "MagazineIN :" + magstat + "\n";
            mystatusline += "has bullets:" + bulletstat + "\n";
            mystatusline += "accept clip:" + acceptclipStat + "\n";
            mystatusline += "is reloadin:" + reloadingtimer + "\n";
            mystatusline += "\n";
            mystatusline += "cur gun index from gun" + _curEquippedIGun.GetCurGunIndex();
            mystatusline += "cur gun index" + _curGunenumIndex;
            //string mystatusline= " MGin " + _IseeThisGun.GEtGunBools().BmagIn.ToString() + "| bull " + _IseeThisGun.GEtGunBools().BHazBullets.ToString() + "| accepnew " + _IseeThisGun.GEtGunBools().CanAcceptNewClip.ToString()+ "| reloa? " + _IseeThisGun.GEtGunBools().BisReloading.ToString();
            _meshGunStatus.text = mystatusline;
        }
    }

    void Fixs_OffhandRotation() { OFFhandScript.FixOffhandWeirdRoation(); }

    void Update() {
        ReadKeyBoardInput(_allowExtraButtonUsage);
    }
}
