using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMNGR : MonoBehaviour {

    // manages corrcet hand movements for reloading 

    bool MagColliderTriggered = false;
    bool FontTriggered = false;
    bool BackTriggered = false;




    public void Trig_Mag() {
        
            StemKitMNGR.CALLOffHandTouchedGUNMAG(); Debug.Log("HandTouchedMagCollider");
        
    }
    
    public void Trig_Hammer() {
       
            StemKitMNGR.Call_OVR_Cell_ID(2); Debug.Log("HAMMERit");
       
    }


  
}
