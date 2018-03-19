using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHandScript : BaseHandScript
{

 
    public override void Equip_byEquipmentIndex(int argIndex)
    {
        // base.Equip_byEquipmentIndex(argIndex);
        base.MyBun.SetMyCurrBunThing(argIndex);
        base.MyBun.ShowMyCurrBunThing();
        base._AnmController.offhandAnimateHOldMag((Ammunition)argIndex);

    }

    public void Animate_OpenHand()
    {
        base._AnmController.DoOpenHandAnim();
    }

    public void AnimateHoldAmmo()
    {
        base. _AnmController.DoTrigHoldAmmo();
    }
    public void FixOffhandWeirdRoation()
    {
        if(GameSettings.Instance.IsRightHandedPlayer)
        this.transform.localRotation = Quaternion.Euler(0, 0, -90);
        else
            this.transform.localRotation = Quaternion.Euler(0, 0, 90);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) { FixOffhandWeirdRoation(); }
    }
    public override void RefHandleCollision(string ArgObjectTouched)
    {
        if (ArgObjectTouched == "Ammo")
        {
            StemKitMNGR.OffHandTouchedThisThing(ArgObjectTouched);
        }
        if (base.MyBun.IsMyThingShowing())
        {

            StemKitMNGR.OffHandTouchedThisThing(ArgObjectTouched);
        }
    } 
    
}
