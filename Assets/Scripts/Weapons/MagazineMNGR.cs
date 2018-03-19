using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineMNGR : MonoBehaviour {



    public Transform GunClipLocation;
    public GameObject FunctionalMagModel;
    //cant unpub , it is used by gun.cs 
    public IMag curMagInGun;
    //unbub
    public GameObject _curActiveMagInstance;
    GameObject _ACTIVE_UI = null;
    //unpub
   // public GameObject _CoppyLinkedOfActivereload;//linked when bundel instantiates us 
    activereloadUIctrl _ActivreReloadScript;


    public void RefillMag() {
        curMagInGun.Refill();
    }
    //*******************************************MAG_MANAGEMENT***************************************
    public bool IsMagPlaced()
    {
        if ( _curActiveMagInstance==null)
        {
            return false;
        }
        return true;
    }

    public bool CanDecrementCurMagBulletCount()
    {
        TestCurMagValidity();
        return curMagInGun.TryDecrementBulletCount();
    }

    public GameObject GetChamberedBullet()
    {
        TestCurMagValidity();
        return curMagInGun.GetBulletFromMag();
    }

    //gun.start with full clip...
    public void InstantiateMagInPlace()
    {
        if (_curActiveMagInstance == null)
        {
            _curActiveMagInstance = Instantiate(FunctionalMagModel, GunClipLocation.position, GunClipLocation.rotation);
            _curActiveMagInstance.transform.parent = GunClipLocation;
            curMagInGun = _curActiveMagInstance.GetComponent<Mag>();
        }
    }

    public bool IsThereBulletsInCurmag()
    {
        TestCurMagValidity();
        if (curMagInGun == null) return false;

        if (curMagInGun.GetBulletsCount_inMag() > 0) return true;
        else
            return false;
    }


    void TestCurMagValidity()
    {
        if (curMagInGun == null) { Debug.LogError("no mag!"); return; }
    }
    //*******************************************xMAG_MANAGEMENT**************************************


    void InitClipInTrans()
    {
        GunClipLocation = gunhelper.DeepSearchContain(this.transform, "_ClipIn");
        if (GunClipLocation == null)
        {
            Debug.Log("_ClipIn not found ");
            GunClipLocation = this.transform;
        }
    }



     void Awake() { InitClipInTrans(); }


    // tracking meter //stemplayerctrl.ItPutsGunInHand or maginhand ->  handscript.ANYHAD_EQUIP  -> takes curIgun.GUNLINK_meter
    // bundle includes public meterobj -> gun  
    //public void GUNRELOAD_LINK_RELOAD_METER(GameObject go)
    //{
    //    _CoppyLinkedOfActivereload = go;
    //}
    // Use this for initialization
  

    // Update is called once per frame
    void Update()
    {

    }

    //    |
    //    |
    //    |
    //    V
    //ANIMATIONEVENTLISTENER
    //public void OnSlideOutAnimComplete()
    //{
    //    GunPopClipOut();
    //    Debug.Log("yo i heard on slid complete and am not on animatorscript");
    //    //ready to accept a clip
    //    //_myGunBools.BmagIn = false; _myGunBools.BHazBullets = false; _myGunBools.BisReloading = false; //_myGunBools.CanAcceptNewClip = true;
    //}
    public void MAGmngerDropRigidMAg()
    {
        if (_curActiveMagInstance != null)
        {
            GameObject rigidclip = _curActiveMagInstance;
            curMagInGun = null;
            _curActiveMagInstance = null;
            
            rigidclip.transform.parent = null;
            DestroyObject(_curActiveMagInstance);
            rigidclip.AddComponent<Rigidbody>();
            rigidclip.GetComponent<Rigidbody>().AddForce(GunClipLocation.forward * -2, ForceMode.Impulse);
            //rigidclip.GetComponent<Mag>().InitCanPlayCollisionSound();
            KillTimer t = rigidclip.AddComponent<KillTimer>();
            t.StartTimer(5.0f);
        }

        //_myGunBools.BmagIn = false;

        ////        _myGunBools.BmagIn = false; _myGunBools.BHazBullets = false; _myGunBools.BisReloading = false;
        //_myGunBools.CanAcceptNewClip = true;
    }

    //
    //    |
    //    |
    //    |
    //    V
    // IF COLLISION AND HAD IS FULL OF AMMO 

    //public void InstantiateMagInPlace()
    //{
    //    if (_curActiveMagInstance == null)
    //    {
    //        _curActiveMagInstance = Instantiate(FunctionalMagModel, GunClipLocation.position, GunClipLocation.rotation);
    //        _curActiveMagInstance.transform.parent = GunClipLocation;
    //        curMagInGun = _curActiveMagInstance.GetComponent<Mag>();
    //    }
    //}

    public void GunInstantiateMagInClipPlace()
    {
        InstantiateMagInPlace();
        //_curActiveMagInstance = Instantiate(FunctionalMagModel, GunClipLocation.position, GunClipLocation.rotation);
        //_curActiveMagInstance.transform.parent = GunClipLocation;

        //curMagInGun = _curActiveMagInstance.GetComponent<Mag>();
        //////_myGunBools.BmagIn = true;  _myGunBools.BisReloading = true;
        ////_myGunBools.CanAcceptNewClip = false;


        ////if (curMagInGun.GetBulletsCount_inMag() > 0) { _myGunBools.BHazBullets = true; }
        ////else
        ////{
        ////    _myGunBools.BHazBullets = false;
        ////}
        ////g_animator.SetTrigger("TrigMagSlide"); // --> SLIDEIN -> CLOSESLIDER
        
    }
    //
    //                                                                   |
    //                                                                   |
    //                                                                   |
    //                                                                   V
    //                                                             Listener OnMagSlideAnimCompleted //wrong name should be closeslidercomplete
    //




}
