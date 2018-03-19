// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class Bullet: BaseBullet{

   
    [Tooltip("Damage this bullet causes")]
    public int damage;

    [Tooltip("Raycast layer of all objects this bullet should collide with")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    [Tooltip("The number of the enemy layer")]
    public int enemyLayer;

    [Tooltip("Pickup layer")]
    public int pickupLayer;

    [HideInInspector]
    public RaycastHit hitInfo;

    // an array for the possible bullet collision sounds
    [Tooltip("Drag bullet collision sounds into here")]
    public AudioClip[] bulletSounds;
    // an array for the possible bullet hole marks left
    [Tooltip("Drag bullet hole marks into here")]
    public GameObject[] bulletHoles;
    // an array for the possible bullet particle effects
    [Tooltip("Drag bullet particle effects into here")]
    public GameObject[] bulletParticles;

    // get the audiosource to a ref
    private AudioSource aS;
    // create a local ref to the bullet hole being spawned
    private GameObject bH_Temp;
    // create a local spot for the spawned particle
    private GameObject bP_Temp;

    void Record_LINE_ZombieHit(Vector3 start, Vector3 end) { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; };  GameManager.Instance.AddZombieHitRelativeToPAthfinder(start, end); } else { Debug.Log("sorry no gamemanager"); } }
    void Record_LINE_Miss(Vector3 start, Vector3 end) { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.Add_FailedShots_RelativeToPAthfinder(start, end); } }

    void BulletMissed() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_IncrementBullets_Missed_ZombieCNT(); } else { Debug.Log("sorry no gamemanager"); } }
    void BulletHitZombie() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_IncrementBullet_Hit_ZombieCNT(); } else { Debug.Log("sorry no gamemanager"); } }
    void BulletFired() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_IncrementBulletsShotCNT(); } else { Debug.Log("sorry no gamemanager"); } }

    void Bullet_HIts_ZHead() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_headShotCNT(); } else { Debug.Log("sorry no gamemanager"); } }
    void Bullet_HIts_ZTorso() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_torsoShotCNT(); } else { Debug.Log("sorry no gamemanager"); } }
    void Bullet_HItsZlimb() { if (GameManager.Instance != null) { if (!GameManager.Instance.IsGameStarted()) { return; }; GameManager.Instance.GetScoreMAnager().Update_limbShotCNT(); } else { Debug.Log("sorry no gamemanager"); } }


    void Newstart()
    {
        //Shot ->  Headshot  sh  or sm  shot miss
        //fired
        BulletFired();

        // on start raycast from bullet position to hit enemies, pickups, or walls
        if (Physics.Raycast(transform.position, transform.forward * -1, out hitInfo, 40.0f, RaycastLayerMask))
        {
            if (hitInfo.collider.gameObject.layer == enemyLayer)
            {
                Record_LINE_ZombieHit(transform.position, hitInfo.point);
                hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this); //will send to zombiebehavior which will assign scores depending on wether the bullet was a lethal or nonlethal shot
                BulletHitZombie();
                if (hitInfo.collider.gameObject.CompareTag("ZombieHead"))
                {
                    Bullet_HIts_ZHead();
                    UpdateStreak_Hit(hitInfo.point);
                }
                if (hitInfo.collider.gameObject.CompareTag("ZombieTorso"))
                {

                    Bullet_HIts_ZTorso();
                    UpdateStreaks_Miss();
                }
                if (hitInfo.collider.gameObject.CompareTag("ZombieLimb"))
                {
                    Bullet_HItsZlimb();
                    UpdateStreaks_Miss();
                }

              
            }
            else
            if ( hitInfo.collider.gameObject.layer == pickupLayer)
            {
                hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this);
                BulletMissed();
                UpdateStreaks_Miss();
            }

            else
            {
                Record_LINE_Miss(transform.position, transform.position + transform.forward * 20);
                BulletMissed();
                UpdateStreaks_Miss();

                if (hitInfo.collider.gameObject.tag == "Interactive" || hitInfo.collider.gameObject.tag == "MetalTag")
                {
                    // check the info of what we hit
                    //print("** Tag of hit object was: " + hitInfo.collider.gameObject.tag + " && hitSpot was: " + hitInfo.collider.gameObject.transform);
                    // call the bullet effect function ** May need changes
                    BulletEffects(hitInfo.collider.gameObject.tag, hitInfo.point);
                    hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this);
                    PlaceBulletHoleMetal(hitInfo);
                }
                else
                    // call the bullet effect function ** May need changes
                    BulletEffects("WoodTag", hitInfo.point);
                    PlaceBulletHoleWood(hitInfo);

               
            }
        }
        else
        {
            //We dident even hit a wall or anything
            BulletMissed();
            UpdateStreaks_Miss();
        }
        Destroy(gameObject);
    }

    void UpdateStreaks_Miss() {
        if (GameManager.Instance != null) {
            if (!GameManager.Instance.IsGameStarted()) { return; };
            GameManager.Instance.GetStreakManager().Set_StreakBreake();
        }
        else { Debug.Log("sorry no gamemanager"); }
    }


    void UpdateStreak_Hit(Vector3 hitLocatoin) {
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.IsGameStarted()) { return; };
            GameManager.Instance.GetStreakManager().Test_Streak(hitLocatoin);
        }
        else { Debug.Log("sorry no gamemanager"); }
    }


    void Start()
    {
        Newstart();
    }

    // a function for the bullet effects | takes a string (hit tag name) and transform (hit transform)
    private void BulletEffects(string tag, Vector3 hitSpot)
    {
        // sent a test message
        Debug.Log("Running the 3 step bullet effects function");

        /// 1st ____________________________ 1st check/grab audio

        //if we have an audiosource
        if (GetComponent<AudioSource>() != null)
        {
            // get the audiosource
            aS = GetComponent<AudioSource>();

            // for each clip in the bullet clips
            foreach (AudioClip bS in bulletSounds)
            {
                // if the name of the sound contains the tag
                if (bS.name.Contains(tag))
                {
                    //display message
                    Debug.Log("Audio Clip " + bS.name +  " contains: " + tag);
                    // set audioclip
                    aS.clip = bS;
                    // play the sound
                    aS.Play();
                }// end of name contains tag
                else
                // if it doesnt not contain tag in name
                {
                    //display message
                    Debug.Log("Audio Clip " + bS.name + " does not contain: " + tag);
                }// end of tag not in name

            }// end of for each bullet sound

        }// end of have audiosource
        else
        // if we have no audioSource attached to this object
        {
            // display message
            Debug.Log("Please add an audioSource to " + transform.name);
        }// end of no audioSource

        /// 2nd ________________________________ 2nd grab the bullet mark

        // check if we have any bulletHoles in the array
        if (bulletHoles.Length > 0)
        {           

            // for each bullet hole in the array
            foreach (GameObject bH in bulletHoles)
            {
                // if the name of the gameobject matches the tag
                if (bH.name.Contains(tag))
                {
                    //display message
                    Debug.Log("BulletHole " + bH.name + " contains: " + tag);
                    // assign and create one at the hit spot
                    bH_Temp = Instantiate(bH, hitSpot, bH.transform.rotation);
                    // destroy the instantiated object after X seconds
                    Destroy(bH_Temp, 1);

                }// end of name contains tag
                else
                // if it doesnt not contain tag in name
                {
                    //display message
                    Debug.Log("BulletHole " + bH.name + " does not contain: " + tag);
                }// end of tag not in name

            }// end of for each bullet hole

        }// end of if bulletHoles array greater than 0
        else
        // if there is nothing in our bullet hole array
        {
            // display message
            Debug.Log("Please place bullet hole gameObjects in the array on " + transform.name);
        }// end of nothing in array

        /// 3rd _____________________________ 3rd grab a bullet particle effect

        // if we have any particles in our bullet particles array
        if (bulletParticles.Length > 0)
        {


            // for each bullet particle in the array
            foreach (GameObject bP in bulletParticles)
            {               
                // if the name of the gameobject matches the tag
                if (bP.name.Contains(tag))
                {
                    //display message
                    Debug.Log("BulletParticle " + bP.name + " contains: " + tag);
                    // assign and create one at hitspot
                    bP_Temp = Instantiate(bP, hitSpot, bP.transform.rotation);
                    // destroy the instantiated object after X seconds
                    Destroy(bP_Temp, 1);

                }// end of name contains tag
                else
                // if it doesnt not contain tag in name
                {
                    //display message
                    Debug.Log("BulletParticle " + bP.name + " does not contain: " + tag);
                }// end of tag not in name


            }// end of for each bullet particle


        }// end of bullet Particles array > 0
        else
        // if we have no bullet particle effects
        {
            // display message
            Debug.Log("Please add bullet Particle effects to the array on " + transform.name);
        }// end of nothing in bullet particles array

    }// end of bullet effects function

    void originalstart() {

        // on start raycast from bullet position to hit enemies, pickups, or walls
        if (Physics.Raycast(transform.position, transform.forward * -1, out hitInfo, 40.0f, RaycastLayerMask))
        {
            if (hitInfo.collider.gameObject.layer == enemyLayer || hitInfo.collider.gameObject.layer == pickupLayer)
            {
                hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this);
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.curgamemode != ARZGameModes.GameNoStem)
                    {
                        Record_LINE_ZombieHit(transform.position, hitInfo.point);
                       // GameManager.Instance.GetScoreMAnager().Handle_BulletFired_landedOnzombie(true);
                    }
                }
                else { Debug.Log("sorry no gamemanager"); }
            }

            else
            {
                Record_LINE_Miss(transform.position, transform.position + transform.forward * 20);
                if (GameManager.Instance != null)
                {
                   // GameManager.Instance.GetScoreMAnager().Handle_BulletFired_landedOnzombie(false);
                }
                if (hitInfo.collider.gameObject.tag == "Interactive" || hitInfo.collider.gameObject.tag == "MetalTag")
                {
                    print("**** Tag of hit object was: " + hitInfo.collider.gameObject.tag + " && hitSpot was: " + hitInfo.collider.gameObject.transform);
                    hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this);                    
                    PlaceBulletHoleMetal(hitInfo);
                    // call the bullet effect function ** May need changes
                    //BulletEffects(hitInfo.collider.gameObject.tag, hitInfo.collider.gameObject.transform);
                }
                else                   
                PlaceBulletHoleWood(hitInfo);
                // call the bullet effect function ** May need changes
                //BulletEffects(hitInfo.collider.gameObject.tag, hitInfo.collider.gameObject.transform);
            }
        }
        Destroy(gameObject);
    }


    void PlaceBulletHoleWood(RaycastHit hitInfo)
    {
        int rand4 = Random.Range(0, base.bulletHoles_Wood.Length);
        Instantiate(base.bulletHoles_Wood[rand4], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
    }

    void PlaceBulletHoleMetal(RaycastHit hitInfo)
    {
        int rand4 = Random.Range(0, base.bulletHoles_Metal.Length);
        Instantiate(base.bulletHoles_Metal[rand4], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
    }
}
