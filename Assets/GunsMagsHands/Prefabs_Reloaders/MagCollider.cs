using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagCollider : MonoBehaviour {


     Color ColorONSuccess = Color.green;
    Color ColorONFail = Color.red;
    private Material material;

    public ReloadMNGR MyReloader;
    private Color originalColor;

    private void Start()
    {


        //if (!GameSettings.Instance.IsTestModeON && GameSettings.Instance.IsActiveReload) { GetComponent<Renderer>().enabled = false; }
        //else
        //{
        //    material = GetComponent<Renderer>().material;
        //    originalColor = material.color;
        //}
        if (GameSettings.Instance == null) { GetComponent<Renderer>().enabled = false; return; }

        if (!GameSettings.Instance.IsTestModeON) { GetComponent<Renderer>().enabled = false; }
        else
        {
            material = GetComponent<Renderer>().material;
            originalColor = material.color;
        }

    }

      
	 

    private void OnTriggerEnter(Collider other)
    {
     
         if (other.gameObject.CompareTag("LoadyHand") )
         {
            MyReloader.Trig_Mag();
            if (GameSettings.Instance.IsTestModeON)
            {
                material.color = ColorONSuccess;
            }
        }
      

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LoadyHand"))
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                material.color = originalColor;
            }
  
        }
        

    }
}
