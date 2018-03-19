using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour {


    public Animator anim;
	void Start () {
        Debug.Log("anim tester is on " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {


        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    anim.Play("A_HammerDown");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    anim.Play("A_Fire");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    anim.Play("A_MagOut");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    anim.Play("A_MagEject");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    anim.Play("A_MagInsert");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    anim.Play("A_MagClose");
        //}

       
    }
}
