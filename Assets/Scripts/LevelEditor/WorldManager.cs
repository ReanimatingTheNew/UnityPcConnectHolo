// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

public class WorldManager : MonoBehaviour
{
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
   // public GameSettings Settings;
    public Material wireframeMaterial;
    public Material occlusionMaterial;

    public GameObject consoleObject;
    string AnchorName_ConsoleObject;


    //+++++++++++++++++++++++++++++++++++++++++++++CoreObjectsNeeded
    public GameObject spawnPoint;
    string AnchorName_SpawnPoint;

    public GameObject pathFinder;
    string AnchorName_PathFinder;

    public GameObject hotspot;
    string AnchorName_HotSpot;

    public GameObject scoreboard;
    string AnchorName_ScoreBoard;

    public GameObject stemBase;
    string AnchorName_StemBase;

    public GameObject infiniteAmmoBox;
    string AnchorName_AmmoBoxInfinite;


    public GameObject mist;
    string AnchorName_MistEmitter;

    public GameObject mistEnd;
    string AnchorName_MistEnd;


    public GameObject target;
    string AnchorName_Target;

    public GameObject metalBarrel;
    string AnchorName_MetalBarrel;

    public GameObject roomModel;
    string AnchorName_RoomModel;

    public GameObject startButton;
    string AnchorName_StartButton;

    List<GameObject> spawns;
    List<GameObject> infiniteAmmoBoxes;
    List<GameObject> hotspots;

    bool drawWireframe = true;
    bool scoreboardPlaced = false;          // only one scoreboard is allowed
    bool pathFinderPlaced = false;          // only one pathfinder is allowed
    bool mist_End_Placed = false;             // only one mist end position is allowed
    bool mistPlaced = false;
    bool stemBasePlaced = false;            // only one stem base allowed
    bool targetPlaced = false;
    bool metalBarrelPlaced = false;
    bool roomModelPlaced = false;
    bool startButtonPlaced = false;
    int infiniteAmmoBoxIdNum = 0;           // id num associated with the number of infinite ammo boxes in the room
    int hotspotIdNum = 0;                   // id num associated with the number of hotspots in the room
    int spawnIdNum = 0;                     // id num associated with the number of spawn points in the room

    WorldAnchorStore anchorStore;
    bool roomLoaded = false;
    bool calledToAnchorStore = false;

    void InitAnchorNameVariables() {

        AnchorName_ConsoleObject = GameSettings.Instance.GetAnchorName_ConsoleObject();

        AnchorName_StemBase = GameSettings.Instance.GetAnchorName_StemBase();

        AnchorName_SpawnPoint = GameSettings.Instance.GetAnchorName_SpawnPoint();

        AnchorName_ScoreBoard = GameSettings.Instance.GetAnchorName_ScoreBoard();

        AnchorName_AmmoBoxInfinite = GameSettings.Instance.GetAnchorName_AmmoBoxInfinite();

        AnchorName_PathFinder = GameSettings.Instance.GetAnchorName_PathFinder();

        AnchorName_MetalBarrel = GameSettings.Instance.GetAnchorName_MetalBarrel();

        AnchorName_MistEmitter = GameSettings.Instance.GetAnchorName_MistEmitter();

        AnchorName_MistEnd = GameSettings.Instance.GetAnchorName_MistEnd();

        AnchorName_HotSpot = GameSettings.Instance.GetAnchorName_HotSpot();

        AnchorName_Target = GameSettings.Instance.GetAnchorName_Target();

        AnchorName_StartButton = GameSettings.Instance.GetAnchorName_StartButton();
    }
    void Start()
    {
        InitAnchorNameVariables();
        Debug.Log("world managewr is on " + gameObject.name);
        spawns = new List<GameObject>();
        infiniteAmmoBoxes = new List<GameObject>();
        hotspots = new List<GameObject>();


        if (SceneManager.GetActiveScene().name != "EditMap")
        {
            StartCoroutine(AUTOGOTOGAME());
        }


    }


    IEnumerator AUTOGOTOGAME() {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Game");
    }



    void Update()
    {
        if (!calledToAnchorStore)
        {
            if (roomLoaded)
            {
                calledToAnchorStore = true;
                WorldAnchorStore.GetAsync(AnchorStoreReady);
            }
        }
    }

