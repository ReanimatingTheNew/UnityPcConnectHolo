// @Author Jeffrey M. Paquette ©2016
// @Author Nabil Lamriben ©2017

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This standard wave spawns zombies for a certain amount of time, keeping no more
 * than x number of zombies on screen at any time. It uses y percent of all
 * spawn points. Bonus points are awarded for destroying more zombies than the alloted par
 */
public class WaveStandard : MonoBehaviour, IWave {

    #region Public_props
    [Tooltip("Total time this wave will be active.")]
   // public float WaveTimerInMinutes;
    public float WavetimeinSeconds;

    [Tooltip("Number of living zombies allowed at any one time.")]
    public int maxZombiesOnScreen;

    [Tooltip("Percentage of spawn points used. Range 0.0 - 1.0")]
    public float percentageOfSpawns;

    [Tooltip("Buffer before the same spawn can respawn a zombie")]
    public float CoolDownTime = 2.0f;

    [Tooltip("The choice of zombies to spawn from.")]
    public GameObject[] zombies;


    [Tooltip("The choice of Special Zombies.")]
    public GameObject[] specialZombies;

    [Tooltip("Set to true if we want this wave to spawn special zombies.")]
    public bool IsspecialZombiesWave=false;

  
    public int SpecialZombieExtraHitPoints = 60;

    [Tooltip("Hitpoint range of zombies this wave.")]
    public int minZombieHP, maxZombieHP;

    public GunType WaveGun;
    #endregion


    #region Private_props

    private GameManager manager;                    // reference to game manager
    private List<SpawnPoint> _placeSpawnPoints;     // reference to all spawn points used by this wave

    //Will get set to true while spawning a special z and back to false when done spawning
    private bool _isSpawningSpetialZombie = false;

    private int maxSpawnPointIndex;                 // the index of the closest spawn used by this wave
    private int CurCountOfZOmbiesOnScreen = 0;      // number of zombies living
    
    private bool waveTimeisUp = false;              // is the wave over?

    private bool OnCompleteNotCalledyet = true;


    #endregion



    //darren brown 

    void Start ()
    {
        manager = GameManager.Instance;

        InitSpawnPoints();
        _isSpawningSpetialZombie = false;

      //debig  Debug.Log("_isSpawningSpetialZombie " + _isSpawningSpetialZombie);
    }


    void Update() {

        if (OnCompleteNotCalledyet && GameManager.Instance.curgamestate == ARZState.WaveEnded && CurCountOfZOmbiesOnScreen <= 0) {
            WaveSTD_Completed_callGM_Handle_Pop_newPlusPLus();
            OnCompleteNotCalledyet = false;
        }

    }
 

    #region PublicMEthods

    //this was on a timer 5sec in wm the first time
    public void StartWave()
    {
        //   Debug.Log("3 waveSTD StartWave");
        GameManager.Instance.curgamestate = ARZState.WavePlay;
        StartCoroutine(ie_StartWaveTimer());
        FillAllSpawnpoints_onlyonStart();
      //  print(WaveGun.ToString() + " <--mu gun");
        StemKitMNGR.Call_GunSetChangeTo(WaveGun);
        StemKitMNGR.CALL_UpdateAvailableGUnIndex((int)WaveGun);
        StemKitMNGR.CALL_ResetGunAndMeter();
        StemKitMNGR.CALL_ToggleStemInput(true);


    }

    public void WaveSTD_Completed_callGM_Handle_Pop_newPlusPLus()
    {
        ResetSpawnStates();
        // tell the game manager that wave is completed
        manager.GM_Handle_WaveCompleteByPoppingNUMplusplus();
    }

    public void ResetSpawnStates() {
        _isSpawningSpetialZombie = false;

        if (_placeSpawnPoints.Count > 0) {
            for (int i = 0; i < _placeSpawnPoints.Count; i++)
            {
                _placeSpawnPoints[i].ResetMe();
            }
        }
    }

    public void StopSpawnStates()
    {
        _isSpawningSpetialZombie = false;

        if (_placeSpawnPoints.Count > 0)
        {
            for (int i = 0; i < _placeSpawnPoints.Count; i++)
            {
                _placeSpawnPoints[i].StopMe();
            }
        }
    }

    //called from Spawnpoint when cooldown is over
    public void RequestAvailableSP()
    {
        //   Debug.Log("Spawnpoit finished coolingdown and is asking if it's time to spwan a z?");
        Wave_isittime_and_canI_spawnNow();
    }

    public void Wave_isittime_and_canI_spawnNow()
    {
        if (GameManager.Instance.curgamestate == ARZState.EndGame ||
            GameManager.Instance.curgamestate == ARZState.WaveBuffer ||
            GameManager.Instance.curgamestate == ARZState.WaveOverTime) return;

        if (CurCountOfZOmbiesOnScreen >= maxZombiesOnScreen || waveTimeisUp || GameManager.Instance.isDead)
            return;
        List<SpawnPoint> AvailableSpawnPointsNotCoolingDown = new List<SpawnPoint>();

        foreach (SpawnPoint spawnPoint in _placeSpawnPoints)
        {
            if (!spawnPoint.IsCoolingDown)
            {
                AvailableSpawnPointsNotCoolingDown.Add(spawnPoint);
            }
        }

        if (AvailableSpawnPointsNotCoolingDown.Count == 0)
        {
            // Debug.Log("no cooled down sp fond");
            return;
        }

        int i = Random.Range(0, AvailableSpawnPointsNotCoolingDown.Count);
        SpawnZombie(AvailableSpawnPointsNotCoolingDown[i]);
        AvailableSpawnPointsNotCoolingDown[i].StartCoolingDown();
    }

