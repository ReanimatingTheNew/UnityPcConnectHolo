using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles
public class gunhelper : MonoBehaviour {
#pragma warning restore IDE1006 // Naming Styles
    //public GameObject Mag;
    //public GameObject Mag1;
    //public GameObject Mag2;
    //public GameObject Mag3;
    //public GameObject MagCLip;

    //GameObject PlacedMag;
    //GameObject PlacedMag1;
    //GameObject PlacedMag2;
    //GameObject PlacedMag3;
#pragma warning disable IDE1006 // Naming Styles
    void putClipin() {
#pragma warning restore IDE1006 // Naming Styles
        //Destroy(PlacedMag);
        //PlacedMag = Instantiate(Mag, MagCLip.transform.position, MagCLip.transform.rotation);
        //PlacedMag1 = Instantiate(Mag1, MagCLip.transform.position, MagCLip.transform.rotation);
        //PlacedMag2 = Instantiate(Mag2, MagCLip.transform.position, MagCLip.transform.rotation);
        //PlacedMag3 = Instantiate(Mag3, MagCLip.transform.position, MagCLip.transform.rotation);
        //PlacedMag.transform.parent = MagCLip.transform;
        //PlacedMag1.transform.parent = MagCLip.transform;
        //PlacedMag2.transform.parent = MagCLip.transform;
        //PlacedMag3.transform.parent = MagCLip.transform;
    }

    void EjectMag() {
        thegun.MANUAL_EJECT_MAG_OUT();


        thegun.AUDIO_PopMagOut();
        

        //    PlacedMag.transform.parent = null;
        //   GameObject ejectedMAg = PlacedMag;
        ////     DestroyObject(PlacedMag);
        //    ejectedMAg.AddComponent<Rigidbody>();
        //    ejectedMAg.GetComponent<Rigidbody>().AddForce(MagCLip.transform.forward * -2, ForceMode.Impulse);

    }


    void InjectMag()
    {
        thegun.GunInjstantiateMagANDSLIDEINanim();
        thegun.AUDIO_PopMagIn();
        //    PlacedMag.transform.parent = null;
        //   GameObject ejectedMAg = PlacedMag;
        ////     DestroyObject(PlacedMag);
        //    ejectedMAg.AddComponent<Rigidbody>();
        //    ejectedMAg.GetComponent<Rigidbody>().AddForce(MagCLip.transform.forward * -2, ForceMode.Impulse);

    }


    void Shoot()
    {
        thegun.GUN_FIRE();
        //    PlacedMag.transform.parent = null;
        //   GameObject ejectedMAg = PlacedMag;
        ////     DestroyObject(PlacedMag);
        //    ejectedMAg.AddComponent<Rigidbody>();
        //    ejectedMAg.GetComponent<Rigidbody>().AddForce(MagCLip.transform.forward * -2, ForceMode.Impulse);

    }

    void StopShooting() {
        thegun.GUN_STOP_FIRE();

    }
    Gun thegun;
    // Use this for initialization
    void Start () {
        // putClipin();
        thegun = GetComponent<Gun>();



    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) { EjectMag(); }
        if (Input.GetKeyDown(KeyCode.R)) { InjectMag(); thegun.MagicReloadBulletCount(); }
        if (Input.GetKeyDown(KeyCode.Space)) { Shoot(); }
        if (Input.GetKeyUp(KeyCode.Space)) { StopShooting(); }
    }



    public static void DrawStaticLaserPointer(Vector3 A, Vector3 B)
    {
        Debug.Log("Line drawn");
        GameObject lineObj = new GameObject("DragLine", typeof(LineRenderer));
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        line.SetWidth(0.005f, 0.001f);
        //  Material whiteDiffuseMat = new Material(Shader.Find("Mobile/Particles/Additive"));
        Material whiteDiffuseMat = new Material(Shader.Find("Standard"))
        {
            color = Color.red
        };
        line.material = whiteDiffuseMat;
        //line.material.color = Color.red;
        line.SetVertexCount(2);
        line.SetPosition(0, A);
        line.SetPosition(1, B);
        //   line.SetColors(Color.red, Color.red);
        lineObj.AddComponent<KillTimer>().StartTimer(0.02f);


    }

    public static Transform DeepSearch(Transform parent, string val)
    {
        // Debug.Log("on " + parent.gameObject.name);

        foreach (Transform c in parent)
        {
            if (c.name == val) { return c; }
            var result = DeepSearch(c, val);
            if (result != null)
                return result;
        }
        return null;
    }

    public static Transform DeepSearchContain(Transform parent, string val)
    {
        // Debug.Log("on " + parent.gameObject.name);

        foreach (Transform c in parent)
        {
            if (c.name.Contains(val)) { return c; }
            var result = DeepSearchContain(c, val);
            if (result != null)
                return result;
        }
        return null;
    }

}
