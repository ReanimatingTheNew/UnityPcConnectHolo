using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{

    private float m_SpawnDelay;
   
    private WaveStandard m_Wave;

    public void Init(WaveStandard wave, float spawnDelay)
    {
        m_SpawnDelay = spawnDelay;
        m_Wave = wave;   
        IsCoolingDown = false;
    }

    public bool IsCoolingDown;

    public void StartCoolingDown() { StartCoroutine(CoolDownCoroutine()); }

    private IEnumerator CoolDownCoroutine()
    {
        IsCoolingDown = true;
        yield return new WaitForSeconds(m_SpawnDelay);
        IsCoolingDown = false;
        //ToSayImFreeNowStartYourMethodOffindingwhosfreeItcouldbemeifyouneedittospawn
        m_Wave.RequestAvailableSP();
    }

    public void ResetMe()
    {
        if (IsCoolingDown) {
            StopCoroutine(CoolDownCoroutine());
        }
        IsCoolingDown = false;
    }

    public void StopMe()
    {
        IsCoolingDown = false;
    }


    /*
     * 
    
        public void Init(WaveStandard wave, float spawnDelay)
    {
        m_SpawnDelay = spawnDelay;
        m_Wave = wave;
        m_Lock = false;
        m_IsSpawning = false;

        CoolindDown = false;
    }

        private bool m_Lock;
    private bool m_IsSpawning;

    private IEnumerator ie_AttemptToSpawnOLD()
    {
        yield return new WaitForSeconds(m_SpawnDelay);

        Debug.Log("6 spawnpoint ieAttempt");
        m_Lock = false;
        m_IsSpawning = false;
        m_Wave.AttemptZombieSpawn();
        m_Lock = false;
    }

    public bool IsSpawning
    {
        get { return m_IsSpawning; }

    }

    public bool IsLocked
    {
        get { return m_Lock; }
    }

    
   public void ToggleSpawning(bool toggle)
   {
       Debug.Log("5 spawnpoints Toggle");
       m_IsSpawning = toggle;
       if (toggle)
       {

           if (!m_Lock)
           {
               m_Lock = true;
               StartCoroutine(CoolDownCoroutine());
           }

           m_Lock = true;

       }
       else
       {
           StopCoroutine(CoolDownCoroutine());
           m_Lock = false;
       }
   }
   */

}
