// @Author Nabil Lamriben ©2018

using UnityEngine;

public class ZombieEffects : MonoBehaviour {
    #region PublicVars
    public GameObject bloodFXObject;
    #endregion

    #region PrivateVars
    BloodFX _bloodFX;
    #endregion

    #region INIT
    void Start () {
        _bloodFX = bloodFXObject.GetComponent<BloodFX>();
    }
    #endregion

    #region PublicMethods
    public void Boold_On_Head(Bullet bullet) {
        if (GameSettings.Instance.IsBloodOn)
            _bloodFX.HeadShotFX(bullet.hitInfo);
    }
    public void Boold_On_Torso(Bullet bullet) {
        if (GameSettings.Instance.IsBloodOn)
            _bloodFX.TorsoShotFX(bullet.hitInfo);
    }
    public void Boold_On_Limb(Bullet bullet) {
        if (GameSettings.Instance.IsBloodOn)
            _bloodFX.LimbShotFX(bullet.hitInfo);
    }
    #endregion

}
