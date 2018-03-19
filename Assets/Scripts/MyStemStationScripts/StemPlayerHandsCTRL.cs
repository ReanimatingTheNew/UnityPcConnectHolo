// @Author Nabil Lamriben ©2018
using HoloToolkit.Unity;
using SixenseCore;
using UnityEngine;
//controls what hand is holding what and what happens on collisions  //NO animations are done via events , and initialized when han what to do with animations
public class StemPlayerHandsCTRL : MonoBehaviour,IPlayerHandsCTRL {


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
    UAudioManager _uaudio;
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

    protected SixenseCore.TrackerVisual[] m_trackers;

    #endregion

    #region Events
    private void OnEnable()
    {
        //isPlayerAllowedToUseStemInput = false;
        //StemKitMNGR.OnSTEMCONNECTED += Handle_StemConnected;
        //StemKitMNGR.OnSTEM_DIS_CONNECTED += Handle_StemDisconnected; 
        StemKitMNGR.OnOffhandTouchedSMTHN += Handle_OffHandCollision;
        StemKitMNGR.OnOffhandTouchedGUNMAG += Handle_OffHandCollidesWithMag;
        StemKitMNGR.OnVibrate += Handle_Vibrate;
        StemKitMNGR.OnUpdateCurIGun += Handle_SetCurEquippedGun;
        StemKitMNGR.OnGunSetChanged += Handle_GunSetChanged;
        StemKitMNGR.OnUICellFilled += Handle_Segment1Ended_AutoClearOffHand;
        StemKitMNGR.OnUpdateAvailableGUnIndex += Handle_UpdateLatestUnlockedGunEnumIndex;
        StemKitMNGR.OnStartFindingTrackers += Handle_StartFindingTrackers;
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
        StemKitMNGR.OnVibrate -= Handle_Vibrate;
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
            if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
            {
                Debug.Log("offhand touched MainHand, now it needs to touch mag collider, so what ..this is ACTIVE RELOAD bitch ... ");

                //if (OFFhandScript.MyBun.IsMyThingShowing())
                //{
                //    if (MAinHandScript.MyBun.IsMyThingShowing())//not really needed 
                //    {
                //        if (_IseeThisGun.GUN_GET_BOOLS().CanAcceptNewClip)
                //        {
                //            PrivatTapintoCOLLISION();
                //        }

                //    }

                //}
            }
            else
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
    void Handle_OffHandCollidesWithMag(){
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

    //Gun Effects flash Calls stemkitmngr Broadcast 
    void Handle_Vibrate(int x, float zeroone)
    {
        if (GameSettings.Instance.IsAllowVibrate)
        {

            SixenseCore.TrackerVisual tracker = null;
            foreach (var t in m_trackers)
            {
                if (t.HasInput)
                {
                    tracker = t;
                    break;
                }
            }

            if (tracker == null)
                return;

            var controller = tracker.Input;
            controller.Vibrate(x, zeroone);
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

    void Handle_ToggleAllowExtraButtons(bool argOnOff) {
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

    delegate void helpdebug(float xf, float yf);
    void PRINTER(float xf, float yf) { Debug.Log("Xval= " + xf + "  Yval" + yf); }

    private void Start()
    {
        isPlayerAllowedToUseStemInput = false;
        _uaudio = GetComponent<UAudioManager>();
        _latestUnlockedGunEnumIndex = 3;

    }

    private void Update()
    {
       Keyboardinput();
        ReadStemInput(_allowExtraButtonUsage);
        //if(GameSettings.Instance.IsTestModeON)  
        //TextStatus();
    }


    public void InitPlayerHandsScripts(MainHandScript argMAIN, OffHandScript argOFF)
    {
        m_trackers = argMAIN.gameObject.GetComponentsInParent<SixenseCore.TrackerVisual>();

        MAinHandScript = argMAIN;
        OFFhandScript = argOFF;
    }


    // tracking meter // evrytime i put a gun in my hand(bundle gun enable) -> I endup making gunbundles pass a reff to metergo to the gun activated
    void Equips_GunInMainHand(GunType gunType) {
        MAinHandScript.Equip_byEquipmentIndex((int)gunType);
    }
    void Equips_MagInOffHand(GunType gunType) { OFFhandScript.Equip_byEquipmentIndex((int)gunType); }       
    void Fires_EquippedGun() {
        _curEquippedIGun.GUN_FIRE();
      //  Debug.Log("fire");
    }
    void FireStops_EquippedGun() {
        _curEquippedIGun.GUN_STOP_FIRE();
       // Debug.Log("STOPfire");
    }
    void Ejects_LoadedMag() {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            if (!_curEquippedIGun.GUN_GET_BOOLS().CanAcceptNewClip)
                StemKitMNGR.Call_OVR_Cell_ID(-1);
        }
        //pressing button only skips seg0 , i will check curr state in meterCTRL, if we are i seg 0 we skip, if not do nothing
        //_IseeThisGun.MANUAL_EJECT_MAG_OUT();
    }
    void Hammers_GunSlider() {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            StemKitMNGR.Call_OVR_Cell_ID(2);
        }
    }
 
    void ReadJoystickHorizontalInput(Tracker ArgTracker) {


        //    -1                   -0.2             0             0.2                   1
        //                                                                              
        if (ArgTracker.JoystickX > 0.2f)
        {  if (!_hasSelectedNextGun) { SelectNextGun(); } }
        if (ArgTracker.JoystickX < 0.2f)
        {   _hasSelectedNextGun = false; }




        if (ArgTracker.JoystickX < -0.2f)
        {  if (!_hasSelectedPreviousGun) { SelectPreviousGun(); } }
        if (ArgTracker.JoystickX >-0.2f)
        {   _hasSelectedPreviousGun = false; }
 

    }
    void SelectNextGun(){
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
            if (_curGunenumIndex < 1 ) { _curGunenumIndex = 1; };
        StemKitMNGR.Call_GunSetChangeTo((GunType)_curGunenumIndex);
        _hasSelectedPreviousGun = true;
    }
    void ReadTriggetInput(Tracker ArgTracker)
    {

        if (ArgTracker.GetButton(Buttons.TRIGGER))
        {
            _StemTriggerPullTimer += Time.deltaTime;
            if (!_hasShotOnceAlready)
            {

                Fires_EquippedGun();
                FireStops_EquippedGun();
                _hasShotOnceAlready = true;
            }


            if (_StemTriggerPullTimer > 0.2f)
            {
                _StemTriggerPullTimer = 0.0f;
                if (_curEquippedIGun.GetCurGunIndex() == (int)GunType.UZI) { Fires_EquippedGun(); }

                //ItStopsShooting();
            }
        }
        if (ArgTracker.GetButtonUp(Buttons.TRIGGER))
        {
            _hasShotOnceAlready = false;
            _StemTriggerPullTimer = 0.0f;
            FireStops_EquippedGun();
        }

    }
    void ReadStemInput(bool argAllowExtra)
    {
        helpdebug hd = PRINTER;
        if (_CanStartLocatingTrackers)
        {
            SixenseCore.TrackerVisual tracker = null;
            foreach (var t in m_trackers)
            {
                if (t.HasInput)
                {
                    tracker = t;
                    break;
                }
            }

            if (tracker == null)
                return;

            var controller = tracker.Input;
            //var id = tracker.m_trackerBind;

            //if (controller.GetButtonDown(SixenseCore.Buttons.TRIGGER)) {
            //    if (GameSettings.Instance.IsAllowVibrate) { controller.Vibrate(150, 1); } ItShootsGun();
            //}

            //if (controller.GetButtonUp(SixenseCore.Buttons.TRIGGER)) { ItStopsShooting(); }



            if (isPlayerAllowedToUseStemInput)
            {
                ReadJoystickHorizontalInput(controller);
                ReadTriggetInput(controller);

                //disable laser from here instead of gun 
                //if (controller.GetButtonUp(SixenseCore.Buttons.BUMPER)) { if (argAllowExtra) _curEquippedIGun.ToggleLazerOnOff(); }

                if (controller.GetButtonDown(Buttons.NEXT) ||
                        controller.GetButtonDown(Buttons.PREV) ||
                        controller.GetButtonDown(Buttons.A) ||
                        controller.GetButtonDown(Buttons.B) ||
                        controller.GetButtonDown(Buttons.X) ||
                        controller.GetButtonDown(Buttons.Y))
                {
                    Ejects_LoadedMag();
                    if (GameSettings.Instance.IsAllowVibrate) { controller.Vibrate(500, .5f); }
                }
            }


        }

    }

    void TextStatus() {
        if (_curEquippedIGun != null) {
            string magstat = _curEquippedIGun.GetGunMagMngr().IsMagPlaced() == true ? " ON" :"___";
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

    void PrivatTapintoCOLLISION()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            StemKitMNGR.Call_OVR_Cell_ID(1);

            _curEquippedIGun.GunInjstantiateMagANDSLIDEINanim();
            OFFhandScript.MyBun.HideMyCurrBunThing();
            OFFhandScript.Animate_OpenHand();
        }
        else {
            _curEquippedIGun.MagicReloadBulletCount();
            OFFhandScript.MyBun.HideMyCurrBunThing();
            OFFhandScript.Animate_OpenHand();

        }
      
    }

    void Fixs_OffhandRotation() { OFFhandScript.FixOffhandWeirdRoation(); }


    void Keyboardinput()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow)) { KEYBOARDSelectNextGun(); }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { KEYBOARDSelectPreviousGun(); }

        //if (Input.GetKeyDown(KeyCode.C)) { PrivatTapintoCOLLISION(); }
        ////PrivatTapintoCOLLISION
        //if (Input.GetKeyDown(KeyCode.E)) { Ejects_LoadedMag(); }
        //if (Input.GetKeyDown(KeyCode.H)) { Hammers_GunSlider(); }

    }

