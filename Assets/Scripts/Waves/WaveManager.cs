// @Author Jeffrey M. Paquette ©2016
// @Author Nabil Lamriben ©2017
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    #region props
    public GameObject[] waves;                      // array of waves prefab objects
    public bool isWaveLoaded { get; private set; }  // flag set when wave is loaded
    GameObject currentWave;                         // the currently loaded wave
    IWave wave;                                     // the script of the currently loaded wave
    int numberOfWaves;                              // the total number of waves
    int waveNum = 0;                                // the number we are on
    WaveStandard WS;
    [Tooltip("Setting this value will allow infinitammo ")]
    public bool isInfinitAmo;
    bool GameIsNotOver_IcanmakeZombies = true;

    [Tooltip("3 waves for 120 ")]
    public GameObject[] ShortWaves;                      // array of waves prefab objects
    #endregion
    // Use this for initialization
    void Start () {
        isWaveLoaded = false;
        numberOfWaves = waves.Length;
        if (numberOfWaves == 0) return;

        if (!GameSettings.Instance.IsGameLong)
        {
            ShortWaves[0].GetComponent<WaveStandard>().WavetimeinSeconds = (waves[0].GetComponent<WaveStandard>().WavetimeinSeconds / 2);
            ShortWaves[1].GetComponent<WaveStandard>().WavetimeinSeconds = (waves[1].GetComponent<WaveStandard>().WavetimeinSeconds / 2);
            ShortWaves[2].GetComponent<WaveStandard>().WavetimeinSeconds = (waves[2].GetComponent<WaveStandard>().WavetimeinSeconds / 2);


            currentWave = Instantiate(ShortWaves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            isWaveLoaded = true;
            WS = GetCurrWave();
        }
        else
        {
            currentWave = Instantiate(waves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            isWaveLoaded = true;
            WS = GetCurrWave();
        }
    }

    public void BeginNextWave(float time)
    {
        //first time when we say survivorready this happens +5seconds
        GameManager.Instance.curgamestate = ARZState.WaveBuffer;
        // start next wave in time seconds
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(time, WMStartWave);
    }


    public void WaveCompleted_soPopANewOne()
    {

        Destroy(currentWave);  //12345
        waveNum++;

        if (!GameSettings.Instance.IsGameLong)
        {

            if (currentWave != null)
            {
                currentWave = Instantiate(ShortWaves[waveNum]);
                wave = currentWave.GetComponent<IWave>();
                isWaveLoaded = true;
                BeginNextWave(GameSettings.Instance.NextBuffer);
            }
        }
        else
        {
            currentWave = Instantiate(waves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            isWaveLoaded = true;
            BeginNextWave(GameSettings.Instance.NextBuffer);
        }
    }

    public void StopTheGame() {
        GameIsNotOver_IcanmakeZombies = false;
        if(currentWave != null)currentWave.GetComponent<WaveStandard>().StopSpawnStates();
        isWaveLoaded = false;
        Destroy(currentWave);

        // don't reload wave if game over
       
    }

    public void KeepPlaying()
    {
        GameIsNotOver_IcanmakeZombies =true;
        currentWave.GetComponent<WaveStandard>().ResetSpawnStates();
        isWaveLoaded = false;
        Destroy(currentWave);

        StemKitMNGR.CALL_ResetGunAndMeter();
        if (!GameSettings.Instance.IsGameLong)
        {


            currentWave = Instantiate(ShortWaves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            WS = GetCurrWave();
            isWaveLoaded = true;

            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(GameSettings.Instance.StartResetWaveIn, ResetWave);


            t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(GameSettings.Instance.StartWaveAgain, WMStartWave);

        }
        else
        {

            currentWave = Instantiate(waves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            WS = GetCurrWave();
            isWaveLoaded = true;

            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(GameSettings.Instance.StartResetWaveIn, ResetWave);


            t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(GameSettings.Instance.StartWaveAgain, WMStartWave);

        }
    }

    public void WMStartWave()
    {
        if (GameIsNotOver_IcanmakeZombies)
        {
           // Debug.Log("2 wave mngr StartWave");
            wave.StartWave();
        }
    }



    public void ResetWave()
    {
        StemKitMNGR.CALL_ResetGunAndMeter();
        if (GameIsNotOver_IcanmakeZombies)
        {
            GameManager.Instance.ResetWave();
            currentWave.GetComponent<WaveStandard>().ResetSpawnStates();
        }
           
    }
    

    //called from Speechmanager and will set the flag on the current wave 
    public void SetFlagForSpecialZombieSpawn() {
        currentWave.GetComponent<IWave>().SetisspawningSpecialZombie();
    }


    public WaveStandard GetCurrWave() { return currentWave.GetComponent<WaveStandard>(); }

 

    public string GetWaveRomanNumeral()
    {
        string romanNumeral;

        switch (waveNum)
        {
            case 0:
                romanNumeral = "I";
                break;
            case 1:
                romanNumeral = "II";
                break;
            case 2:
                romanNumeral = "III";
                break;
            case 3:
                romanNumeral = "IV";
                break;
            case 4:
                romanNumeral = "V";
                break;
            case 5:
                romanNumeral = "VI";
                break;
            case 6:
                romanNumeral = "VII";
                break;
            case 7:
                romanNumeral = "VIII";
                break;
            case 8:
                romanNumeral = "IX";
                break;
            case 9:
                romanNumeral = "X";
                break;
            default:
                romanNumeral = "";
                break;
        }

        return romanNumeral;
    }

    public IWave GetWave()
    {
        return wave;
    }



    public void OnReload()
    {
        wave.OnReloadWave();
    }

    public void OnOutOfAmmo()
    {
        wave.OnOutOfAmmo();
    }

    public void OnKill(GameObject enemy)
    {
        wave.OnKill(enemy);
    }

    public void OnTouchObject(GameObject touched)
    {
        wave.OnTouchObject(touched);
    }

    public void OnTrigger(Collider c)
    {
        wave.OnTrigger(c);
    }

}
