// @Author Nabil Lamriben ©2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SixenseCore;

public class StemSecurity : MonoBehaviour {
    protected SixenseCore.TrackerVisual Alpha_trackerVisual;
    protected SixenseCore.TrackerVisual Bravo_trackerVisual;
    // Use this for initialization

    public GameObject TRACKED_TrackerAlpha;
    public GameObject TRACKED_TrackerBravo;
    void Start () {
        Alpha_trackerVisual = TRACKED_TrackerAlpha.GetComponentInChildren<SixenseCore.TrackerVisual>();
        Bravo_trackerVisual = TRACKED_TrackerBravo.GetComponentInChildren<SixenseCore.TrackerVisual>();
    }

    // Update is called once per frame
    void Update () {
        Locate();

    }


    void Locate( )
    {
        if (GameSettings.Instance.IsSecurityOn)
        {
            if (Alpha_trackerVisual.HasInput)
            {

                var id = Alpha_trackerVisual.m_trackerBind;
                var pos = Alpha_trackerVisual.m_sensor.position;
                if (Vector3.Distance(Vector3.zero, pos) > 2)
                {
                    Alpha_trackerVisual.Input.SetVibration(1);
                    print("come back ALPHA dude");
                }
                else { Alpha_trackerVisual.Input.SetVibration(0); }
            }
            if (Bravo_trackerVisual.HasInput)
            {

                var id = Bravo_trackerVisual.m_trackerBind;
                var pos = Bravo_trackerVisual.m_sensor.position;


                if (Vector3.Distance(Vector3.zero, pos) > 2)
                {
                    Bravo_trackerVisual.Input.SetVibration(1);
                    print("come back BRAVO dude");
                }
                else
                {
                    Bravo_trackerVisual.Input.SetVibration(0);
                }
            }
        }
      
    

    }



}
