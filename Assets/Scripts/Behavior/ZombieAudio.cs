// @Author Nabil Lamriben ©2018

using HoloToolkit.Unity;
using UnityEngine;

public class ZombieAudio : MonoBehaviour {
	#region dependencies
	UAudioManager _audioManager;
    ZombieBehavior _ZBEH;
	#endregion

	#region INITandListeners
	private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += UpdateAudio;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= UpdateAudio;
    }

	void Awake () {
        _ZBEH = GetComponent<ZombieBehavior>();
        _audioManager = GetComponent<UAudioManager>();
    }
	#endregion

	#region PrivateMethods
	void UpdateAudio(ZombieState argNewZombieState)
    {
        _audioManager.StopAllEvents();
        switch (argNewZombieState)
        {
            case ZombieState.IDLE:

            case ZombieState.WALKING:
                _audioManager.PlayEvent("_Idle");
                break;
            case ZombieState.CHASING:
                _audioManager.PlayEvent("_Chasing");
                break;
            case ZombieState.REACHING:
                _audioManager.PlayEvent("_Attack");
                break;
            case ZombieState.DEAD:
                _audioManager.PlayEvent("_Die");
                break;
            case ZombieState.PAUSED:
                _audioManager.StopAllEvents();
                break;

        }
    }
	#endregion
}
