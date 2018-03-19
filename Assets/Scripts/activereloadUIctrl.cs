using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activereloadUIctrl : MonoBehaviour {


    public Image Cell_0;
    public float cell0Time;

    public Image Cell_1;
    public float cell1Time;

    public Image Cell_2;
    public float cell2Time;

    float t0 = 0.0f;
    int indexOfActiveCell ;
    Image[] allcells;
    float[] cellTimes;
    bool all3celsfinished = false;



    void Awake () {
        allcells = new Image[3];
        allcells[0] = Cell_0;
        allcells[1] = Cell_1;
        allcells[2] = Cell_2;


        cellTimes = new float[3];
        cellTimes[0] =cell0Time;
        cellTimes[1] = cell1Time;
        cellTimes[2] = cell2Time;

        indexOfActiveCell = 0;
        startIndexWasSet =false;
    }

    bool startIndexWasSet;
    public void SetAllCellTimes(float f1, float f2, float f3) {
        cellTimes[0] = f1;
        cellTimes[1] = f2;
        cellTimes[2] = f3;
    }
    public void SetStartCellIndex(int argStartIndex) {
        if (argStartIndex >= allcells.Length || argStartIndex < 0)
        {
            indexOfActiveCell = 0;

        }
        else
        {
            indexOfActiveCell = argStartIndex;
        }
        startIndexWasSet = true;
    }

    public void STARTmeter() { SetStartCellIndex(0); }

    public void SkipCell_0() { }
    void FillCell_0() { }
    public void SkipCell_1() { }
    void FillCell_1() { }
    public void SkipCell_2() { }
    void FillCell_2AndFinish() { }

    // Update is called once per frame
    void Update () {
        testandrun();
    }
    void testandrun() {
        if (startIndexWasSet)
        {
            if (!all3celsfinished)
            {
                DrainInSeconds(0.5f, indexOfActiveCell);
            }
            else {
                StemKitMNGR.HEyActiveReloadEnds();
                Destroy(this.gameObject);
            }
                
        }
    }
   
    public float minimum = 0.0F;
    public float maximum = 1.0F;

    
 

    void DrainInSeconds(float seconds,int argindex) {
        
        if (argindex >= allcells.Length) { all3celsfinished = true; return; }
       
        float factor = 1.0f / cellTimes[argindex];

        float v= Mathf.Lerp(minimum, maximum, t0);
        t0 += factor * Time.deltaTime;

       
        allcells[argindex].fillAmount = v;

        if (t0 > 1.0f)
        {
            indexOfActiveCell++;

            t0 = 0.0f;
        }
    }
}