    //need to call this from a voice manager
    public void SetisspawningSpecialZombie()
    {
        Debug.Log("is it special wave " + IsspecialZombiesWave);
        if (IsspecialZombiesWave)
        {
            _isSpawningSpetialZombie = true;
        }
        Debug.Log("_isSpawningSpetialZombie " + _isSpawningSpetialZombie);
    }

    public void OnReloadWave()
    {
        ResetSpawnStates();
    }

    public void OnOutOfAmmo()
    {

    }

    public void OnKill(GameObject enemy)
    {
        CurCountOfZOmbiesOnScreen--;


        if (CurCountOfZOmbiesOnScreen <= 0 && GameManager.Instance.curgamestate == ARZState.WaveOverTime)
        {
            GameManager.Instance.curgamestate = ARZState.WaveEnded;
            return;
        }

        Wave_isittime_and_canI_spawnNow();
    }

    public void OnTouchObject(GameObject touched)
    {

    }

    public void OnTrigger(Collider c)
    {

    }

    public void OnGameOver()
    {
        ResetSpawnStates();
    }



    #endregion


    #region privateMethods

    private IEnumerator ie_StartWaveTimer()
    {
        // yield return new WaitForSeconds(WaveTimerInMinutes * 60);
        yield return new WaitForSeconds(WavetimeinSeconds);
        waveTimeisUp = true;
        ResetSpawnStates();
        if (CurCountOfZOmbiesOnScreen > 0)
        {
            GameManager.Instance.curgamestate = ARZState.WaveOverTime;

        }
        else
        {
            GameManager.Instance.curgamestate = ARZState.WaveEnded;
        }
    }

    private void InitSpawnPoints()
    {
        //ToDo: 
        // please make sure to handle "NoSpawns Placed" 
        // try doing this in edit scene. Warn user that no spwns have been placed

        List<GameObject> PlacedSpawnPoints = manager.GetSpawnPoints();
        _placeSpawnPoints = new List<SpawnPoint>();


        // calculate max spawn point index used by this array
        if (percentageOfSpawns >= 0.0f && percentageOfSpawns <= 1.0f)
        {
            maxSpawnPointIndex = (int)((PlacedSpawnPoints.Count) * percentageOfSpawns);
            if (maxSpawnPointIndex == 0 && PlacedSpawnPoints.Count > 0)
            {
                maxSpawnPointIndex = 1;
            }
        }
        else
        {
            maxSpawnPointIndex = PlacedSpawnPoints.Count;
        }

        for (int i = 0; i < maxSpawnPointIndex; i++)
        {
            SpawnPoint spawnPoint = PlacedSpawnPoints[i].GetComponent<SpawnPoint>();
            spawnPoint.Init(this, CoolDownTime);
            _placeSpawnPoints.Add(spawnPoint);
        }
    }

    private void SpawnZombie(SpawnPoint argSpawnpoint)
    {

        if (IsspecialZombiesWave)
        {
            if (_isSpawningSpetialZombie)
                SpawnZombieSpecial(argSpawnpoint);
            else
                SpawnRegularZombie(argSpawnpoint);
        }
        else
        {
            SpawnRegularZombie(argSpawnpoint);
        }
    }

    private void SpawnRegularZombie(SpawnPoint spawnPoint)
    {
        // Debug.Log("wavestd spawning");
        int randZombieIndex = Random.Range(0, zombies.Length);
        int randZombieHP = Random.Range(minZombieHP, maxZombieHP);

        GameObject z = manager.CreateEnemy(spawnPoint.gameObject, zombies[randZombieIndex]);
        ZombieBehavior zombie = z.GetComponent<ZombieBehavior>();

        zombie.SetHP(randZombieHP);
        CurCountOfZOmbiesOnScreen++;
    }

    private void SpawnZombieSpecial(SpawnPoint spawnPoint)
    {
        Debug.Log("wavestd spawning");
        int randSpecialZombieIndex = Random.Range(0, specialZombies.Length);
        int randZombieHP = maxZombieHP + SpecialZombieExtraHitPoints;
        GameObject z = manager.CreateEnemy(spawnPoint.gameObject, specialZombies[randSpecialZombieIndex]);
        _isSpawningSpetialZombie = false;
        ZombieBehavior zombie = z.GetComponent<ZombieBehavior>();
        zombie.SetHP(randZombieHP);
        CurCountOfZOmbiesOnScreen++;
    }

    private void FillAllSpawnpoints_onlyonStart()
    {
        if (GameManager.Instance.curgamestate == ARZState.EndGame || GameManager.Instance.curgamestate == ARZState.WaveBuffer) return;

        if (CurCountOfZOmbiesOnScreen >= maxZombiesOnScreen || waveTimeisUp || GameManager.Instance.isDead)
            return;

        foreach (SpawnPoint spawnPoint in _placeSpawnPoints)
        {
            spawnPoint.ResetMe();
            SpawnZombie(spawnPoint);
            spawnPoint.StartCoolingDown();

        }
    }

#endregion

}