    public void RoomLoaded()
    {
        roomLoaded = true;
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;

        // list of strings
        List<string> spawnIds = new List<string>();
        List<string> dummySpawnIds = new List<string>();
        List<string> ammoBoxIds = new List<string>();
        List<string> infiniteAmmoBoxIds = new List<string>();
        List<string> mistIds = new List<string>();
        List<string> hotspotIds = new List<string>();


        // gather all stored anchors
        string[] ids = anchorStore.GetAllIds();

        for (int index = 0; index < ids.Length; index++)
        {
            if (ids[index] == AnchorName_StemBase)
            {
                // if anchor is stem base
                GameObject obj = Instantiate(stemBase) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(false);
                stemBasePlaced = true;
            }
            else if (ids[index] == AnchorName_ConsoleObject)
            {
                // if anchor is console object
                GameObject obj = Instantiate(consoleObject) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
            }
            else if (ids[index] == AnchorName_ScoreBoard)
            {
                // if anchor is the scoreboard
                GameObject obj = Instantiate(scoreboard) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
                scoreboardPlaced = true;
            }

            //----------------------------------------------------------------
            else if (ids[index] == AnchorName_Target)
            {
                // if anchor is weapons rack
                GameObject obj = Instantiate(target) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
                targetPlaced = true;
            }

            //----------------------------------------------------------------

            else if (ids[index] == AnchorName_StartButton)
            {
                // if anchor is weapons rack
                GameObject obj = Instantiate(startButton) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
                startButtonPlaced = true;
            }

            //----------------------------------------------------------------
            else if (ids[index] == AnchorName_MetalBarrel)
            {
                // if anchor is weapons rack
                GameObject obj = Instantiate(metalBarrel) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
             }

            
            else if (ids[index] == AnchorName_PathFinder)
            {
                // if anchor is the pathfinder object
                GameObject obj = Instantiate(pathFinder) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pathFinderPlaced = true;
            }
            
            else if (ids[index] == AnchorName_MistEnd)
            {
                // if anchor is the mistEnd
                GameObject obj = Instantiate(mistEnd) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                mist_End_Placed = true;
            }
            else if (ids[index]==AnchorName_MistEmitter)
            {

                // if anchor is the mistEnd
                GameObject obj = Instantiate(mist) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                mistPlaced = true;
            }



            else if (ids[index].Contains(AnchorName_AmmoBoxInfinite))
            {
                // if anchor is an infinite ammo box
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_AmmoBoxInfinite.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > spawnIdNum)
                {
                    infiniteAmmoBoxIdNum = thisId;
                }

                // add id to string list to instantiate later
                infiniteAmmoBoxIds.Add(ids[index]);
            }
           
          
            else if (ids[index].Contains(AnchorName_SpawnPoint))
            {
                // if anchor is a spawn point
                // get spawn id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_SpawnPoint.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > spawnIdNum)
                {
                    spawnIdNum = thisId;
                }

                // add id to string list to instantiate later
                spawnIds.Add(ids[index]);
            }
           
         
          
            else if (ids[index].Contains(AnchorName_HotSpot))
            {
                // if anchor is a hostpot
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_HotSpot.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > hotspotIdNum)
                {
                    hotspotIdNum = thisId;
                }

                // add id to string list to instantiate later
                hotspotIds.Add(ids[index]);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        // load and instantiate all infinite ammo boxes
        foreach (string id in infiniteAmmoBoxIds)
        {
            infiniteAmmoBoxes.Add(InstantiateObject(infiniteAmmoBox, id));
        }       

        // load and instantiate all spawn points
        foreach (string id in spawnIds)
        {
            spawns.Add(InstantiateObject(spawnPoint, id));
        }     

       
       

        // load and instantiate all hostpot objects
        foreach (string id in hotspotIds)
        {
            hotspots.Add(InstantiateObject(hotspot, id));
        }
    }

