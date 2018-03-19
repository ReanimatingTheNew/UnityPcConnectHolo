// @Author Nabil Lamriben ©2018
using UnityEngine;

public class ZombieAnimState : MonoBehaviour {


    #region PublicVars
    public bool UseBobingAnim;
    public bool UseHyperChase;
    #endregion

    #region PrivateVars
    /// <summary>
    /// 0 chase normal
    /// 1 chase Bob2
    /// 2 chase BobUpDown
    /// 3 Hyperchase 
    /// </summary>
    /// 
    int numberOfReachAnimation = 2;
    int numberOfDeathAnimations = 4;
    float attackSpeed = 1f;
    float multiplierBeforePause = 1f;
    float multiplier = 0.9f;        // multiplier of animation speed
    int deathType = 0;              // deathType for animator
    int reachType = 0;              // reachType for animator
    int ChaseType = 0;
    float RealMultiplyer;
    int multiplyerExtra = 0;
    #endregion

    #region dependencies
    TimerBehavior attackTimer;
    Animator[] _animators;

    ZombieBehavior _ZBEH;
    #endregion

    #region INITandListeners

    private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += UpdateAnimation;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= UpdateAnimation;
    }

    void Awake()
    {
        _ZBEH = GetComponent<ZombieBehavior>();
    }

    void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
        attackTimer = gameObject.AddComponent<TimerBehavior>();
        Calc_Reach_type();
        Calc_Shase_type();
        Calc_DEath_type();
    }
    #endregion

    #region PublicMethods
    public void Trigger_HeadShotAnim()
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetTrigger("trigHeadShot");
        }
    }

    public void StopAttackTimer()
    {
        attackTimer.StopTimer();
    }
    #endregion

    #region PrivateMethods
    void Calc_Shase_type()
    {
        multiplyerExtra = Random.Range(0, 5);

        if (UseBobingAnim)
        {
            ChaseType = Random.Range(0, 3);
            RealMultiplyer = multiplier + ((float)multiplyerExtra) / 10;
        }
        else
            if (UseHyperChase)
        {
            ChaseType = 3;
            RealMultiplyer = 0.5f;

        }
        else
        {
            ChaseType = 0;
            RealMultiplyer = multiplier + ((float)multiplyerExtra) / 10;
        }
    }

    void Calc_Reach_type() {
        reachType = Random.Range(0, numberOfReachAnimation - 1);
    }

    void Calc_DEath_type() {
        deathType = Random.Range(0, numberOfDeathAnimations - 1);
    }

    void UpdateAnimation(ZombieState argNewZombieState)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetInteger("state", (int)argNewZombieState);
            _animators[i].SetFloat("multiplier", RealMultiplyer);
        }

    

        if (argNewZombieState == ZombieState.PAUSED) { PauseZombieAnimation(); }
        if (argNewZombieState == ZombieState.ATTACKING) { Attack(); }
        if (argNewZombieState == ZombieState.REACHING) { Reach(); }


    }
    //************************************************pausing and continuing
    void PauseZombieAnimation()
    {
        multiplierBeforePause = multiplier;

        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].speed = 0;
        }

        multiplier = 0.0f;       
    }

    void ContinueZombieAnimation()
    {
        multiplier = multiplierBeforePause;
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].speed = multiplier;
        }
         
    }
    //*******************************************************pausing and continuing
    void Reach()
    {
        // turn model to face camera
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
        Attack();
    }

    void Attack()
    {
        if (_ZBEH.CurZombieState == ZombieState.PAUSED || _ZBEH.CurZombieState == ZombieState.DEAD)
            return;

        DamageThePlayer();
    }

    void DamageThePlayer() {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AttackPlayer();
            attackTimer.StartTimer(attackSpeed, Attack, false);
        }
        else { Debug.Log("no Gm can't attack player"); }
    }
    #endregion
}
