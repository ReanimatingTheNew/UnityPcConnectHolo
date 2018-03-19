// @Author Nabil Lamriben ©2018
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReloadMeterCTRL : MonoBehaviour {
    #region OldNotouch
    public ReloadMeterState currentActiveCell;

    public Image Cell_0;
    public float cell0Time;
    public Image Cell_1;
    public float cell1Time;
    public Image Cell_2;
    public float cell2Time;
    public Text txt;
    public GameObject TheCanvas;

    float _CELLFILLTIMECOUNTER = 0.0f;
    float _extraTimeCounter = 0.0f;
    const float _allowedExtratime = 0.8f;

    UiCellObj[] allCELLOBJ;
    // SegmentBools[] allSegbools;

    const float minimum = 0.0F;
    const float maximum = 1.0F;
   
    //public bool IexistSaysMeter;
    //Levels/Waves can increas this meter's cell filltime
 

    void EnableCanvas() {
        TheCanvas.SetActive(true);
    }
    void DisableCanvas()
    {
        TheCanvas.SetActive(false);
    }

    void ResetCellObjsVAlues()
    {
        allCELLOBJ[0].ResetIgniteBool();
        allCELLOBJ[0].ResetMyTimerAndFill();

        allCELLOBJ[1].ResetIgniteBool();
        allCELLOBJ[1].ResetMyTimerAndFill();

        allCELLOBJ[2].ResetIgniteBool();
        allCELLOBJ[2].ResetMyTimerAndFill();
    }


    

    public bool showText;
    private void INIT_NewCellObjects()
    {

        allCELLOBJ = new UiCellObj[3];
        allCELLOBJ[0] = new UiCellObj(0,Cell_0, cell0Time, CellState.Empty);
        allCELLOBJ[1] = new UiCellObj(1,Cell_1, cell1Time, CellState.Empty);
        allCELLOBJ[2] = new UiCellObj(2,Cell_2, cell2Time, CellState.Empty);
         _CELLFILLTIMECOUNTER = 0.0f;
         _extraTimeCounter = 0.0f;
        ResetCellObjsVAlues();

    }
    #endregion


    public void ResetMeter()
    {
        currentActiveCell = ReloadMeterState.Sleeping; //int=6
        INIT_NewCellObjects();
        DisableCanvas();
    }
    private void Start()
    {
        ResetMeter();
    }

    #region listesAndHandlers
    private void OnEnable()
    {
        StemKitMNGR.On_START_Uicell += Handle_START_Cell;
        StemKitMNGR.On_Override_UICellid += HAndleOverriceCellid;
        StemKitMNGR.OnUICellFilled += Handle_CellFiled;
        StemKitMNGR.OnResetGunAndMeter += ResetMeter;
    }
    private void OnDisable()
    {
        StemKitMNGR.On_START_Uicell -= Handle_START_Cell;
        StemKitMNGR.On_Override_UICellid -= HAndleOverriceCellid;
        StemKitMNGR.OnUICellFilled -= Handle_CellFiled;
        StemKitMNGR.OnResetGunAndMeter -= ResetMeter;

    }
    void Handle_CellFiled(int id) {
        if (id == 0) { currentActiveCell = ReloadMeterState.Anim0; }
        else
            if (id == 1) { currentActiveCell = ReloadMeterState.Anim1; }
        else
            if (id == 2) { currentActiveCell = ReloadMeterState.Anim2; }

    }

    void Handle_START_Cell(int id)
    {
        EnableCanvas();

        if (id >2) {
           // StartCoroutine(WaitExtratime());
            return;
        }
        if (id ==-666) {
            StartCoroutine(WaitExtratime());

            return;
        }

        if (id == 0)
        {
            currentActiveCell = ReloadMeterState.SEGMENT_0;
        }
        else
        if (id == 1) currentActiveCell = ReloadMeterState.SEGMENT_1;
        else
        if (id == 2) currentActiveCell = ReloadMeterState.SEGMENT_2;

        allCELLOBJ[id].SetIgnite();
    }

    void HAndleOverriceCellid(int id) {
        if ((int)currentActiveCell == id)
            allCELLOBJ[id].FastForwardTimerTOmaketheFillStop();
        else
        if(id==-1) {
            if (currentActiveCell == ReloadMeterState.Sleeping || currentActiveCell == ReloadMeterState.SEGMENT_0)
            {
                Handle_START_Cell(0);
                HAndleOverriceCellid(0);
            }

        }
        else
            return;
    }

    IEnumerator WaitExtratime() {
        yield return new WaitForSeconds(0.2f);
        DisableCanvas();
        ResetCellObjsVAlues();
        currentActiveCell = ReloadMeterState.Sleeping;
        StemKitMNGR.CALL_UICELLFilled(-666); //to now have i can haz shooties again
    }
    #endregion

    private void Update()
    {
        if (GameSettings.Instance.ReloadDifficulty != ARZReloadLevel.EASY)
        {
            if (Input.GetKeyDown(KeyCode.I)) { StemKitMNGR.Call_OVR_Cell_ID(0); }
            if (Input.GetKeyDown(KeyCode.O)) { StemKitMNGR.Call_OVR_Cell_ID(1); }
            if (Input.GetKeyDown(KeyCode.P)) { StemKitMNGR.Call_OVR_Cell_ID(2); }
        }
        if (GameSettings.Instance.IsTestModeON)
        {
            if (showText)
                txt.text = currentActiveCell.ToString();
        }
            allCELLOBJ[0].FillWithTime();
            allCELLOBJ[1].FillWithTime();
            allCELLOBJ[2].FillWithTime();  
    }


}


