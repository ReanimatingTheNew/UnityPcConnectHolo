using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [Tooltip("What type of powerup?")]
    public PowerUpType type;

    [Tooltip("Time in seconds before object is automatically destroyed.")]
    public float destroyTime;

    [Tooltip("The effect to instantiate upon successful pickup.")]
    public GameObject pickupEffect;

    KillTimer killTimer;

    // Use this for initialization
    void Start()
    {
        killTimer = gameObject.AddComponent<KillTimer>();
        killTimer.StartTimer(destroyTime);
    }

    public void TakeHit(Bullet bullet)
    {
        // instantiate effect, award weapon, and destroy self
        Instantiate(pickupEffect, transform.position, transform.rotation);
        //GameManager.Instance.DecrementCounter();

        Destroy(gameObject);
    }
}