    GameObject InstantiateObject(GameObject obj, string id, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj) as GameObject;
        PersistoMatic pscript = o.GetComponent<PersistoMatic>();
        pscript.SetAnchorStoreName(id);
        pscript.SetRotateOnNormals(rotateOnNormals);
        pscript.KeepUpright(keepUpright);
        return o;
    }

    GameObject InstantiateObject(GameObject obj, string id, Vector3 position, Quaternion rotation, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj, position, rotation) as GameObject;
        PersistoMatic pscript = o.GetComponent<PersistoMatic>();
        pscript.SetAnchorStoreName(id);
        pscript.SetRotateOnNormals(rotateOnNormals);
        pscript.KeepUpright(keepUpright);
        return o;
    }

    public int GetSpawnCount()
    {
        return spawns.Count;
    }

 


    public int GetInfiniteAmmoBoxCount()
    {
        return infiniteAmmoBoxes.Count;
    }

 
   

    public int GetHotspotCount()
    {
        return hotspots.Count;
    }

    public bool isScoreboadPlaced()
    {
        return scoreboardPlaced;
    }

    public bool isTargetPlaced()
    {
        return targetPlaced;
    }


    public bool isStartButtonPlaced()
    {
        return startButtonPlaced;
    }


    //used in demowave to checkoff placed things in ui
    public bool isMistEndPlaced()
    {
        return mist_End_Placed;
    }

    public bool isPathFinderPlaced()
    {
        return pathFinderPlaced;
    }

    public int GetSpawnIdNum()
    {
        spawnIdNum++;
        return spawnIdNum;
    }

 
    public int GetHotspotIdNum()
    {
        hotspotIdNum++;
        return hotspotIdNum;
    }

    public int GetInfiniteAmmoBoxIdNum()
    {
        infiniteAmmoBoxIdNum++;
        return infiniteAmmoBoxIdNum;
    }

   


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleWireframe()
    {
        drawWireframe = !drawWireframe;
        if (drawWireframe)
            SpatialMappingManager.Instance.SurfaceMaterial = wireframeMaterial;
        else
            SpatialMappingManager.Instance.SurfaceMaterial = occlusionMaterial;
    }

    public void CreateStemBase()
    {
        if (stemBasePlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            GameObject obj = InstantiateObject(stemBase, AnchorName_StemBase, GazeManager.Instance.HitInfo.point, stemBase.transform.rotation) as GameObject;
            obj.GetComponent<PersistoMatic>().SetFaceCamera(true);
            stemBasePlaced = true;
        }
    }

    public void CreateSpawnPoint()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_SpawnPoint+ GetSpawnIdNum().ToString();
            spawns.Add(InstantiateObject(spawnPoint, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateMetalBarrel()
    {
        if (metalBarrelPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            GameObject obj = InstantiateObject(metalBarrel, AnchorName_MetalBarrel, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;
            obj.GetComponent<PersistoMatic>().SetFaceCamera(true);
            metalBarrelPlaced = true;
        }
    }

    public void CreateRoomModel()
    {
        if (roomModelPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            GameObject obj = InstantiateObject(roomModel, AnchorName_RoomModel, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;
            obj.GetComponent<PersistoMatic>().SetFaceCamera(false);
            roomModelPlaced = true;
        }
    }

    public void CreateConsole()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(consoleObject, AnchorName_ConsoleObject, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true);
        }
    }

    public void CreateHotspot()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            string id = AnchorName_HotSpot + GetHotspotIdNum().ToString();
            hotspots.Add(InstantiateObject(hotspot, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateInfiniteAmmoBox()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_AmmoBoxInfinite + GetInfiniteAmmoBoxIdNum().ToString();
            infiniteAmmoBoxes.Add(InstantiateObject(infiniteAmmoBox, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateScoreboard()
    {
        if (scoreboardPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(scoreboard, AnchorName_ScoreBoard, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            scoreboardPlaced = true;
        }
    }


    //------------------------------------------------------------
    public void CreateTarget()
    {
        if (targetPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(target, AnchorName_Target, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            targetPlaced = true;
        }
    }

    //--------------------------------------------------
    public void CreateStartButton()
    {
        if (startButtonPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(startButton, AnchorName_StartButton, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            startButtonPlaced = true;
        }
    }

    //--------------------------------------------------



    public void CreatePathFinder()
    {
        if (pathFinderPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(pathFinder, AnchorName_PathFinder, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            pathFinderPlaced = true;
        }
    }

 

    public void CreateMist()
    {
        if (mist_End_Placed)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(mist, AnchorName_MistEmitter, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            mist_End_Placed = true;
        }
    }

    public void CreateMistEnd()
    {
        if (mist_End_Placed)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(mistEnd, AnchorName_MistEnd, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            mist_End_Placed = true;
        }
    }
 
    public void Removing(PersistoMatic pScript)
    {
        if (pScript.AnchorStoreBaseName == AnchorName_StemBase)
        {
            stemBasePlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_ScoreBoard)
        {
            scoreboardPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_Target)
        {
            targetPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_StartButton)
        {
            startButtonPlaced = false;
        }

        else if (pScript.AnchorStoreBaseName == AnchorName_PathFinder)
        {
            pathFinderPlaced = false;
        }


        else if (pScript.AnchorStoreBaseName == AnchorName_MetalBarrel)
        {
            metalBarrelPlaced = false;
        }

        else if (pScript.AnchorStoreBaseName == AnchorName_RoomModel)
        {
            roomModelPlaced = false;
        }


        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_SpawnPoint))
        {
            spawns.Remove(pScript.gameObject);
        }
        
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_AmmoBoxInfinite))
        {
            infiniteAmmoBoxes.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_MistEmitter))
        {
            mist_End_Placed = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_MistEnd)
        {
            mist_End_Placed = false;
        }
     
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_HotSpot))
        {
            hotspots.Remove(pScript.gameObject);
        }

    }

    void OnReset()
    {
        stemBasePlaced = false;         // only one stem base is allowed
        spawnIdNum = 0;                 // id num associated with the number of spawn points in the room
        scoreboardPlaced = false;       // only one scoreboard is allowed
        targetPlaced = false;
        infiniteAmmoBoxIdNum = 0;       // id num associated with the number of infinite ammo boxes in the room
        pathFinderPlaced = false;       // only one pathfinder is allowed
        mistPlaced = false;                  // id num associated with the number of mist objects in the room
        hotspotIdNum = 0;               // id num associated with the number of hotspot objects in the room
        metalBarrelPlaced = false;   // only one airstrike start is allowed
        roomModelPlaced = false;
        mist_End_Placed = false;
        startButtonPlaced = false;
    }
}