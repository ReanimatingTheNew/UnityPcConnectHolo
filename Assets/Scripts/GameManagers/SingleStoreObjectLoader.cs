using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;


public class SingleStoreObjectLoader : MonoBehaviour {
    public GameObject PathFinderWorldRef;
    string AnchorName_PathFinder;
    GameObject _localPathFindObj;
    WorldAnchorStore anchorStore;

    public Transform getPathfinderWorldReff() { return this.PathFinderWorldRef.transform; }

  
    void InitAnchorNameVariables()
    {
        InitAnchorNameVariables();
        AnchorName_PathFinder = GameSettings.Instance.GetAnchorName_PathFinder();
    }
    // Use this for initialization
    void Start () {
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }
    
    
    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
        LoadObjects();
    }
    void LoadObjects()
    {
     
            // gather all stored anchors
            string[] ids = anchorStore.GetAllIds();
            for (int index = 0; index < ids.Length; index++)
            {
                
                if (ids[index].Contains("PathFind"))
                {
                    _localPathFindObj  = Instantiate(PathFinderWorldRef) as GameObject;
                    anchorStore.Load(ids[index], _localPathFindObj);
                    WorldAnchor attachedAnchor = _localPathFindObj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null) DestroyImmediate(attachedAnchor);
                    _localPathFindObj.name = "myWorldRefObject";
                }
            }
        

        if (AnchorName_PathFinder == null || AnchorName_PathFinder=="")
        {
            DebugConsole.print(" pathfinder is NULL or empty");
        }

       // LevelLoaded();
        //todonabilsr
        //maketheStartResetBlock();
        // maketheStarBlock();
    }

}
