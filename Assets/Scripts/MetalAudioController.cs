using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalAudioController : MonoBehaviour {
    UAudioManager _uaudio;
    // Use this for initialization
    void Start () {
        _uaudio = GetComponent<UAudioManager>();
    }

    public void TakeHit(Bullet argBullet) {
        _uaudio.PlayEvent("_MetalDing");
    }
}
