using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInitializer : MonoBehaviour {

    public CustomUiLeftRight VizGridCLR;
    public CustomUiLeftRight GameTimeCLR;
    public CustomUiLeftRight InfAmmoCLR;
    public CustomUiLeftRight PowUpCLR;
    public CustomUiLeftRight SegDistCLR;

    IEnumerator Startin2seconds() {
        yield return new WaitForSeconds(2);
        Debug.Log("did it work?");
    }

    void Start () {
        //StartCoroutine(Startin2seconds());


        //VizGridCLR.InitialDisplayInit(GlobalSettings.Instance.IsVisibleGridPoints == false ? 0 : 1);
        //GameTimeCLR.InitialDisplayInit(GlobalSettings.Instance.GameDuration);
        //InfAmmoCLR.InitialDisplayInit(GlobalSettings.Instance.IsInfinitAmmo == false ? 0 : 1);
        //PowUpCLR.InitialDisplayInit(GlobalSettings.Instance.IsPowerUp == false ? 0 : 1);
        //SegDistCLR.InitialDisplayInit(ConvertSegDistToINt(GlobalSettings.Instance.SegmentDistance));
    }

    int ConvertSegDistToINt(float argStoredSegDist) {

        int SegmentDistance = 0;
        //     case 0.25f  - >
        if ( argStoredSegDist < 0.26f) return 0;
        // 0.35f->1
        if (argStoredSegDist > 0.26f && argStoredSegDist < 0.36f) return 1;

        //0.4f->2
        if (argStoredSegDist > 0.36f && argStoredSegDist < 0.46f) return 2;

        //0.5f->3
        if (argStoredSegDist > 0.46f && argStoredSegDist < 0.56f) return 3;

        //1.0f ->4
        if (argStoredSegDist > 0.56f && argStoredSegDist < 1.1f) return 4;


        //1.25f -5
        if (argStoredSegDist > 1.1f ) return 5;



        return SegmentDistance;

    }


}
