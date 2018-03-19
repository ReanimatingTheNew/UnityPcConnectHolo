// @Author Nabil Lamriben ©2018
using UnityEngine;

public class SliderBackCollider : MonoBehaviour {


    Color ColorONSuccess = Color.green;
    Color ColorONFail = Color.red;
    private Material material;

    public ReloadMNGR MyReloader;
    private Color originalColor;

    bool FontTriggered = false;
    public void Trig_Front(bool argbool) { FontTriggered = argbool; }

    private void Start()
    {     
        if (GameSettings.Instance == null) { GetComponent<Renderer>().enabled = false; return; }

        if (!GameSettings.Instance.IsTestModeON) { GetComponent<Renderer>().enabled = false; }
        else
        {
            material = GetComponent<Renderer>().material;
            originalColor = material.color;
        }
    }
 
    void Handle_LoadyHandCollision(Collider other) {

        if (other.gameObject.CompareTag("LoadyHand"))
        {
            MyReloader.Trig_Hammer();
            if (GameSettings.Instance.IsTestModeON)
            {
                material.color = ColorONSuccess;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.HARD) {
            if (FontTriggered)
            {
                Handle_LoadyHandCollision(other);
            }
        }
        else
            if (GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.MEDIUM) {
            //no need to check for prior FrontCollision
            Handle_LoadyHandCollision(other);
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
