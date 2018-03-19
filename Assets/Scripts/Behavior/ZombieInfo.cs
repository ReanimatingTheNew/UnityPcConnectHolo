// @Author Nabil Lamriben ©2018
using UnityEngine;

public class ZombieInfo : MonoBehaviour {

    #region dependencies
    TextMesh _zinfoMesh;
    ZombieBehavior _ZBEH;
    //Gamesettings as well
    #endregion

    #region INITandListeners
    void Awake () {
        _ZBEH = GetComponent<ZombieBehavior>();
        _zinfoMesh = GetComponentInChildren<TextMesh>();
    }

    private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += ShowState;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= ShowState;
    }
    #endregion

    #region PrivateMethods
    void ShowState(ZombieState argstate) {
        if (GameSettings.Instance != null) {
            if (GameSettings.Instance.IsTestModeON) {
                _zinfoMesh.text = argstate.ToString();
            }
        } else { Debug.Log("no game settings"); }
    }
    #endregion
}
