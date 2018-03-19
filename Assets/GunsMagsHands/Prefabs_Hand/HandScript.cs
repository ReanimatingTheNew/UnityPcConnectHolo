using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    public Transform MyGunGrip;
    public Transform MyMagazinGrip;


    //----------------------------------------------InitializedThisHand

    //**************************************
    //    Gun or Mag bundel
    //*************************************
    public IBundle MyBun;

    //**************************************
    //     Ainimatior for each hand object
    //*************************************
    public HandAnimatorCTRL _AnmController;

    //**************************************
    //     handtype each hand object
    //*************************************
    public ARZHandType Mytype;
    //----------------------------------------------X InitializedThisHand


    //stemkitmanager start
    public void InitializedThisHand(ARZHandType argType, IBundle argIBun) {
        Mytype = argType;
        MyBun = argIBun;
        _AnmController = this.gameObject.GetComponent<HandAnimatorCTRL>();

        if (argType== ARZHandType.HandGun) {
            _AnmController.SetHAndType(1);
         //   gameObject.AddComponent<MainHandScript>();
        } else { _AnmController.SetHAndType(2); }
        
    }

    public ARZHandType GetHandType() { return this.Mytype; }


    // tracking meter //stemplayerctrl.ItPutsGunInHand or maginhand - 
    public void EquipAnyHand_ByEquipmentIndex(int intenum) {
        MyBun.SetMyCurrBunThing(intenum);
        MyBun.ShowMyCurrBunThing();

        if (Mytype == ARZHandType.HandGun) _AnmController.MainHandAnimateHoldGun((GunType)intenum);
        else
            _AnmController.offhandAnimateHOldMag((Ammunition)intenum);

    }

    public void OFFHANDopenAnim()
    {
        _AnmController.DoOpenHandAnim();
    }

    public void OFFHandDoTrigHoldAmmo() {
        _AnmController.DoTrigHoldAmmo();
    }


    //**************************************************************
    //  collisions raise static events Mainhandtouched blah and offhandtouched blah
    //**************************************************************
    private void OnTriggerEnter(Collider other)
    {
         // Debug.Log(" collision "+ this.gameObject.name + "->OntrigEnt-> " + other.gameObject.name);

        if (Mytype == ARZHandType.HandGun){
             StemKitMNGR.MAINHandTouchedThisThing(other.gameObject.tag);
        }
        else
        if (Mytype == ARZHandType.HandMag) {
            if (other.gameObject.tag == "Ammo") {
                StemKitMNGR.OffHandTouchedThisThing(other.gameObject.tag);
            }
            if (MyBun.IsMyThingShowing()) {
                
                StemKitMNGR.OffHandTouchedThisThing(other.gameObject.tag);
             }
           
           }
        else
            Debug.Log(" collision but no type set" );

    }
  
}

