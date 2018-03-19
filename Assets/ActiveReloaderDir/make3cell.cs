using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class make3cell : MonoBehaviour {

    public GameObject MyactiveReloader;
    activereloadUIctrl scr;
    // Use this for initialization
    void Start()
    {

        //scr = MyactiveReloader.GetComponent<activereloadUIctrl>();


    }

    void poof()
    {
        GameObject go = Instantiate(MyactiveReloader, this.transform.position, this.transform.rotation);
        go.GetComponent<activereloadUIctrl>().SetStartCellIndex(1);

    }
    // Update is called once per frame
    void Update()
    {
      //  if (Input.GetKeyDown(KeyCode.Space)) { poof(); }
    }
}
