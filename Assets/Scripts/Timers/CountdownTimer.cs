// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

    public Text timerTxt;

   
    private float timeInSeconds;

    float timer;
    string timerText;
    bool timesUpThrown;

    const int maxSecondCount = 12; //this cannot change

    private void OnEnable()
    {
        GameManager.OnGamePaused += PauseCountDownCounter;
        GameManager.OnGameContinue += ContinueCountDownCounter;
    }

    private void OnDisable()
    {
        GameManager.OnGamePaused -= PauseCountDownCounter;
        GameManager.OnGameContinue -= ContinueCountDownCounter;
    }

    bool _isPaused = false;
    void PauseCountDownCounter(bool argtruefalse) { _isPaused = true; }
    void ContinueCountDownCounter(bool argtruefalse) { _isPaused = false; }

    // Use this for initialization
    void Start () {
        if (GameSettings.Instance != null)
                    {
            
                        if (GameSettings.Instance.IsGameLong) { timeInSeconds = 240f; }
                       else { timeInSeconds = 120f; }
            timer = timeInSeconds;
            timesUpThrown = false;
            DisplayTimerText();
                }
    }


    float OneSec = 1.01f;
	void Update () {

        if(!_isPaused)CountDown();
        DisplayTimerText();

    }

    void CountDown()
    {
        if (timesUpThrown)
            return;

        if (timer <= 0.01f)
        {
            timesUpThrown = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TimesUp();
                GameManager.Instance.HardStop();
                if (timerTxt != null)
                    timerText = "00:00";
            }
            return;
        }
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsGameStarted())
            {
                timer -= Time.deltaTime;
                {
                    if ((int)timer >= maxSecondCount ? false : true)
                    {
                        OneSec -= Time.deltaTime;
                        if (OneSec < 0)
                        {
                            OneSec = 1f;
                            GameManager.Instance.call_CountDownAudioVideo((int)timer);
                        }
                    }
                }
            }
        }
    }

    void DisplayTimerText()
    {
        int t = (int)timer;
        int minutes = t / 60;
        int seconds = t % 60;
        string min = (minutes < 10) ? ("0" + minutes) : ("" + minutes);
        string s = (seconds < 10) ? ("0" + seconds) : ("" + seconds);
        if (timerTxt != null)
            timerText = min + ":" + s;
    }

    void LateUpdate()
    {
        if(timerTxt!=null)
        timerTxt.text = timerText;
    }
}
