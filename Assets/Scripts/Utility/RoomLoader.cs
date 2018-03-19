// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;

public class RoomLoader : MonoBehaviour {

    public GameObject managerObject;            // the room manager for this scene
    public GameObject surfaceObject;            // prefab for surface mesh objects
    public string fileName= "ARZArena";         // name of file used to store mesh
    public string anchorStoreName="ARZRoomMesh";// name of world anchor for room

    WorldAnchorStore anchorStore;               // store of world anchors
    List<Mesh> roomMeshes;                      // list of room meshes
    List<GameObject> roomObjects;               // list of game objects that hold room meshes

	// Use this for initialization
	void Start () {
        Debug.Log("async");
        // get instance of WorldAnchorStore


        if (managerObject.GetComponent<GameManager>() != null) {
            if (GameManager.Instance.usedevRoom) { managerObject.SendMessage("ActivateDevRoom"); return; }
        }


        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    public void ToggleRoom()
    {
        foreach(GameObject obj in roomObjects)
        {
            if (obj.activeInHierarchy)
                obj.SetActive(false);
            else
                obj.SetActive(true);
        }
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        // save instance
        anchorStore = store;

        // load room meshesn
        roomMeshes = MeshSaverOld.Load(fileName) as List<Mesh>;
        roomObjects = new List<GameObject>();

        foreach (Mesh surface in roomMeshes)
        {
            GameObject obj = Instantiate(surfaceObject) as GameObject;
            obj.GetComponent<MeshFilter>().mesh = surface;
            obj.GetComponent<MeshCollider>().sharedMesh = surface;
            obj.transform.parent = this.transform;
            roomObjects.Add(obj);

             if (!anchorStore.Load(surface.name, obj)) Debug.Log("WorldAnchor load failed...");
        }

        if (managerObject != null)
            managerObject.SendMessage("RoomLoaded");
    }

    void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        else
        if (GameManager.Instance.usedevRoom)
        {
            return;
        }
        else
        {
            foreach (Mesh mesh in roomMeshes)
            {
                Destroy(mesh);
            }
            roomMeshes.Clear();
        }

    }
}
