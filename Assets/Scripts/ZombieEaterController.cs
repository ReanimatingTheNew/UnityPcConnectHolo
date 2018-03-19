// @Author Nabil Lamriben ©2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEaterController : MonoBehaviour {

    public GameObject Bloodobject;
    BloodFX bloodFX;
    Animator Zanim;
    public bool isEater;
    // Use this for initialization
    void Start() {


        bloodFX = Bloodobject.GetComponent<BloodFX>();
        Zanim = GetComponent<Animator>();
        if(!isEater)
        DisableBoxColliders(this.transform);

    }

    bool iswalking;
    // Update is called once per frame
    void Update() {
        if (iswalking)
        { // get rotation towards targetPoint
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
            Quaternion toRotation = transform.rotation;
            transform.rotation = oldRotation;

            // rotate towards targetPoint
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 500.0f * Time.deltaTime);
        }
    }


    Transform DisableBoxColliders(Transform argTrans)
    {
        if (argTrans.gameObject.GetComponent<BoxCollider>())
            argTrans.gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform c in argTrans)
        {
            var result = DisableBoxColliders(c);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    int hitcount=0;

    IEnumerator TriggerDeath() {
        yield return new WaitForSeconds(8);
        Zanim.SetTrigger("TrigDeath");
    }


    IEnumerator StartGameIn18()
    {
        yield return new WaitForSeconds(18);
        GameManager.Instance.CheckStartGame();
    }


    

    void TakeHit(Bullet b) {
        bloodFX.TorsoShotFX(b.hitInfo);
        playsplat.Instance.PlaySplatSound();
        Debug.Log("hit eater");
        Zanim.SetTrigger("TookHit");
        iswalking = true;
         hitcount ++;
        if (hitcount == 1) {// gameObject.AddComponent<TimerBehavior>(); 
            StartCoroutine(StartGameIn18());
            StartCoroutine(TriggerDeath());
            KillTimer t = gameObject.transform.parent.transform.parent.gameObject.AddComponent<KillTimer>();
            t.StartTimer(20);
        }



    }
}
