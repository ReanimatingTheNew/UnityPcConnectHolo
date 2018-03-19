using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadHandCollider : MonoBehaviour {

    private Material material;

    public BaseHandScript bhs;

    private void Start()
    {

        //Debug.Log("getting rendere0");
        if (!GameSettings.Instance.IsTestModeON) { GetComponent<Renderer>().enabled = false; }
        else
        {
            material = GetComponent<Renderer>().material;
        }

    }
    private void OnTriggerEnter(Collider other)
    {

      //  Debug.Log(" collision with " + other.gameObject.name);
        //   StemKitMNGR.OffHandTouchedThisThing("ar*"+other.gameObject.tag);
        bhs.RefHandleCollision(other.gameObject.tag);
    }
}
