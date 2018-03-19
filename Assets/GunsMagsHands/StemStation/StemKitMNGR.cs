// @Author Nabil Lamriben ©2018
//using SixenseCore;
using UnityEngine;

public class StemKitMNGR : MonoBehaviour {
    // .0028 .073 -.14
    Transform A_CtrlPos;
    Transform B_CtrlPos;
    Transform A_TrakPos;
    Transform B_TrakPos;

    GunsBundle MNGR_GunsBundle;
    MagsBundle MNGR_MagsBundle;

    GameObject MNGR_MAIN_Hand;
    GameObject MNGR_OFF_Hand;
    public bool useStem;



    IPlayerHandsCTRL _playerHandsController;
    StemHandsFactory _myHandsFactory;

    GunType GlobalDeleteMe_GunFlavor;
    Ammunition GlobalDeleteMe_ammoFlaver;

   // public bool IsStandAlone;


    #region Events
    //public delegate void STEMCONNECTED(TrackerID argid);
    //public static event STEMCONNECTED OnSTEMCONNECTED;
    //public static void StemObjectConnected(TrackerID argid)
    //{
    //    if (OnSTEMCONNECTED != null) OnSTEMCONNECTED(argid);
    //}

    //public delegate void STEMDISCONNECTED(TrackerID argid);
    //public static event STEMDISCONNECTED OnSTEM_DIS_CONNECTED;
    //public static void StemObject_dis_Connected(TrackerID argid)
    //{
    //    if (OnSTEM_DIS_CONNECTED != null) OnSTEM_DIS_CONNECTED(argid);
    //}



    public delegate void OffHandTouched(string argStr);
    public static event OffHandTouched OnOffhandTouchedSMTHN;
    public static void OffHandTouchedThisThing(string argStr)
    {
        if (OnOffhandTouchedSMTHN != null) OnOffhandTouchedSMTHN(argStr);
    }

    public delegate void OffHandTouchedGUNMAG();
    public static event OffHandTouchedGUNMAG OnOffhandTouchedGUNMAG;
    public static void CALLOffHandTouchedGUNMAG()
    {
        if (OnOffhandTouchedGUNMAG != null) OnOffhandTouchedGUNMAG();
    }


    public delegate void MAINHandTouched(string argStr);
    public static event MAINHandTouched OnMAINhandTouchedSMTHN;
    public static void MAINHandTouchedThisThing(string argStr)
    {
        if (OnMAINhandTouchedSMTHN != null) OnMAINhandTouchedSMTHN(argStr);
    }

    public delegate void UPDATECURIGUN(IGun argIgun);
    public static event UPDATECURIGUN OnUpdateCurIGun;
    public static void Call_SetCurIgunTo(IGun argIgun)
    {
        if (OnUpdateCurIGun != null) OnUpdateCurIGun(argIgun);
    }


    public delegate void EVENTACTIVERELRELOADENDED();
    public static event EVENTACTIVERELRELOADENDED OnEVENTACTIVERELRELOADENDED;
    public static void HEyActiveReloadEnds()
    {
        if (OnEVENTACTIVERELRELOADENDED != null) OnEVENTACTIVERELRELOADENDED();
    }


    
    
    public delegate void GunFlavorChanged(GunType argGunType);
    public static event GunFlavorChanged OnGunSetChanged;
    public static void Call_GunSetChangeTo(GunType argGunType)
    {
        if (OnGunSetChanged != null) OnGunSetChanged(argGunType);
    }


    //======================================================================================
    public delegate void EVENT_UIcell_FILLED(int id);
    public static event EVENT_UIcell_FILLED OnUICellFilled;
    public static void CALL_UICELLFilled(int id)
    {
        if (OnUICellFilled != null) OnUICellFilled(id);
    }

    public delegate void EVENT_START_UIcell(int id);
    public static event EVENT_START_UIcell On_START_Uicell;
    public static void CALL_Start_UIcell(int id)
    {
        if (On_START_Uicell != null) On_START_Uicell(id);
    }

   
    public delegate void EVENT_Override_UICELLID(int id);
    public static event EVENT_Override_UICELLID On_Override_UICellid;
    public static void Call_OVR_Cell_ID(int id)
    {
        if (On_Override_UICellid != null) On_Override_UICellid(id);
    }


