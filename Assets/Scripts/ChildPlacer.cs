// @Author Nabil Lamriben ©2017
using UnityEngine;

public class ChildPlacer : MonoBehaviour {
    Vector3 Colt_MainHand_PositionOffset = new Vector3(0f,0.0726f,-0.03f);
    Vector3 Colt_MainHand_Rotation = new Vector3(0f, 180f, 0);

    Vector3 M1911_MainHand_PositionOffset = new Vector3(0f, 0.0628f, -0.0119f);
    Vector3 M1911_MainHand_Rotation = new Vector3(0f, 180f, 0f);

    Vector3 Mac11_MainHand_PositionOffset = new Vector3(0f, 0.107f, -0.03f);
    Vector3 Mac11_MainHand_Rotation = new Vector3(0f, 180f, 0f);

    Vector3 SOShotgun_MainHand_PositionOffset = new Vector3(0f, 0.073f, 0f);
    Vector3 SOShotgun_MainHand_Rotation = new Vector3(0f, 0f, 0f);

   



    Vector3 Colt_MAG_InGun_PositionOffset = new Vector3(0.02f, 0f, 0f);
    Vector3 Colt_MAG_InGun_Rotation = new Vector3(90f, 180f, 0f);

    Vector3 M1911_MAG_InGun_PositionOffset = new Vector3(0.0417f, 0f, -0.1134f);
    Vector3 M1911_MAG_InGun_Rotation = new Vector3(0f, 159.5f, 90);

    Vector3 Mac11_MAG_InGun_PositionOffset = new Vector3(0f, 0f, 0f);
    Vector3 Mac11_MAG_InGun_Rotation = new Vector3(270f, 270f, 90f);

    Vector3 SOShotgun_MAG_InGun_PositionOffset = new Vector3(0f, 0f, 0.0453f);
    Vector3 SOShotgun_MAG_InGun_Rotation = new Vector3(90f, 270f, 0f);

    //***********************************OFFHAND
    Vector3 Colt_MAG_OFFHAND_PositionOffset = new Vector3(0f, 0.07f, -.15f);
    Vector3 Colt_MAG_OFFHAND_Rotation = new Vector3(90f, 0f, 0f);

    Vector3 M1911_MAG_OFFHAND_PositionOffset = new Vector3(0f, 0.0263f, 0.3367f);
    Vector3 M1911_MAG_OFFHAND_Rotation = new Vector3(0f, 0f, 90);

    Vector3 Mac11_MAG_OFFHAND_PositionOffset = new Vector3(-0.09299308f, 0.04079999f, 0.2501176f);
    Vector3 Mac11_MAG_OFFHAND_Rotation = new Vector3(89.98f, 0f, 67.318f);

    Vector3 SOShotgun_MAG_OFFHAND_PositionOffset = new Vector3(-0.0053f, 0.0262f, 0.233f);
    Vector3 SOShotgun_MAG_OFFHAND_Rotation = new Vector3(88.438f, 0f, 169f);
    //***********************************OFFHAND


    Vector3 DEFAULT_PositionOffset = new Vector3(0f, 0f, 0f);
    Vector3 DEFAULT_Rotation = new Vector3(0f, 0f, 0f);

    public WeaponContext context;

 

     Vector3 ChildLocation;
     Vector3 ChildRotation;

	void Start () {
        
        switch (context) {
            case WeaponContext.Colt_GUN_MainHand:
                ChildLocation = Colt_MainHand_PositionOffset;
                ChildRotation = Colt_MainHand_Rotation;
                break;
            case WeaponContext.M1911_GUN_MainHand:
                ChildLocation = M1911_MainHand_PositionOffset;
                ChildRotation = M1911_MainHand_Rotation;
                break;
            case WeaponContext.Mac11_GUN_MainHand:
                ChildLocation = Mac11_MainHand_PositionOffset;
                ChildRotation = Mac11_MainHand_Rotation;
                break;
            case WeaponContext.SOShotgun_GUN_MainHand:
                ChildLocation = SOShotgun_MainHand_PositionOffset;
                ChildRotation = SOShotgun_MainHand_Rotation;
                break;
                //*******************************************************
            case WeaponContext.Colt_MAG_inGun:
                ChildLocation = Colt_MAG_InGun_PositionOffset;
                ChildRotation = Colt_MAG_InGun_Rotation;
                break;
            case WeaponContext.M1911_MAG_inGun:
                ChildLocation = M1911_MAG_InGun_PositionOffset;
                ChildRotation = M1911_MAG_InGun_Rotation;
                break;
            case WeaponContext.Mac11_MAG_inGun:
                ChildLocation = Mac11_MAG_InGun_PositionOffset;
                ChildRotation = Mac11_MAG_InGun_Rotation;
                break;
            case WeaponContext.SOShotgun_MAG_inGun:
                ChildLocation = SOShotgun_MAG_InGun_PositionOffset;
                ChildRotation = SOShotgun_MAG_InGun_Rotation;
                break;
            //******************************************************

            case WeaponContext.Colt_MAG_OffHand:
                ChildLocation = Colt_MAG_OFFHAND_PositionOffset;
                ChildRotation = Colt_MAG_OFFHAND_Rotation;
                break;
            case WeaponContext.M1911_MAG_OffHand:
                ChildLocation = M1911_MAG_OFFHAND_PositionOffset;
                ChildRotation = M1911_MAG_OFFHAND_Rotation;
                break;
            case WeaponContext.Mac11_MAG_OffHand:
                ChildLocation = Mac11_MAG_OFFHAND_PositionOffset;
                ChildRotation = Mac11_MAG_OFFHAND_Rotation;
                break;
            case WeaponContext.SOShotgun_MAG_OffHand:
                ChildLocation = SOShotgun_MAG_OFFHAND_PositionOffset;
                ChildRotation = SOShotgun_MAG_OFFHAND_Rotation;
                break;
            //***********************************************

            case WeaponContext.Colt_GUN_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.M1911_GUN_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.Mac11_GUN_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.SOShotgun_GUN_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.Colt_MAG_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.M1911_MAG_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            case WeaponContext.SOShotgun_MAG_onRack:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;
            default:
                ChildLocation = DEFAULT_PositionOffset;
                ChildRotation = DEFAULT_Rotation;
                break;

        }
     
        this.gameObject.transform.GetChild(0).transform.localPosition = ChildLocation;
        this.gameObject.transform.GetChild(0).transform.localEulerAngles = ChildRotation;
    }
}
