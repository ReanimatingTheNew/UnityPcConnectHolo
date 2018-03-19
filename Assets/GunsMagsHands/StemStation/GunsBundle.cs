// @Author Nabil Lamriben ©2018
using UnityEngine;

public class GunsBundle : MonoBehaviour ,IBundle{

    public GameObject gunM1911;
    public GameObject gunMac11;
    public GameObject gunColt;
    public GameObject gunShotgun;

    GameObject M1911;
    GameObject Mac11;
    GameObject Colt;
    GameObject Shotgun;

    bool _isEquipedGunVisible;
    GameObject _curGunObject;
    IGun CurGunScript;

    public IGun GetActiveGunScript() { return CurGunScript; }
 
    

    private void Awake()
    {
        M1911 = Instantiate(gunM1911, this.transform.position, this.transform.rotation);
        M1911.name = "Gun_M1911";
        M1911.transform.parent = this.transform;

        Mac11 = Instantiate(gunMac11, this.transform.position,   this.transform.rotation );
        Mac11.name = "Gun_Mac11";
        Mac11.transform.parent = this.transform;

        Colt = Instantiate(gunColt, this.transform.position, this.transform.rotation);
        Colt.name = "Gun_Colt";
        Colt.transform.parent = this.transform;

        Shotgun = Instantiate(gunShotgun, this.transform.position, this.transform.rotation);
        Shotgun.name = "Gun_Shotgun";
        Shotgun.transform.parent = this.transform;

        HideAllMyThings();
    }



    #region InterfaceRegion

    public bool IsMyThingShowing()
    {
        if (_curGunObject == null) { return false; }
        if (!_isEquipedGunVisible) { return false; }
        return true;
    }

    public void HideAllMyThings()
    {
        M1911.SetActive(false);
        Colt.SetActive(false);
        Mac11.SetActive(false);
        Shotgun.SetActive(false);
    }


    // tracking meter //stemplayerctrl.ItPutsGunInHand or maginhand ->  handscript.ANYHAD_EQUIP  
    public void SetMyCurrBunThing(int argIndexEnum)
    {
        //unequip previous weapon
        if (CurGunScript != null)
        {
            _curGunObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("tried to equip but no weapon was found");
        }
        switch ((GunType)argIndexEnum)
        {
            case GunType.PISTOL:
                _curGunObject = M1911;
                break;
            case GunType.MAGNUM:
                _curGunObject = Colt;
                break;
            case GunType.UZI:
                _curGunObject = Mac11;
                break;
            case GunType.SHOTGUN:
                _curGunObject = Shotgun;
                break;
        }

        if (_curGunObject != null)
        {
            CurGunScript = _curGunObject.GetComponent<IGun>();

            // tracking meter //stemplayerctrl.ItPutsGunInHand or maginhand ->  handscript.ANYHAD_EQUIP()
        }
        else {
            Debug.LogWarning("no curr weapon Onject ");
        }
    }


    public void ShowMyCurrBunThing()
    {
        Set_EquipedGunObject_Visible(true);
    }

    public void HideMyCurrBunThing()
    {
        Set_EquipedGunObject_Visible(false);
    }


    #endregion

    ////stemkitmanager.start() -> playerHandsCTRL.INit()
    void Set_EquipedGunObject_Visible(bool argVisible)
    {        // make equipped clip active
        _isEquipedGunVisible = argVisible;
        if (_curGunObject != null)
        {
            _curGunObject.SetActive(argVisible);
           StemKitMNGR.Call_SetCurIgunTo(CurGunScript);
        }
        else { Debug.LogWarning("no curr weapon Onject "); }
    }

}
