// @Author Nabil Lamriben ©2018s
using UnityEngine;

public class StreaksManager : MonoBehaviour {
    #region dependencies
    public GameObject StreakObject;
    public GameObject HeadshotEffectObject;
    #endregion

    #region PrivateVariables
    int points =0;
    bool lastShot_wasHEadHit;

    int curStreakLength;
    int MaxRecordedStreakLength;

    int numberofStreaks;
    int BonusToAwardForThisStreakHit;
    #endregion

    #region INIT
    void Start()
    {
        lastShot_wasHEadHit = false;
        curStreakLength = 0;
        MaxRecordedStreakLength = 0;
        numberofStreaks = 0;
        BonusToAwardForThisStreakHit = 0;
    }
    #endregion

    #region PublicMethods
    public void Set_StreakBreake()
    {
        if (curStreakLength>0) numberofStreaks++;
        curStreakLength = 0;
        lastShot_wasHEadHit = false;
        BonusToAwardForThisStreakHit = 0;
        ScoreDebugCon.Instance.update_wasHead(lastShot_wasHEadHit);
        ScoreDebugCon.Instance.update_CNTStrek(numberofStreaks);
    }

    public void Test_Streak(Vector3 here) {
        if (lastShot_wasHEadHit)
        {
            curStreakLength++;
            
            if (curStreakLength > MaxRecordedStreakLength) MaxRecordedStreakLength = curStreakLength;

            BonusToAwardForThisStreakHit += 25;
            GameObject so = Instantiate(StreakObject, here, Quaternion.identity);
            points = BonusToAwardForThisStreakHit;// + ((GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.EASY) ? 100 : 125);
            so.GetComponent<StreakText>().SetTextbox("+ " + (points + ((GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.EASY) ? 100 : 125)));
            KillTimer t = so.AddComponent<KillTimer>();
            t.StartTimer(2);
        }
        else
        {
            
            BonusToAwardForThisStreakHit += 0;
            GameObject so = Instantiate(HeadshotEffectObject, here, Quaternion.identity);
            points = BonusToAwardForThisStreakHit;// + ((GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.EASY) ? 100 : 125);
            so.GetComponent<StreakText>().SetTextbox("+ " + ( points+ ((GameSettings.Instance.ReloadDifficulty == ARZReloadLevel.EASY) ? 100 : 125)));
            KillTimer t = so.AddComponent<KillTimer>();
            t.StartTimer(2);
        }

        lastShot_wasHEadHit = true;
        GameManager.Instance.GetScoreMAnager().Update_Add_PointsTotal(points);
        GameManager.Instance.GetScoreMAnager().Update_Add_PointsCurWave(points);
        GameManager.Instance.GetScoreMAnager().Update_BonusPoints(BonusToAwardForThisStreakHit);


        ScoreDebugCon.Instance.update_wasHead(lastShot_wasHEadHit);
        ScoreDebugCon.Instance.update_curStrek(curStreakLength);
        ScoreDebugCon.Instance.update_MaxStrek(MaxRecordedStreakLength);
    }

    public int Get_MaxHEadshotsInARow() { return MaxRecordedStreakLength; }

    public int Get_NumberOfStreaks() { return numberofStreaks; }
    #endregion
}
