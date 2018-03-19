// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections;
using HoloToolkit.Unity.InputModule;

public class GameGestureManager : MonoBehaviour {

    public LayerMask layerMask = Physics.DefaultRaycastLayers;
     GameObject gun;

    GestureRecognizer gestureRecognizer;
    //GunSelector playerGunSelector;
    WaveManager waveManager;

    // Use this for initialization
    void Start () {

        // get gun instance


        // get wave manager instance
        StartCoroutine(findGunInaFew());

    }


    IEnumerator findGunInaFew()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("get ref to curr gun , it should be a nostemgun");


        waveManager = FindObjectOfType<WaveManager>();

        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        // tap event handler
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;

        // hold event handlers - cancel and complete should do the same thing
        //gestureRecognizer.HoldStartedEvent += GestureRecognizer_HoldStartedEvent;
        //gestureRecognizer.HoldCompletedEvent += GestureRecognizer_HoldCompletedEvent;
        //gestureRecognizer.HoldCanceledEvent += GestureRecognizer_HoldCompletedEvent;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();


    }

    private void OnTap()
    {
        //Debug.Log("we tappi");
        // don't register taps if player is dead

        //if (playerGunSelector!=null)
        //{
        //    if (GameManager.Instance.isDead || !playerGunSelector.gameObject.activeInHierarchy)
        //    {
        //        Debug.Log("no tappi");
        //        return;
        //    }
        //}
   
        /*

        if (GazeManager.Instance.IsGazingAtObject)
        {
            if (GazeManager.Instance.HitObject.CompareTag("Ammo"))
            {
                // removed distance condition
                // Vector3.Distance(Camera.main.transform.position, GazeManager.Instance.HitInfo.point) < 1.25f &&
                if (!playerGunSelector.IsReloading() && playerGunSelector.GetCurrGunType() == GunType.PISTOL)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGunSelector.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("MagnumAmmo"))
            {
                if (!playerGunSelector.IsReloading() && playerGunSelector.GetCurrGunType() == GunType.MAGNUM)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGunSelector.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("UziClip"))
            {
                if (!playerGunSelector.IsReloading() && playerGunSelector.GetCurrGunType() == GunType.UZI)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGunSelector.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("ShotgunAmmo"))
            {
                if (!playerGunSelector.IsReloading() && playerGunSelector.GetCurrGunType() == GunType.SHOTGUN)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGunSelector.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("RackWeapon"))
            {
                GazeManager.Instance.HitObject.SendMessage("Take");
                return;
            }
            else if (GazeManager.Instance.HitObject.CompareTag("Interactive"))
            {
                // removed distance condition
                //if (Vector3.Distance(Camera.main.transform.position, GazeManager.Instance.HitInfo.point) < 1.25f)
                //{
                    waveManager.OnTouchObject(GazeManager.Instance.HitObject);
                    return;
                //}
            }
        }

        playerGunSelector.PlayerGunFire();*/
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.curgamestate != ARZState.EndGame)
            {
                OnTap();
            }
        }
    
    }

    private void GestureRecognizer_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {
        //if (!playerGunSelector.enabled)
        //    return;

        //playerGunSelector.GetActiveGunScript().GUN_FIRE();
    }

    private void GestureRecognizer_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {
        //if (!playerGunSelector.enabled)
        //    return;

        //playerGunSelector.GetActiveGunScript().GUN_STOP_FIRE();
    }

    // Update is called once per frame   playerGun.Fire();
    void Update () {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.curgamestate != ARZState.EndGame)
            {
                if (Input.GetKeyDown(KeyCode.Space)) OnTap();
            }
        }
     

    }
}
