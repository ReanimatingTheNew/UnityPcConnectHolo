// @Author Nabil Lamriben ©2018
using UnityEngine;

public class ZombieAnimCTRL : MonoBehaviour {


    public bool Normal = true;
    public bool eater;
    public bool fromgrave;

    bool nolegs = false;
    Animator[] animator;            // the animator component of the model we want to animate
    ZombieState state;
    int deathType = 4;
    int reachType = 2;
    int ChaseType = 2;
    // Use this for initialization
    void Start() {
        state = ZombieState.IDLE;
        animator = GetComponentsInChildren<Animator>();

        Debug.Log((int)state + " " + state);
        UpdateAnimation();
    }



    /// <summary>
    /// 0 chase normal
    /// 1 chase Bob2
    /// 2 chase BobUpDown
    /// 3 Hyperchase 
    /// </summary>
    //void calcRandChasetype()
    //{
    //    multiplyerExtra = Random.Range(0, 5);
    //    if (UseBobingAnim)
    //    {
    //        ChaseType = Random.Range(0, 3);
    //        RealMultiplyer = multiplier + ((float)multiplyerExtra) / 10;
    //    }
    //    else
    //        if (UseHyperChase)
    //    {
    //        ChaseType = 3;
    //        RealMultiplyer = 0.5f;
    //    }
    //    else
    //    {
    //        ChaseType = 0;
    //        RealMultiplyer = multiplier + ((float)multiplyerExtra) / 10;
    //    }
    //}
    void UpdateAnimation()
    {

        // deathType = Random.Range(0, numberOfDeathAnimations - 1);
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].SetInteger("state", (int)state);
            animator[i].SetFloat("multiplier", 1.0f);
            animator[i].SetInteger("deathType", deathType);
            animator[i].SetInteger("reachType", reachType);
            animator[i].SetInteger("chaseType", ChaseType);

            animator[i].SetBool("isnormal", Normal);
            animator[i].SetBool("iseater", eater);
            animator[i].SetBool("isfromgrave", fromgrave);
            animator[i].SetBool("ihavenolegs", nolegs);

        }
    }

    // Update is called once per frame
    void Update() {
        KEYBOARDIINPUT();
        UpdateAnimation();
    }
    void KEYBOARDIINPUT()
    {
 
        if (Input.GetKeyDown(KeyCode.Alpha0)) { state = ZombieState.IDLE; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { state = ZombieState.WALKING; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { state = ZombieState.CHASING; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { state = ZombieState.ATTACKING; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { state = ZombieState.DEAD; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { state = ZombieState.REACHING; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { state = ZombieState.PAUSED; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { state = ZombieState.CRAWL; Debug.Log((int)state + " " + state); }
        if (Input.GetKeyDown(KeyCode.Q)) { ChaseType = 1; }
        if (Input.GetKeyDown(KeyCode.W)) { ChaseType = 2; }

        if (Input.GetKeyDown(KeyCode.E)) { deathType = 1; }
        if (Input.GetKeyDown(KeyCode.R)) { deathType = 2; }
        if (Input.GetKeyDown(KeyCode.T)) { deathType = 3; }
        if (Input.GetKeyDown(KeyCode.Y)) { deathType = 4; }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (Animator ator in animator)
            {
                ator.SetTrigger("trigHeadShot");
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) { reachType = 1; }
        if (Input.GetKeyDown(KeyCode.S)) { reachType = 2; }
    }
}
