using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingScript : MonoBehaviour {

    //fires when setactive true
    private void OnEnable()
    {
        transform.GetComponent<Rigidbody>().WakeUp();
        Invoke("HideCasing", 5.0f);
    }
    //fires when setactive false

    private void OnDisable()
    {
        transform.GetComponent<Rigidbody>().Sleep();
        CancelInvoke(); //so that hidecasin will not run 2wice
    }

    void HideCasing() {
        this.gameObject.SetActive(false);
    }
    UAudioManager audioManager;
    // Use this for initialization
    public float killDelay = 5.0f;
    void Start () {
        audioManager = GetComponent<UAudioManager>();
       // KillTimer t = this.gameObject.AddComponent<KillTimer>();
        //t.StartTimer(killDelay);
    }
	
	 
    private void OnCollisionEnter(Collision collision)
    {
//        Debug.Log("collision with " + collision.gameObject.name);
       if(collision.gameObject.tag=="SpatialMesh")
        audioManager.PlayEvent("Cling");
    }
}
