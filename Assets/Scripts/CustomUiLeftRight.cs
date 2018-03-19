using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomUiLeftRight : MonoBehaviour {

    public TextMesh TmValue;
    public TextMesh TmLabel;
    // public even EVNT;   //EventHandler EVNTS;
    public EventSingleInt IntChangeEvent;



    public int ValMin = 0;
    public int ValMax = 5;
    public int StepSize = 1;

    private int _propVal;
    public int PropVal
    {
        get { return _propVal; }
        set {
            _propVal = value;
            DisplayCurPropVal();
            SpyonFloatChanege(value); //to help fife the event that gamesettings will register . the registration is done on the public prop in inspector
            
        }
    }

    //awake before start . start of the uiinitializer that will take the vals from glbal settings and override the onse set by user
    void Awake() {
        //todo: fetch from saved value in settings
        _propVal = ValMin;
        DisplayCurPropVal();

        if (IntChangeEvent == null)
            IntChangeEvent = new EventSingleInt();

        IntChangeEvent.AddListener(SpyonFloatChanege);
    }

    void SpyonFloatChanege(int argFloaChanged) {
        Debug.Log("SPY");
    }

    void DecrementValue()
    {
        if (_propVal > ValMin)
        {
            PropVal = _propVal - StepSize;
        }

    }

    void IncrementValue()
    {
        if (_propVal < ValMax)
        {
            PropVal = _propVal + StepSize;
        }
    }

    public void CallScreen(object o)
    {
        if (o is string)
        {
            if (o.ToString() == "LeftBox") { DecrementValue(); }
            else
                 if (o.ToString() == "RightBox") { IncrementValue(); }

        }
    }

    void DisplayCurPropVal() {
        SpyonFloatChanege(_propVal);
        TmValue.text = "[ "+ _propVal.ToString()+" ]";
    }
    public void InitialDisplayInit(int argSavedVAlueConvertedToint) {
        TmValue.text = "[ " + argSavedVAlueConvertedToint + " ]";
        _propVal = argSavedVAlueConvertedToint;
    }

}