    //======================================================================================


    //****************************************************************************
    // Use this for initialization
    //****************************************************************************


    public delegate void EVENTVIBRATE(int x, float zeroone);
    public static event EVENTVIBRATE OnVibrate;
    public static void CALL_VIBRATECONTROLLERG(int x, float zeroone)
    {
        if (OnVibrate != null) OnVibrate(x,zeroone);
    }


    public delegate void EVENTALLOWEDGUNINDEX(int x);
    public static event EVENTALLOWEDGUNINDEX OnUpdateAvailableGUnIndex;
    public static void CALL_UpdateAvailableGUnIndex(int x)
    {
        if (OnUpdateAvailableGUnIndex != null) OnUpdateAvailableGUnIndex(x);
    }

    public delegate void EVENTRESTGUNANDMETER();
    public static event EVENTRESTGUNANDMETER OnResetGunAndMeter;
    public static void CALL_ResetGunAndMeter()
    {
        if (OnResetGunAndMeter != null) OnResetGunAndMeter();
    }



    public delegate void EVENTTOGGOLESTEMINPUT(bool argonoff);
    public static event EVENTTOGGOLESTEMINPUT OnToggleInputs;
    public static void CALL_ToggleStemInput(bool argonoff)
    {
        if (OnToggleInputs != null) OnToggleInputs(argonoff);
    }


    public delegate void EVENTSTARTFINDINGTRACKERS(bool argonoff);
    public static event EVENTSTARTFINDINGTRACKERS OnStartFindingTrackers;
    public static void CALL_StartFindingTrackers(bool argonoff)
    {
        if (OnStartFindingTrackers != null) OnStartFindingTrackers(argonoff);
    }

    public delegate void EVENTALLOWEXTRABUTTONS(bool argonoff);
    public static event EVENTALLOWEXTRABUTTONS OnToggleONOFFExtraButtons;
    public static void CALL_ToggleAllowExtraButtons(bool argonoff)
    {
        if (OnToggleONOFFExtraButtons != null) OnToggleONOFFExtraButtons(argonoff);
    }

    #endregion



    private void Awake()
    {
        _myHandsFactory = GetComponent<StemHandsFactory>();
        _playerHandsController = GetComponent<IPlayerHandsCTRL>();
    }

    void Start()
    {
     
        InitHandsBundles();
        if (!useStem) {
            this.transform.parent = Camera.main.transform;
        }
    }

    void StandaloneInit() {
        GlobalDeleteMe_GunFlavor = GunType.SHOTGUN;
        GlobalDeleteMe_ammoFlaver = Ammunition.SHOTGUN;

        Find_StandaloneCTRLandTRKRPositions();
        FindBundles();


        SetupAlphaSide(true);

        _playerHandsController.InitPlayerHandsScripts(MNGR_MAIN_Hand.GetComponent<MainHandScript>(), MNGR_OFF_Hand.GetComponent<OffHandScript>());
   
    }
    void SetGun()
    {
        Call_GunSetChangeTo(GunType.MAGNUM);
        CALL_UpdateAvailableGUnIndex(3);
        CALL_ToggleAllowExtraButtons(true); 
        CALL_ToggleStemInput(true);
     }
    ////off
    //IEnumerator getgandsinafew() {
    //    yield return new WaitForSeconds(0.5f);
    //    _playerHandsController.ItPutsGunInHand(GlobalDeleteMe_GunFlavor);
    //    _playerHandsController.ItPutsMagInHand(GlobalDeleteMe_GunFlavor);


    //}


