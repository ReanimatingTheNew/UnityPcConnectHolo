using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevInputs : MonoBehaviour {
    public GameObject TheMeter;
    public ReloadMeterCTRL _meterCTRL;

    //public GunType guntofind;
    //GunType _guntofind;
    //string nameOfGunToFind;
    //public GameObject GunObj;

    //Animator gunanimator;

    //StemPlayerHandsCTRL _LISTERERERER;

    //private void OnEnable()
    //{

    //    StemKitMNGR.OnGunFlavorChanged += GunFlavorChanged;
 
    //}
    //private void OnDisable()
    //{
    //    StemKitMNGR.OnGunFlavorChanged -= GunFlavorChanged;
      
    //}
    void Start () {

        _meterCTRL = TheMeter.GetComponent<ReloadMeterCTRL>();
        //_guntofind = guntofind;


        //_LISTERERERER = GameObject.FindObjectOfType<StemPlayerHandsCTRL>();

    }
 

 


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {}

        if (Input.GetKeyDown(KeyCode.Alpha1)) {  }
        if (Input.GetKeyDown(KeyCode.Q)) {   }

       // if (Input.GetKeyDown(KeyCode.Alpha2)) { _meterCTRL.StartCell_1_Fill = true; }
        if (Input.GetKeyDown(KeyCode.W)) {  }

        //if (Input.GetKeyDown(KeyCode.Alpha3)) { _meterCTRL.StartCell_2_Fill = true; }
        if (Input.GetKeyDown(KeyCode.E)) {  }


        if (Input.GetKeyDown(KeyCode.Escape)) {   }
        //if (Input.GetKeyDown(KeyCode.Alpha4)) { SLIDEOUT(); }
        //if (Input.GetKeyDown(KeyCode.Alpha5)) { SLIDEIN(); }
        //if (Input.GetKeyDown(KeyCode.Alpha6)) { CLOSESLIDER(); }
        //if (Input.GetKeyDown(KeyCode.Alpha7)) { FIREFLAT(); }
        //if (Input.GetKeyDown(KeyCode.Alpha8)) { FIREROT(); }
        // if (Input.GetKeyDown(KeyCode.Alpha9)) { _LISTERERERER.ItShootsGun(); }
        // if (Input.GetKeyUp(KeyCode.Alpha9)) { _LISTERERERER.ItStopsShooting(); }

        //if (Input.GetKeyDown(KeyCode.Q)) { _LISTERERERER.ItPutsGunInHand(GunType.PISTOL); _LISTERERERER.ItPutsMagInHand(GunType.PISTOL); }
        //if (Input.GetKeyDown(KeyCode.W)) { _LISTERERERER.ItPutsGunInHand(GunType.MAGNUM); _LISTERERERER.ItPutsMagInHand(GunType.MAGNUM); }
        //if (Input.GetKeyDown(KeyCode.E)) { _LISTERERERER.ItPutsGunInHand(GunType.UZI); _LISTERERERER.ItPutsMagInHand(GunType.UZI); }
        //if (Input.GetKeyDown(KeyCode.R)) { _LISTERERERER.ItPutsGunInHand(GunType.SHOTGUN); _LISTERERERER.ItPutsMagInHand(GunType.SHOTGUN); }


    }




    //void IDLE() { gunanimator.Play("IDLE"); }
    //void HAMMERDOWN() { gunanimator.Play("HAMMERDOWN"); }
    //void OPENSLIDEER() { gunanimator.Play("OPENSLIDEER"); }
    //void SLIDEOUT() { gunanimator.Play("SLIDEOUT"); }
    //void SLIDEIN() { gunanimator.Play("SLIDEIN"); }
    //void CLOSESLIDER() { gunanimator.Play("CLOSESLIDER"); }
    //void FIREFLAT() { gunanimator.Play("FIREFLAT"); }
    //void FIREROT() { gunanimator.Play("FIREROT"); }
   
}
