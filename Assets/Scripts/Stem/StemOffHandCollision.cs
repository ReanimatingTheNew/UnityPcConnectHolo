using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemOffHandCollision : MonoBehaviour {
 

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("OFFtouch!! "+this.gameObject.name + "->OntrigEnt->" + other.gameObject.name);
       // StemKitManager.OffHandTouchedThisThing(other.gameObject.tag);
    }

  
    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(this.gameObject.name + " onTRigstayy " + other.gameObject.name);
    //}
}
