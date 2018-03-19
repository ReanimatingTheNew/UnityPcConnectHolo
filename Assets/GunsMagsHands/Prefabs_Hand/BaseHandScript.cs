using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandScript : MonoBehaviour {
    public Transform MyBundleBone;
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

    public void InitializedThisHand( IBundle argIBun)
    {
        
        MyBun = argIBun;
        _AnmController = this.gameObject.GetComponent<HandAnimatorCTRL>();

        if (Mytype == ARZHandType.HandGun)
        {
            _AnmController.SetHAndType(1);
            //   gameObject.AddComponent<MainHandScript>();
        }
        else { _AnmController.SetHAndType(2); }
    }
    public ARZHandType GetHandType() { return this.Mytype; }


    // tracking meter //stemplayerctrl.ItPutsGunInHand or maginhand - 
    public virtual void Equip_byEquipmentIndex(int argIndex)
    {
        Debug.Log("equipping by index in BASE");
    }

    public virtual void RefHandleCollision(string ArgObjectTouched)
    {
        
    }

}


