// @Author Nabil Lamriben ©2017
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GameVoiceCommands : MonoBehaviour {




 


    //GameMode
    ARZGameModes curMode;
    string KeyPhraseStart;
    string KeyPhraseReset;
    string KeyPhraseExit;
    string KeyPhrasePause;
    string KeyPhraseContinue;
    string NameOfSceneToReload;

    public WaveManager MyWaveManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void keyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.J)) { StartTheGame(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { ResetTheGame(); }
        if (Input.GetKeyDown(KeyCode.M)) { ExitTheGame(); }
        if (Input.GetKeyDown(KeyCode.P)) { PauseTheGame(); }
        if (Input.GetKeyDown(KeyCode.O)) { ContinueTheGame(); }

    }

    void StartTheGame() { GameManager.Instance.CheckStartGame(); }
    void ResetTheGame() { GameManager.Instance.LoadScene(NameOfSceneToReload); }
    void ExitTheGame() { GameManager.Instance.LoadScene("MainMenu"); }
    void PauseTheGame() {
        Time.timeScale = 0;
        GameManager.Instance.SignalGamePause(false); //when stem hand ctrl hears it it will handle allow dissallow inout ,in th is case, it will DISSALLOW the user from pressing buttons
        StemKitMNGR.CALL_ResetGunAndMeter();
    }
    private void ContinueTheGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.SignalGameContinue(true);//when stem hand ctrl hears it it will handle allow dissallow inout , in th is case, it will allow
        StemKitMNGR.CALL_ResetGunAndMeter();
    }

    void SpawnSpecialZombie()
    {
        Debug.Log("special on ");
        MyWaveManager.SetFlagForSpecialZombieSpawn();
    }

    // Use this for initialization
    void Start()
    {

        if (GameManager.Instance.curgamemode == ARZGameModes.GameRight)
        {
            KeyPhrasePause = "Bravo Pause"; KeyPhraseContinue = "Bravo Continue";
            KeyPhraseExit = "Bravo Exit"; KeyPhraseReset = "Bravo Reset"; KeyPhraseStart = "Bravo Ready"; NameOfSceneToReload = "Game";
        }
        else if (GameManager.Instance.curgamemode == ARZGameModes.GameLeft)

        {
            KeyPhrasePause = "Alpha Pause"; KeyPhraseContinue = "Alpha Continue";
            KeyPhraseExit = "Alpha Exit"; KeyPhraseReset = "Alpha Reset"; KeyPhraseStart = "Alpha Ready"; NameOfSceneToReload = "Game";
        }
        else
        {
            KeyPhraseExit = "No Stem Exit"; KeyPhraseReset = "No Stem Reset"; KeyPhraseStart = "No Stem Survivor Ready"; NameOfSceneToReload = "Game";
            KeyPhrasePause = "Pause No Stem"; KeyPhraseContinue = "Continue No Stem";
        }

       

        

        if (!GameManager.Instance.IsInDevRoom())
        {

            keywords.Add(KeyPhrasePause, () =>
            {
                PauseTheGame();
            });
            keywords.Add(KeyPhraseContinue, () =>
            {
                ContinueTheGame();
            });

            keywords.Add(KeyPhraseStart, () =>
            {
                StartTheGame();
            });


            keywords.Add(KeyPhraseReset, () =>
            {
                ResetTheGame();
            });

            keywords.Add(KeyPhraseExit, () =>
            {
                ExitTheGame();
            });


            keywords.Add("Shit", () =>
            {
                SpawnSpecialZombie();
            });


            keywords.Add("Fuck", () =>
            {
                SpawnSpecialZombie();
            });


            keywords.Add("O Fuck", () =>
            {
                SpawnSpecialZombie();
            });


            keywords.Add("huge Zombie", () =>
            {
                SpawnSpecialZombie();
            });



            keywords.Add("Oh My God", () =>
            {
                SpawnSpecialZombie();
            });


            keywords.Add("O Shit", () =>
            {
                SpawnSpecialZombie();
            });

            // Tell the KeywordRecognizer about our keywords.
            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

            // Register a callback for the KeywordRecognizer and start recognizing!
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Start();
        }
    }

    void Update()
    {
        keyboardInputs();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Dispose();
        }
    }
}
