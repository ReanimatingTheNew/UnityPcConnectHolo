// @Author Nabil Lamriben ©2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandScript : BaseHandScript {

    public override void Equip_byEquipmentIndex(int argIndex)
    {
        // base.Equip_byEquipmentIndex(argIndex);
        base.MyBun.SetMyCurrBunThing(argIndex);
        base.MyBun.ShowMyCurrBunThing();
        base._AnmController.MainHandAnimateHoldGun((GunType)argIndex);
    }

    public override void RefHandleCollision(string ArgObjectTouched)
    {
        //should be the tag of the object
        StemKitMNGR.MAINHandTouchedThisThing(ArgObjectTouched);
    }

}
