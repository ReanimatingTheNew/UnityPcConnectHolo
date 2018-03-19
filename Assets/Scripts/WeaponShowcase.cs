using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShowcase : MonoBehaviour {
    public GameObject pistol;
    public GameObject Colt;
    public GameObject Mac11;

    void HIdeAll() { pistol.SetActive(false); Colt.SetActive(false); Mac11.SetActive(false); }
    void Show_Pistole() { pistol.SetActive(true); Colt.SetActive(false); Mac11.SetActive(false); }
    void Show_Colt() { pistol.SetActive(false); Colt.SetActive(true); Mac11.SetActive(false); }
    void Show_Mac11() { pistol.SetActive(false); Colt.SetActive(false); Mac11.SetActive(true); }
    // Use this for initialization
    void Start () {
        HIdeAll();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Show_Pistole(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Show_Colt(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Show_Mac11(); }
        Rortateme();
    }


    void Rortateme()
    {        // Rotate the object around its local X axis at 1 degree per second
        //transform.Rotate(Time.deltaTime, 0, 0);

        // ...also rotate around the World's Y axis
        transform.Rotate(0, Time.deltaTime*50, 0, Space.World);
    }
}
