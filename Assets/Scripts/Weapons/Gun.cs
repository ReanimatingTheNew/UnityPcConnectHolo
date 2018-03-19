// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour, IGun {
    #region oldStuff
    GunEffects _myGunEffect;
    GunAnimate _myGunAnimate;
   // ReloadMNGR _myGunReload; //GUNRELOAD_LINK_RELOAD_METER
    MagazineMNGR _myMagazineMNGR;
    public MagazineMNGR GetGunMagMngr() { return this._myMagazineMNGR; }
    public GunType gunType;
    public Transform barrelTran;
    GameObject BulletReff;
   
    public GunBools _myGunBools;

    bool stopSpreading = true;  
    float spreadAngle = 0.0f;
    float GLOBALANGLE = 0.0f;

    public int GetCurGunIndex(){ return (int)gunType; }

    //*************is for fire methed , it can stay in Gun.cs
    TimerBehavior reloadTimer;                  // object that keeps track of reload time
    TimerBehavior repeatTimer;                  // object that keeps track of repeat fire
    [Tooltip("This time is only used if the weapon is an Uzi")]
    float repeatTime;
    //*************is for fire methed , it can stay in Gun.cs                    

    void GunFlash_FIRE()
    {
        if (_myGunEffect != null) { _myGunEffect.FlashEffect();
            if (gunType == GunType.PISTOL || gunType == GunType.UZI) { _myGunEffect.CasingEjectEffect(); }
        }

    }

    #endregion



    #region ListenersANdHAndler

    private void OnEnable()
    {
        StemKitMNGR.OnUICellFilled += GunHandle_CellFiled;
        StemKitMNGR.On_Override_UICellid += GunHandle_CellOverride;
        StemKitMNGR.OnResetGunAndMeter += ResetGunReloadState;

    }
    private void OnDisable()
    {
        StemKitMNGR.OnUICellFilled -= GunHandle_CellFiled;
        StemKitMNGR.On_Override_UICellid -= GunHandle_CellOverride;
        StemKitMNGR.OnResetGunAndMeter -= ResetGunReloadState;

    }

    void GunHandle_CellFiled(int argIdOfFilledCEll) {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {


            if (argIdOfFilledCEll == 0)
            {
                _myGunAnimate.Gunimate_OPENSLIDER();
                _myGunEffect.AUDIO_PopMagOut();

            }
            else
            if (argIdOfFilledCEll == 1)
            {
                GunInjstantiateMagANDSLIDEINanim(); _myGunBools.CanAcceptNewClip = false;
                _myGunEffect.AUDIO_PushMagIn();
            }
            else
            if (argIdOfFilledCEll == 2)
            {
               // Debug.Log("CAN  SHOOT not yet, wiat for a mock HAMERanimatoin time to end , and just play HAMMERDOWN?");
                StemKitMNGR.CALL_Start_UIcell(-666);
                //___ time0.8 seconds of wait time before this point \
                _myGunAnimate.Gunimate_HAMMERDOWN();//                |~
                _myGunEffect.AUDIO_Chamber();                         //                                                     | to this poit is a wait of .8 
            }//                                                       |
            else//                                                    V              
            if (argIdOfFilledCEll == -666) //this is wird, since i dont have animation event on hammerdown, i ll fake it by making reloadmeterctrl run a waitforseconds, and at the same time jujst play the hammerdown anim. when waitfr ends, it should call CEllfilled(-666) which will hit here 
            {
               // Debug.Log("and now i can shoot");
                _myGunBools.ThisGunIsReloading = false;
                _myGunBools.CAnManuallyDropMag = true;


                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GetScoreMAnager().Increment_ReloadsCNT();
                }
            }
            else
                Debug.LogError("there should only be 0,1,2, -666 right now");

        }

    }

    void GunHandle_CellOverride(int id) {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            if (id == -1) { _myGunBools.ThisGunIsReloading = true; _myGunBools.CanAcceptNewClip = false; }
        }
    }

    public void OnSlideOutAnimComplete()
    {
        if (GameSettings.Instance == null)
        {
            DropMagFromGun();
            return;
        }

        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            DropMagFromGun();
            _myGunBools.CanAcceptNewClip = true;
            StemKitMNGR.CALL_Start_UIcell(1);
        }

    }

    public void OnMagSlideAnimCompleted()
    {
        if (GameSettings.Instance == null) {
            _myGunBools.CanAcceptNewClip = false;
            return;
        }

        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            _myGunBools.CanAcceptNewClip = false;
            StemKitMNGR.CALL_Start_UIcell(2);
        }
        }

    #endregion


    public void AUDIO_PopMagOut() {
        _myGunEffect.AUDIO_PopMagOut();
    }
    public void AUDIO_PopMagIn()
    {
        _myGunEffect.AUDIO_PushMagIn();
    }
 

    void Awake()
    {
        _myGunBools = new GunBools( false, false, true);
        _myGunEffect = GetComponent<GunEffects>();
      
        _myGunAnimate = GetComponent<GunAnimate>();
        _myMagazineMNGR = GetComponent<MagazineMNGR>();//GUNRELOAD_LINK_RELOAD_METER
        repeatTimer = gameObject.AddComponent<TimerBehavior>();
        if (gunType == GunType.UZI)
        {
            repeatTime = 0.05f;
        }
        else
        {
            repeatTime = 0f;
        }
        InitBarelTrans();
    }
   
    void Start() {

       
        //  StartEmpty_MagIsOut();//set statemoty and hopefully not load smthin
        GUN_START_MAG_IN();
        
        //  GunPutClipInNoOPENLOADANIMATIONS();
    }

    void InitBarelTrans()
    {
        barrelTran = gunhelper.DeepSearch(this.transform, "BarrelObj");
        if (barrelTran == null)
        {
            Debug.LogError("barel not found ");
            barrelTran = this.transform;
         
        }
    }





    public GunBools GUN_GET_BOOLS() { return this._myGunBools; }


    public void ResetGunReloadState() { GUN_START_MAG_IN(); }

    public void GUN_START_MAG_IN()
    {
        //needs a mag with bullets
        _myMagazineMNGR.InstantiateMagInPlace();

        //BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOLS
        // A FACT IS A FACT, I can get rid of these 
        //_myGunBools.BmagIn = _myMagazineMNGR.IsMagPlaced();
       // _myGunBools.BHazBullets = _myMagazineMNGR.IsThereBulletsInCurmag();
        //I DECIDE WHEN THESE GET SET in GUN because GUN-> can accept new clip, notg MamgMANGR same as gun is reloading
        _myGunBools.CanAcceptNewClip = false;
        _myGunBools.ThisGunIsReloading = false;
        _myGunBools.CAnManuallyDropMag = true;
        //BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOLS

        // g_animator.Play("SLIDEIN");//will trigger closeslider and hit our listener 
    }

    public void GUN_START_MAG_OUT()
    {
        //_myGunBools.BmagIn = false;
        //_myGunBools.BHazBullets = false;
        //_myGunBools.BisReloading = false;
        //_myGunBools.CanAcceptNewClip = false; <-- at end of this method , the animation SLIDEOUT is started-> triggers OnSlideOutAnimComplete event and this bool will get set to true
        //BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOLS
        // A FACT IS A FACT, I can get rid of these 
        //_myGunBools.BmagIn = _myMagazineMNGR.IsMagPlaced();
        //_myGunBools.BHazBullets = _myMagazineMNGR.IsThereBulletsInCurmag();
        //I DECIDE WHEN THESE GET SET in GUN because GUN-> can accept new clip, notg MamgMANGR same as gun is reloading
        _myGunBools.CanAcceptNewClip = false;
        _myGunBools.ThisGunIsReloading = false;
        _myGunBools.CAnManuallyDropMag = false;
        //BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOLS

        _myGunAnimate.Gunimate_OPENSLIDER();//  g_animator.Play("OPENSLIDER"); //then goes to SLIDEOUT-> triggers OnSlideOutAnimComplete
    }
    //_myGunBools.CanAcceptNewClip = true;
    public void GUN_FIRE()
    {
        //****************************************************************************************************************************************
        //gunhelper.DrawStatic(barrelTran.transform.position, barrelTran.transform.position + (barrelTran.transform.forward * -5));
        //****************************************************************************************************************************************

        Quaternion thenewrot = DoSpread(stopSpreading);
        // gunhelper.DrawStatic(barrelTran.transform.position, thenewrot  * Vector3.one *5);

        //****************************************************************************************************************************************
        //gunhelper.DrawStatic(barrelTran.transform.position, barrelTran.transform.position + (thenewrot*(barrelTran.transform.forward * 5)  ));
        //****************************************************************************************************************************************
        if (_myGunBools.ThisGunIsReloading) { Debug.Log("no shot I b reloading "); return; }

        if (!_myMagazineMNGR.IsMagPlaced()) { Debug.Log("no shot no mag in "); return; }
         
        if (!_myMagazineMNGR.IsThereBulletsInCurmag())
        {
           // Debug.Log("play dry shot  caling cell0");
            _myGunEffect.AUDIO_Dry();
            if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
            {
                StemKitMNGR.CALL_Start_UIcell(0);

                _myGunBools.ThisGunIsReloading = true;
                _myGunBools.CAnManuallyDropMag = true;
            }
            return;
        }

         
        {

            // repeat fire if gun type is uzi
            if (gunType == GunType.UZI)
            {
                _myGunAnimate.PlayFastest();
                stopSpreading = false;
                repeatTimer.StartTimer(repeatTime, GUN_FIRE, false);
            }
            else
            {
                _myGunAnimate.PlayFast();
            }

            if (!_myMagazineMNGR.IsMagPlaced())
            {
                Debug.Log("no mag");
                GUN_STOP_FIRE();//FOR SAFE MEASURE
                return;
            }
            if (_myMagazineMNGR.CanDecrementCurMagBulletCount())
            {
                //_myGunBools.BHazBullets = true;

                Instantiate(_myMagazineMNGR.GetChamberedBullet(), barrelTran.position, thenewrot);// barrelTran.rotation);
                _myGunAnimate.Gunimate_FIRE();
                GunFlash_FIRE();
            }
         
        }




    }


    Quaternion DoSpread(bool argstopspread)
    {
        GLOBALANGLE += 2.2f;
        if (GLOBALANGLE > 30) { GLOBALANGLE = 30; }

        spreadAngle = GLOBALANGLE;
        float RandAng = Random.Range(0.0f, spreadAngle);
        Vector3 FireDirection = transform.forward;
        Quaternion fireRotation = Quaternion.LookRotation(FireDirection);
        Quaternion randRot = Random.rotation;
        fireRotation = Quaternion.RotateTowards(fireRotation, randRot, RandAng);
        if (!argstopspread) { return fireRotation; }

        else
        { return barrelTran.rotation; }
    }


    public void GUN_STOP_FIRE()
    {
        Debug.Log(" gun stops fire");
        _myGunAnimate.PlayNormal();
        stopSpreading = true;
        GLOBALANGLE = 0.0f;
        spreadAngle = 0.0f;
        repeatTimer.StopTimer();
    }

    public GunType GunGetGunType()
    {
        throw new System.NotImplementedException();
    }

    //********************************************
    //add rigidbody let gravity do its thing ---> triggers animation
    //********************************************
    public void MANUAL_EJECT_MAG_OUT()
    {
        // throw new System.NotImplementedException();
        _myGunAnimate.Gunimate_OPENSLIDER();
        _myGunBools.CAnManuallyDropMag = false;
    }

    public void GunInjstantiateMagANDSLIDEINanim()
    {
        _myMagazineMNGR.GunInstantiateMagInClipPlace();
        _myGunAnimate.Gunimate_SLIDEIN();// --> SLIDEIN -> CLOSESLIDER
    }

    public void DropMagFromGun()
    {
        _myMagazineMNGR.MAGmngerDropRigidMAg();
    }

    public void PopAnActiveReload()
    {
        throw new System.NotImplementedException();
    }

    public void MagicReloadBulletCount()
    {
        _myMagazineMNGR.RefillMag();
        _myGunEffect.AUDIO_FullReload();
    }

    public void ToggleLazerOnOff() {
        _myGunEffect.SetToggleLazer();
    }
}


