// @Author Jeffrey M. Paquette ©2016
// @Author Nabil Lamriben ©2018
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    #region events
    public delegate void ZombieStateChanged(ZombieState argargZstate);
    public  event ZombieStateChanged OnZombieStateChanged;
    void ZombieStateUpdated(ZombieState argargZstate)
    {
        if (OnZombieStateChanged != null) OnZombieStateChanged(argargZstate);
    }
    private ZombieState _curState;

    public ZombieState CurZombieState
    {
        get { return _curState; }
        set { _curState = value; ZombieStateUpdated(_curState); }
    }


    #endregion

    #region PrivateVars

    //ZombieState StateBeforePause;
    private int _zID;
    #endregion

    #region Dependencies
    ZombieDamage _zDamage;
    ZombieAnimState _zStateAnim;
    ZombieLocomotion _zLocomotion;
    #endregion

    #region INIT
    private void OnEnable()
    {
       // GameVoiceCommands.OnGamePaused += PauseZombieAnimation;
      //  GameVoiceCommands.OnGameContinue += ContinueZombieAnimation;
    }

    private void OnDisable()
    {
     //   GameVoiceCommands.OnGamePaused -= PauseZombieAnimation;
      //  GameVoiceCommands.OnGameContinue -= ContinueZombieAnimation;

    }

    private void Awake() {
        _zDamage = GetComponent<ZombieDamage>();
        _zStateAnim = GetComponent<ZombieAnimState>();
        _zLocomotion = GetComponent<ZombieLocomotion>();
        //  calcRandChasetype();
    }
    #endregion

    #region PublicMethods
    public void SetID(int argId) { _zID = argId; }
    public int GetID() { return _zID; }

    //ZSight.Update
    public void HasLineOfSight(bool argCanSeeyou)
    {
        //locomotion has line of sight
    }

    //WaveManager.SpawnEnemy()
    public void SetHP(int value)
    {
        if (_zDamage != null)
        {
            _zDamage.SetHP(value);
        }
        else
        {
            Debug.Log("YO ! , no zdammage found");
        }
    }

    //gm.hardstop() and gm.playerdied()
    public void Zbeh_PauseZombieAnimation()
    {
        CurZombieState = ZombieState.PAUSED;
    }

    //BY EVENT
    ////gm.DestroyDeadEnemy()
    // void Melt()
    //{
    //   //event _zLocomotion.Melt();
    //}
    #endregion 
}



//public Transform DeepSearch(Transform parent, string val)
//{
//    foreach (Transform c in parent)
//    {
//        if (c.gameObject.tag == val) { return c; }
//        var result = DeepSearch(c, val);
//        if (result != null)
//            return result;
//    }
//    return null;
//}