public class UiCellObj {
    int CellID;
    Image _FgImage;
    float _cellTime;
    float _Tcounter;
    public CellState _cellState;
    Color NormalColor;
    Color SkippedToEndColor;
    bool IgniteBoolean;
    bool fastforwarded;
    public void SetIgnite() { IgniteBoolean = true; fastforwarded = false; }
    public void ResetIgniteBool() {
        IgniteBoolean = false;
        fastforwarded = false; //yes still falses
    }
    public UiCellObj(int argId,Image argImg, float argT, CellState ArgCellstate) {
        CellID = argId;
        _FgImage = argImg;
        _cellTime = argT;
        _cellState = ArgCellstate;
        NormalColor = Color.red;
        SkippedToEndColor = Color.green;
        IgniteBoolean = false;
        fastforwarded = false;
    }
    
    public void FillWithTime() {
        if (IgniteBoolean)
        {
            TimeFillForeGround();
            DetectForeGroundCompletelyFilled();
        }
    }

    void TimeFillForeGround() {
        _FgImage.color = NormalColor;
        if (fastforwarded)
        {
            _FgImage.color = SkippedToEndColor;
        }
        float factor = 1.0f / _cellTime;
        float v = Mathf.Lerp(0, 1, _Tcounter);
        _Tcounter += factor * Time.deltaTime;

        _FgImage.fillAmount = v;
    }
    void DetectForeGroundCompletelyFilled() {
        if (_Tcounter > 1.0f)
        {
            if (fastforwarded)
            {
                _FgImage.color = SkippedToEndColor;
            }
            SignalEndOfThisCellReached();
            ResetIgniteBool();
        }
    }

    void SignalEndOfThisCellReached() {
        StemKitMNGR.CALL_UICELLFilled(CellID);
    }
    //when i disable the canvas i should reset all times and hit
    public void ResetMyTimerAndFill() {
        _Tcounter = 0.0f;
        _FgImage.fillAmount =0;
    }
    public void FastForwardTimerTOmaketheFillStop()
    {
        fastforwarded = true;

        _Tcounter = 200.0f;
    }
}

