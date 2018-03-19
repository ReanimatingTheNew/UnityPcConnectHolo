// @Author Nabil Lamriben ©2018
using HoloToolkit.Unity;
using System.Collections;
using UnityEngine;

public class StartButtonControle : MonoBehaviour {

 
    public GameObject EXPLOSION;

    UAudioManager _uaudio;
    
    const int maxhitcounts = 3;
    int hitsTaken = 0;
    private void Start()
    {
        _uaudio = GetComponent<UAudioManager>();

    }
    public void MakeInvisible()
    {
        AmIOn = false;
        this.gameObject.SetActive(false);
    }
    public bool GetAmIOn() { return AmIOn; }
    bool AmIOn = true;

    IEnumerator waitWhileCGCplays() {
       
        yield return new WaitForSeconds(1.2f);
        GameManager.Instance.CheckStartGame();

        DOExplosion();
    }

    void DOExplosion()
    {
       
        Instantiate(EXPLOSION, transform.position, Quaternion.identity);
        MakeInvisible();

    }
    bool hasreachedMax = false;
    public void TakeHit(Bullet bullet)
    {
        if (hasreachedMax) return;
        if (bullet == null) return;
        hitsTaken++;

        if (hitsTaken == 1) {
             _uaudio.PlayEvent("_c1");
          //  StartCoroutine(waitPlayC1("_c1"));
        }else

        if (hitsTaken == 2)
        {
            _uaudio.PlayEvent("_g2");
            //StartCoroutine(waitPlayC1("_g2"));
        }
        else

        if (hitsTaken == 3)
        {
             _uaudio.PlayEvent("_c3");
            //StartCoroutine(waitPlayC1("_c3"));
            hasreachedMax = true;
            StartCoroutine(waitWhileCGCplays());
        }
     

        //if (hitsTaken >= maxhitcounts)
        //{
        //    StartCoroutine(waitWhileCGCplays());// DOExplosion();
        //}

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasreachedMax = true;
            StartCoroutine(waitWhileCGCplays());
        }
      
    }

    IEnumerator waitPlayC1(string argsoundname) {
        yield return new WaitForSeconds(0.2f);
        _uaudio.PlayEvent(argsoundname);
    }
}
