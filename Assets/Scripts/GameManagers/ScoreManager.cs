using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    ShotScoreController _scoreCTRL;
    // Use this for initialization
    void Start () {
        _scoreCTRL = GetComponent<ShotScoreController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    int CNT_reloads = 0;
    //PersistantScoreGrabber.DoGrabScores()->
    public int Get_ReloadsCNT() { return CNT_reloads; }
    //Gun.GunHandle_CellFiled() when last cell is filled -> we call this via gamemanager._scoremanager
    public void Increment_ReloadsCNT() { CNT_reloads++; }


    int CNT_Deaths = 0;
    //PersistantScoreGrabber.DoGrabScores()->
    public int Get_DeathsCNT() { return CNT_Deaths; }
    //gamemnager.playerdied()->
    public void Increment_DeathsCNT() { CNT_Deaths++; }


    int CNT_WavesPlayed = 0;
    //PersistantScoreGrabber.DoGrabScores()->
    public int Get_WavesPlayedCNT() { return CNT_WavesPlayed; }
    //gamemanager.GM_Handle_WaveCompleteByPoppingNUMplusplus()->
    public void Increment_WavesPlayedCNT() { CNT_WavesPlayed++; }

    int CNT_BulletsFired = 0;
    int CNT_Bullet_Hit_Zombie = 0;
    int CNT_Bullets_Missed_Zombie = 0;
    //not used anymore untill we get pickups 
    //if we fired at a pickup or ammo box to get amo. this shot should not count 
    //public void SKORE_Decrement_ShotsFiredCounter() { if (CNT_BulletsFired > 0) CNT_BulletsFired--; }
    public int Get_BulletsFiredCNT() { return CNT_BulletsFired; }
    public int Get_Bullet_Hit_ZombieCNT() { return CNT_Bullet_Hit_Zombie; }
    public int Get_Bullets_Missed_ZombieCNT() { return CNT_Bullets_Missed_Zombie;  }
    public void Update_IncrementBulletsShotCNT() { CNT_BulletsFired++;   ScoreDebugCon.Instance.update_shotsfired(CNT_BulletsFired); }
    public void Update_IncrementBullet_Hit_ZombieCNT() { CNT_Bullet_Hit_Zombie++; ScoreDebugCon.Instance.update_hit(CNT_Bullet_Hit_Zombie); }
    public void Update_IncrementBullets_Missed_ZombieCNT() { CNT_Bullets_Missed_Zombie++; ScoreDebugCon.Instance.update_miss(CNT_Bullets_Missed_Zombie); }
 
    int CNT_ZombiesKilled = 0;
    // gamemanager.PlayerDied_GameManager()->
    public int Get_ZombiesKilledCNT() { return CNT_ZombiesKilled; }
    //gamemanager.KillEnemy_and_handle_streak
    public void Increment_ZombiesKilledCNT() {  CNT_ZombiesKilled++; }


    int SKORE_headShotCount = 0;
    public void Update_headShotCNT() { SKORE_headShotCount++; ScoreDebugCon.Instance.update_heads(SKORE_headShotCount); }
    public int Get_headShotCNT() { return SKORE_headShotCount; }


    int SKORE_torsoShotCount = 0;
    public void Update_torsoShotCNT() { SKORE_torsoShotCount++; ScoreDebugCon.Instance.update_torsos(SKORE_torsoShotCount); }
    public int Get_torsoShotCNT() { return SKORE_torsoShotCount; }

    int SKORE_limbShotCount = 0;
    public void Update_limbShotCNT() { SKORE_limbShotCount++; ScoreDebugCon.Instance.update_limb(SKORE_limbShotCount); }
    public int Get_limbShotCNT() { return SKORE_limbShotCount; }


    //not used in scores , but may be needed to generate more stats
    int CNT_Zombies_created = 0;
    public int Get_ZombiesCreatedCNT() { return CNT_Zombies_created; }
    public void Increment_ZombiesCreated( ){ CNT_Zombies_created++; }

    //0000000000000000000000000000000000000000000000000000000000000000000000
    int Points_CurWave = 0;
    // gamemanager.PlayerDied_GameManager()-> we get curwavepoints, and we 
    //1) show them on canvas
    //2) add them to  totalpoints lost
    //3) remove them from curr totalpoints 
    public int Get_PointsCurWave() { return Points_CurWave; }
    public void Update_Add_PointsCurWave(int argpointstoadd) { Points_CurWave += argpointstoadd; ScoreDebugCon.Instance.update_WAVEPoints(Points_CurWave); }
    public void Reset_WavePoints() { Points_CurWave = 0; }


    int Points_TotalLost = 0;
    //PersistantScoreGrabber.DoGrabScores()->
    public int Get_PointsTotalLost() { return Points_TotalLost; }
    //gamemanager.PlayerDied_GameManager()-> we get curwavepoints, and we   add them to  totalpoints lost
    public void Update_Add_PointsTotalLost(int arglostpoints) { Points_TotalLost += arglostpoints; }


    //do this last 
    int Points_Total = 0;
    //PersistantScoreGrabber.DoGrabScores() 
    // gamemanager.HardStop()
    // gamemanager.PlayerDied_GameManager() if time is up
    // SCOREboard.UpdateUIText()
    public int Get_PointsTotal() { return Points_Total; }
    public void Update_Add_PointsTotal(int argpointstoadd)
    {
        Points_Total += argpointstoadd;
    }
    public void Update_Remove_PointsTotal(int argpointstoadd)
    {
        Points_Total -= argpointstoadd;
    }

    int bonusPoints = 0;
    public int Get_BonusPoints() { return bonusPoints; }
    public void Update_BonusPoints(int argbonus) { bonusPoints+=argbonus; }

    //0000000000000000000000000000000000000000000000000000000000000000000000

    public void ResetScore()
    {
        CNT_Deaths = 0;
        CNT_WavesPlayed = 0;
        CNT_reloads = 0;

        CNT_BulletsFired = 0;
        CNT_Bullet_Hit_Zombie = 0;
        CNT_Bullets_Missed_Zombie = 0;

        CNT_ZombiesKilled = 0;
        CNT_Zombies_created = 0;

        Points_Total = 0;
        Points_CurWave = 0;
        Points_TotalLost = 0;

        SKORE_headShotCount = 0;
        SKORE_torsoShotCount = 0;
        SKORE_limbShotCount = 0;

    }

}
