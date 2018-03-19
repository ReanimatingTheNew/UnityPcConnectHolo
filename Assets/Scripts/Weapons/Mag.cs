using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour,IMag {

    public GameObject MagBulletlet;

    UAudioManager audioManager;
    public int MagSize;

    public int MagBulletCount;

 
	void Awake () {
        MagBulletCount = MagSize;
        canplaycollisonsound = false;
        audioManager = GetComponent<UAudioManager>();
    }

   
    public bool TryDecrementBulletCount() {
        if (MagBulletCount <= 0 ) { return false; }
        else { MagBulletCount--; return true; }      
	}

    public GameObject GetBulletFromMag()
    {
        return this.MagBulletlet;
    }

    public int GetBulletsCount_inMag() { return MagBulletCount; }

    public void Refill()
    {
        Debug.Log("bullets refilled");
        MagBulletCount = MagSize;
    }


    bool canplaycollisonsound;
    public void InitCanPlayCollisionSound() { }

    private void OnCollisionEnter(Collision collision)
    {    
            if (collision.gameObject.tag == "SpatialMesh")
                audioManager.PlayEvent("Cling");
      

    }
}
