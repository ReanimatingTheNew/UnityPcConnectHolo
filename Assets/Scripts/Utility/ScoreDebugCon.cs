using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDebugCon : Singleton<ScoreDebugCon>
{

    public TextMesh PointsWaveMesh;
    public TextMesh ShotsMesh;
    public TextMesh HitsMesh;
    public TextMesh MissMesh;
    public TextMesh WasHeadMesh;
    public TextMesh HeadShotMesh;
    public TextMesh TorsoMesh;
    public TextMesh LimbMesh;
    public TextMesh CurStreakMesh;
    public TextMesh MaxStreakMesh;
    public TextMesh CntStreakMesh;

    private void Start()
    {
        if (GameSettings.Instance != null) {
            if (!GameSettings.Instance.IsTestModeON) {

                PointsWaveMesh.text = "";
                ShotsMesh.text = "";
                WasHeadMesh.text = "";
                HitsMesh.text = "";
                MissMesh.text = "";
                HeadShotMesh.text = "";
                TorsoMesh.text = "";
                LimbMesh.text = "";
                CurStreakMesh.text = "";
                MaxStreakMesh.text = "";
                CntStreakMesh.text = "";
            }
        }
    }

    public void update_WAVEPoints(int argwavepoint) {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                PointsWaveMesh.text = "wave points=" + argwavepoint.ToString();
            }
        }
    }

    public void update_shotsfired(int shotsfired)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                ShotsMesh.text = "shots fired=" + shotsfired.ToString();
            }
        }
    }

    public void update_wasHead(bool washead )
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                WasHeadMesh.text = "ishead?=" + washead.ToString();
            }
        }
    }

    public void update_hit(int hit)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                HitsMesh.text = "hit =" + hit.ToString();
            }
        }
    }

    public void update_miss(int miss)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                MissMesh.text = "miss =" + miss.ToString();
            }
        }
    }

    public void update_heads(int heads)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                HeadShotMesh.text = "head =" + heads.ToString();
            }
        }
    }
    public void update_torsos(int torsos)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                TorsoMesh.text = "torso =" + torsos.ToString();
            }
        }
    }
    public void update_limb(int limbs)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                LimbMesh.text = "limb =" + limbs.ToString();
            }
        }
    }
    public void update_curStrek(int curStreak)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                CurStreakMesh.text = "curstreak =" + curStreak.ToString();
            }
        }
    }

    public void update_MaxStrek(int maxStreak)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                MaxStreakMesh.text = "maxStreak =" + maxStreak.ToString();
            }
        }
    }
    public void update_CNTStrek(int cntStreak)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsTestModeON)
            {
                CntStreakMesh.text = "CNTStreak =" + cntStreak.ToString();
            }
        }
    }
    

}