    void FullINit() {
        // Debug.Log("full init");
        GlobalDeleteMe_GunFlavor = GunType.MAGNUM;
        GlobalDeleteMe_ammoFlaver = Ammunition.MAGNUM;

       

        Find_StemTrackerandControllerPositions();
        FindBundles();


        if (GameSettings.Instance.GameMode == ARZGameModes.GameLeft)
        {
            Debug.Log("full initleftalpha");
            if (GameSettings.Instance.IsRightHandedPlayer)
            {

                SetupAlphaSide(true);
            }
            else
            {
                SetupAlphaSide(false);
            }
        }
        if (GameSettings.Instance.GameMode == ARZGameModes.GameRight)
        {
            Debug.Log("full init RightBravo");
            if (GameSettings.Instance.IsRightHandedPlayer)
            {
                SetupBravoSide(true);
            }
            else
            {
                SetupBravoSide(false);
            }
        }
        _playerHandsController.InitPlayerHandsScripts(MNGR_MAIN_Hand.GetComponent<MainHandScript>(), MNGR_OFF_Hand.GetComponent<OffHandScript>());
        //DissAllowPlayerFromUseingStemInputs();
        CALL_ToggleStemInput(false);

        if (GameManager.Instance == null)
        {
            SetGun();
        }
        // StartCoroutine(Wait1SecToPutGunINHand());
        // StartCoroutine(AllowSteminputsinafew());
    }


    void InitHandsBundles()
    {
        //if (IsStandAlone) { StandaloneInit(); }
        //else { FullINit(); }

        FullINit();
        CALL_StartFindingTrackers(true);
        
    }

    //****************************************************************************
    Transform DeepSearch(Transform parent, string val)
    {
        foreach (Transform c in parent)
        {
            if (c.name == val) { return c; }
            var result = DeepSearch(c, val);
            if (result != null)
                return result;
        }
        return null;
    }

    void Find_StemTrackerandControllerPositions() {
        A_CtrlPos = DeepSearch(this.transform, "AlphaCTRL_HandPos");
        if (A_CtrlPos==null) { Debug.Log("did not find hand pos" + "AlphaCTRL_HandPos"); }
        B_CtrlPos = DeepSearch(this.transform, "BravoCTRL_HandPos");
        if (B_CtrlPos == null) { Debug.Log("did not find hand pos" + "BravoCTRL_HandPos"); }


        A_TrakPos = DeepSearch(this.transform, "AlphaTRKL_HandPos");
        if (A_TrakPos == null) { Debug.Log("did not find hand pos" + "AlphaTRKL_HandPos"); }
        B_TrakPos = DeepSearch(this.transform, "BravoTRK_HandPos");
        if (B_TrakPos == null) { Debug.Log("did not find hand pos" + "BravoTRK_HandPos"); }
    }

    void Find_StandaloneCTRLandTRKRPositions()
    {
        A_CtrlPos = DeepSearch(this.transform, "AlphaBravoCTRL_HandPos");
        if (A_CtrlPos == null) { Debug.Log("did not find hand pos" + "AlphaCTRL_HandPos"); }
        B_CtrlPos = DeepSearch(this.transform, "AlphaBravoCTRL_HandPos");
        if (B_CtrlPos == null) { Debug.Log("did not find hand pos" + "BravoCTRL_HandPos"); }


        A_TrakPos = DeepSearch(this.transform, "AlphaBravoTRKL_HandPos");
        if (A_TrakPos == null) { Debug.Log("did not find hand pos" + "AlphaTRKL_HandPos"); }
        B_TrakPos = DeepSearch(this.transform, "AlphaBravoTRKL_HandPos");
        if (B_TrakPos == null) { Debug.Log("did not find hand pos" + "BravoTRK_HandPos"); }
    }

    void FindBundles() {

        MNGR_GunsBundle = GetComponentInChildren<GunsBundle>();
        MNGR_MagsBundle = GetComponentInChildren<MagsBundle>();

        if (MNGR_MagsBundle==null) { Debug.Log("no mags bundur found"); }
        if (MNGR_GunsBundle == null) { Debug.Log("no guns bundur found"); }

    }

    void SetupAlphaSide(bool argRighty)
    {
        SetupSide(argRighty, A_CtrlPos, A_TrakPos);
    }

    void SetupBravoSide(bool argRighty)
    {
        SetupSide(argRighty, B_CtrlPos, B_TrakPos);
    }

    void SetupSide(bool argRighty, Transform argCTRL,Transform argTRK) {

        MNGR_MAIN_Hand = _myHandsFactory.FactoryBuild_MainHand(argRighty, argCTRL, MNGR_GunsBundle);
        MNGR_OFF_Hand = _myHandsFactory.FactoryBuild_OffHand(argRighty, argTRK,MNGR_MagsBundle);

    }

}
