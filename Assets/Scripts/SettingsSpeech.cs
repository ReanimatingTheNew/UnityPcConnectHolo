// @Author Nabil Lamriben ©2017
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;


public class SettingsSpeech : MonoBehaviour {

    //GameMode
    //ARZGameModes curMode;
    //string KeyPhraseToggleGameLength;
    //string KeyPhraseToggleBlood;
 

    //string KeyPhraseExit;
    //string NameOfSceneToReload;
    //public WaveManager MyWaveManager;

    //KeywordRecognizer keywordRecognizer = null;
    //Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    //void keyboardInputs()
    //{
    //    if (Input.GetKeyDown(KeyCode.J)) { StartTheGame(); }
    //    if (Input.GetKeyDown(KeyCode.R)) { ResetTheGame(); }
    //    if (Input.GetKeyDown(KeyCode.M)) { ExitTheGame(); }

    //}

    //void StartTheGame() { GameManager.Instance.CheckStartGame(); }
    //void ResetTheGame() { GameManager.Instance.LoadScene(NameOfSceneToReload); }
    //void ExitTheGame() { GameManager.Instance.LoadScene("MainMenu"); }

    //void SpawnSpecialZombie()
    //{
    //    Debug.Log("special on ");
    //    MyWaveManager.SetFlagForSpecialZombieSpawn();
    //}

    //// Use this for initialization
    //void Start()
    //{

       
    //        KeyPhraseToggleGameLength = "Left Survivor Ready";
    
 

    //    if (!GameManager.Instance.IsInDevRoom())
    //    {
    //        keywords.Add(KeyPhraseStart, () =>
    //        {
    //            StartTheGame();
    //        });


    //        keywords.Add(KeyPhraseReset, () =>
    //        {
    //            ResetTheGame();
    //        });

    //        keywords.Add(KeyPhraseExit, () =>
    //        {
    //            ExitTheGame();
    //        });


    //        keywords.Add("Shit", () =>
    //        {
    //            SpawnSpecialZombie();
    //        });


    //        keywords.Add("Fuck", () =>
    //        {
    //            SpawnSpecialZombie();
    //        });


    //        keywords.Add("Oh My God", () =>
    //        {
    //            SpawnSpecialZombie();
    //        });


    //        keywords.Add("Holly Shit", () =>
    //        {
    //            SpawnSpecialZombie();
    //        });

    //        // Tell the KeywordRecognizer about our keywords.
    //        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

    //        // Register a callback for the KeywordRecognizer and start recognizing!
    //        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
    //        keywordRecognizer.Start();
    //    }
    //}

    //void Update()
    //{
    //    keyboardInputs();
    //}

    //private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    //    System.Action keywordAction;
    //    if (keywords.TryGetValue(args.text, out keywordAction))
    //    {
    //        keywordAction.Invoke();
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (keywordRecognizer != null)
    //    {
    //        keywordRecognizer.Stop();
    //        keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
    //        keywordRecognizer.Dispose();
    //    }
    //}
}