    void KEYBOARDSelectNextGun()
    {
        if (_curEquippedIGun.GUN_GET_BOOLS().ThisGunIsReloading) return;
         _curGunenumIndex = _curEquippedIGun.GetCurGunIndex();
        _curGunenumIndex++;
        if (_curGunenumIndex > _latestUnlockedGunEnumIndex)
        {
            _curGunenumIndex = _latestUnlockedGunEnumIndex;
        };
        StemKitMNGR.Call_GunSetChangeTo((GunType)_curGunenumIndex);
     }
    void KEYBOARDSelectPreviousGun()
    {
        if (_curEquippedIGun.GUN_GET_BOOLS().ThisGunIsReloading) return;
 
        _curGunenumIndex = _curEquippedIGun.GetCurGunIndex();
        _curGunenumIndex--;
        if (_curGunenumIndex < 1) { _curGunenumIndex = 1; };
        StemKitMNGR.Call_GunSetChangeTo((GunType)_curGunenumIndex);
     }
}







//void HandleSegmentEndedEvent(ReloadMeterState argstate) {
//    if ((int)argstate == 1)
//    {
//        _curEquippedIGun.GunInjstantiateMagANDSLIDEINanim();
//        OFFhandScript.MyBun.HideMyCurrBunThing();
//        OFFhandScript.Animate_OpenHand();
//    }
//}