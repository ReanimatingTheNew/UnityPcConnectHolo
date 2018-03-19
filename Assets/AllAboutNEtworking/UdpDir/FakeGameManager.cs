using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGameManager : MonoBehaviour {


    public static FakeGameManager Instance = null;


    public delegate void EventOtherPlayerReachedStreak();
    public static EventOtherPlayerReachedStreak OtherPlayerStreakHandeler;
    public  void Call_IHeardOtherPlayerStreakMax() {
        if (IsHandlerAvailable()) { OtherPlayerStreakHandeler(); } }
    public  bool IsHandlerAvailable()
    {
        return OtherPlayerStreakHandeler != null;
    } 
   

    private void Awake()
    {
        //curgamemode = GameSettings.Instance.GameMode;
       // InitAnchorNameVariables();

        if (Instance == null)
        {
            //curgamestate = ARZState.Pregame;

            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    #region GunLines
  
    #endregion


    #region START
    void Start()
    {

        

         
    }
    //todo: find a better way to initialize the gun in had in any other way than a timer... bad programming
    void SetGun()
    {
    
    }

    public void CheckStartGame()
    {
      
    }
 
 
    #endregion
}
