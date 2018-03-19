// @Author Nabil Lamriben ©2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBundle  {

    // Use this for initialization
    bool IsMyThingShowing();
    void HideAllMyThings();
    void SetMyCurrBunThing(int argIndexEnum);
    void ShowMyCurrBunThing();
    void HideMyCurrBunThing();
}
