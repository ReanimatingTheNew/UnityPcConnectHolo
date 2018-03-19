using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBlockScript : MonoBehaviour {


    public void TakeHit(Bullet bullet)
    {
        Debug.Log("hit start block");
        GameManager.Instance.CheckStartGame();
        Destroy(this.gameObject);
    }
}
