using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantScoreGrabber : MonoBehaviour {
    public static PersistantScoreGrabber Instance = null;
    private void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
          
            Instance = this;

        }
        else
            Destroy(gameObject);
    }

    Data_PlayerPoints CurPlayerPoints;

    //   UpdatePoints(points, headShotCount, streakCounter, maxHeadshotsinarow, killCount, missedshots, shotCount, deaths, pointslost,numberofReloads,numberofReloads);  
    void UpdatePoints(int ArgScore , int ArgHeadshots , int ArgStreakcount , int ArgMaxstreak ,  int ArgKills , int ArgMiss , int ArgTotalshots , int ArgDeaths, int ArgPointslost, int ArgReloads, int ArgWavesPlayed) {
        CurPlayerPoints = new Data_PlayerPoints();

        CurPlayerPoints.score = ArgScore;
        CurPlayerPoints.headshots = ArgHeadshots;
        CurPlayerPoints.streakcount = ArgStreakcount;
        CurPlayerPoints.maxstreak = ArgMaxstreak;
        CurPlayerPoints.kills = ArgKills;
        CurPlayerPoints.miss = ArgMiss;
        CurPlayerPoints.totalshots = ArgTotalshots;
        CurPlayerPoints.deaths = ArgDeaths;
        CurPlayerPoints.pointslost = ArgPointslost;
        CurPlayerPoints.numberofReloads = ArgReloads;
        CurPlayerPoints.wavessurvived = ArgWavesPlayed;

        Debug.Log("the scores are " + CurPlayerPoints.ToString());
    }

   public  void DoGrabScores() {
 
       

        int points = GameManager.Instance.GetScoreMAnager().Get_PointsTotal();
        int streakCounter = GameManager.Instance.GetStreakManager().Get_NumberOfStreaks();//STRAK sssssssssssssssssssssssssssssssssss
        int shotCount = GameManager.Instance.GetScoreMAnager().Get_BulletsFiredCNT();
        int headShotCount = GameManager.Instance.GetScoreMAnager().Get_headShotCNT();
        int torsoShotCount = GameManager.Instance.GetScoreMAnager().Get_torsoShotCNT();
        int limbShotCount = GameManager.Instance.GetScoreMAnager().Get_limbShotCNT();
        int killCount = GameManager.Instance.GetScoreMAnager().Get_ZombiesKilledCNT();
        int maxHeadshotsinarow = GameManager.Instance.GetStreakManager().Get_MaxHEadshotsInARow(); //is number of streak ssssssssssssssssssssssssssssssssssss  

        int missedshots = GameManager.Instance.GetScoreMAnager().Get_Bullets_Missed_ZombieCNT();
        int deaths = GameManager.Instance.GetScoreMAnager().Get_DeathsCNT();
        int pointslost = GameManager.Instance.GetScoreMAnager().Get_PointsTotalLost();
        int numberofReloads = GameManager.Instance.GetScoreMAnager().Get_ReloadsCNT();
        int wavessurvived = GameManager.Instance.GetScoreMAnager().Get_WavesPlayedCNT();


        UpdatePoints(points, headShotCount, streakCounter, maxHeadshotsinarow, killCount, missedshots, shotCount, deaths, pointslost,numberofReloads, wavessurvived);  
  
    }


    public List<DoubleVector> _ZombieHitLines;
    public List<DoubleVector> _ZombieMissLines;
    public void DoGrabLines() {
        _ZombieHitLines = GameManager.Instance.ZombieHitLines;
        _ZombieMissLines = GameManager.Instance.ZombieMissLines;
    }


    public Data_PlayerPoints Get_Data_Player() { return this.CurPlayerPoints; }


}
