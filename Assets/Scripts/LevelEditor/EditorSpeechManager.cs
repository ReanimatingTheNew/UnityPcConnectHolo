// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EditorSpeechManager : MonoBehaviour
{
    public GameObject worldManagerObject;
    //public GameObject waveEditorManagerObject;

    WorldManager worldManager;
    RoomLoader roomLoader;
    WaveEditorManager waveEditorManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


    void EditorKeyboardInputs()
    {
 

        if (Input.GetKeyDown(KeyCode.Z)) { worldManager.CreateSpawnPoint(); }
        if (Input.GetKeyDown(KeyCode.X)) { worldManager.CreatePathFinder(); }
        if (Input.GetKeyDown(KeyCode.C)) { worldManager.CreateHotspot(); }


        if (Input.GetKeyDown(KeyCode.V)) { worldManager.CreateScoreboard(); }
        if (Input.GetKeyDown(KeyCode.B)) { worldManager.CreateInfiniteAmmoBox(); }
        if (Input.GetKeyDown(KeyCode.N)) { worldManager.CreateStemBase(); }

        if (Input.GetKeyDown(KeyCode.A)) { worldManager.CreateTarget(); }
        if (Input.GetKeyDown(KeyCode.S)) { worldManager.CreateMetalBarrel(); }
        if (Input.GetKeyDown(KeyCode.D)) { worldManager.CreateConsole(); }

        if (Input.GetKeyDown(KeyCode.F)) { worldManager.CreateStartButton(); }


        if (Input.GetKeyDown(KeyCode.LeftBracket)) { worldManager.CreateMist(); }
        if (Input.GetKeyDown(KeyCode.RightBracket)) { worldManager.CreateMistEnd(); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { worldManager.CreateRoomModel(); }

        if (Input.GetKeyDown(KeyCode.M)) { worldManager.LoadScene("MainMenu"); }



        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PersistoMatic[] objects = (PersistoMatic[])GameObject.FindObjectsOfType(typeof(PersistoMatic));
            foreach (PersistoMatic obj in objects)
            {
                obj.SendMessage("OnRemove");
            }
        }
       

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null) { focusObject.SendMessage("OnRemove"); }
        }
 

    }
    
    void Update()
    {
        EditorKeyboardInputs();
    }

    // Use this for initialization
    void Start()
    {
        if (worldManagerObject == null)
        {
            worldManager = FindObjectOfType<WorldManager>();
        }
        else
            worldManager = worldManagerObject.GetComponent<WorldManager>();

        roomLoader = FindObjectOfType<RoomLoader>();

        // load main menu scene
        keywords.Add("Main Menu", () =>
        {
            worldManager.LoadScene("MainMenu");
        });

        // Call the OnReset method on every gameobject.
        keywords.Add("Reset world", () =>
        {
            PersistoMatic[] objects = (PersistoMatic[])GameObject.FindObjectsOfType(typeof(PersistoMatic));
            foreach (PersistoMatic obj in objects)
            {
                obj.SendMessage("OnRemove");
            }  
        });

        keywords.Add("Toggle wireframe", () =>
        {
            worldManager.ToggleWireframe();
        });

        keywords.Add("Toggle room", () =>
        {
            roomLoader.ToggleRoom();
        });

        keywords.Add("Start scan", () =>
        {
            SpatialMappingManager.Instance.StartObserver();
        });

        keywords.Add("Stop scan", () =>
        {
            SpatialMappingManager.Instance.StopObserver();
        });

        keywords.Add("Place stem base", () =>
        {
            worldManager.CreateStemBase();
        });

        keywords.Add("Place console", () =>
        {
            worldManager.CreateConsole();
        });

        keywords.Add("Place spawn", () =>
        {
            worldManager.CreateSpawnPoint();
        });

       

       

        keywords.Add("Place scoreboard", () =>
        {
            worldManager.CreateScoreboard();
        });

        keywords.Add("Place metal barrel", () =>
        {
            worldManager.CreateMetalBarrel();
        });


        //not yet ready
        //keywords.Add("Place room model", () =>
        //{
        //    worldManager.CreateRoomModel();
        //});



        keywords.Add("Place infinite ammo box", () =>
        {
            worldManager.CreateInfiniteAmmoBox();
        });

        keywords.Add("Place path finder", () =>
        {
            worldManager.CreatePathFinder();
        });

        keywords.Add("Place start button", () =>
        {
            worldManager.CreateStartButton();
        });

        keywords.Add("Place mist", () =>
        {
            worldManager.CreateMist();
        });

        keywords.Add("Place mist end", () =>
        {
            worldManager.CreateMistEnd();
        });
       

        keywords.Add("Place hot spot", () =>
        {
            worldManager.CreateHotspot();
        });

    

        keywords.Add("Place Target", () =>
        {
            worldManager.CreateTarget();
        });

        keywords.Add("Remove", () =>
        {
            var focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnRemove");
            }
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}