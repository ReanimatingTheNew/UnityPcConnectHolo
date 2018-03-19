using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimate : MonoBehaviour {
    Animator g_animator;
   
    private void Awake()
    {
        g_animator = gameObject.GetComponent<Animator>();
     }

    public void PlayFast() { g_animator.speed = 2.0f; }
    public void PlayFastest() { g_animator.speed = 2.0f; }
    public void PlayNormal() { g_animator.speed = 1.0f; }

    public void Gunimate_FIRE()
    {
        if (g_animator != null)
        {
           // g_animator.Play("FIREFLAT");
            g_animator.Play("FIREROT");
           
        }
        else
        {
            Debug.LogError("no animator for this gun");
        }
    }
    //**************************** move to gunAnimation.cs
    public void Gunimate_FIREFLAT()
    {
        if (g_animator != null)
        {
            //g_animator.Play("FIREFLAT");
            g_animator.Play("FIREROT");
        }
        else
        {
            Debug.LogError("no animator for this gun");
        }
    }
    public void Gunimate_FIREtrigger()
    {
        if (g_animator != null)
        {
            g_animator.SetTrigger("TrigFire");
        }
        else
        {
            Debug.LogError("no animator for this gun");
        }
    }

    public void Gunimate_OPENSLIDER() {
        g_animator.Play("OPENSLIDER"); // OPENSLIDER >then goes to> SLIDEOUT-> triggers OnSlideOutAnimComplete (animator takes care of this)
    }
    public void Gunimate_SLIDEIN()
    {
     //   g_animator.SetTrigger("TrigMagSlide"); // --> SLIDEIN -> CLOSESLIDER
        g_animator.Play("SLIDEIN"); // 
    }


    public void Gunimate_HAMMERDOWN() {
        g_animator.Play("HAMMERDOWN");

    }


}
