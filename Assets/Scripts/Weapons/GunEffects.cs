using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffects : MonoBehaviour {

    public FlashScript myflash;
    public CasingEjector Ejector;

    public void FlashEffect() { myflash.Flash(); _uaudio.PlayEvent("_Fire"); StemKitMNGR.CALL_VIBRATECONTROLLERG(100, 1f); }

    public void CasingEjectEffect() { Ejector.EjectCasing(); }

 

    public void AUDIO_PopMagOut() { _uaudio.PlayEvent("_MagOut"); }
    public void AUDIO_PushMagIn() { _uaudio.PlayEvent("_MagIn"); }
    public void AUDIO_Chamber() { _uaudio.PlayEvent("_Hammer"); StemKitMNGR.CALL_VIBRATECONTROLLERG(200, 1f); }
    public void AUDIO_Dry() { _uaudio.PlayEvent("_Dry"); }
    public void AUDIO_FullReload() { _uaudio.PlayEvent("_FullReload"); StemKitMNGR.CALL_VIBRATECONTROLLERG(500, 0.5f); }

    UAudioManager _uaudio;
    // Use this for initialization
    void Start () {
        laserOn = false;
        _uaudio = GetComponent<UAudioManager>();
        InitBarelTrans();
    }

    public Transform barrelTran;


    void InitBarelTrans()
    {
        barrelTran = gunhelper.DeepSearch(this.transform, "BarrelObj");
        if (barrelTran == null)
        {
            Debug.LogError("barel not found ");
            barrelTran = this.transform;
        }
    }

  // public void initBareltrans(Transform ArgbarrelTran) { Transform barrelTran = ArgbarrelTran; }

    void doLaserfrom() { gunhelper.DrawStaticLaserPointer(barrelTran.transform.position, barrelTran.transform.position + (barrelTran.transform.forward * - 8 )); }


    bool laserOn;
    public void SetToggleLazer() { laserOn = !laserOn; }

	// Update is called once per frame
	void Update () {
        if (laserOn) { doLaserfrom(); }
    }
}
