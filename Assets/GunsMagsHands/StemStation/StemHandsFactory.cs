// @Author Nabil Lamriben ©2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemHandsFactory : MonoBehaviour {

 
 
    public GameObject HandsObj;
    public GameObject MainHandObj;
    public GameObject OffHandObj;

    GameObject Factory_MAIN_Hand;
    GameObject Factory_OFF_Hand;

    public GameObject FactoryBuild_MainHand(bool argIsRightySetup, Transform StemObjTransform, GunsBundle argGunsBun)
    {
        Factory_MAIN_Hand = Instantiate(MainHandObj, StemObjTransform.position, StemObjTransform.rotation) as GameObject;

        if (argIsRightySetup) { Factory_MAIN_Hand.transform.localScale = new Vector3(1, 1, 1); }

        else
        {
            Factory_MAIN_Hand.transform.localScale = new Vector3(-1, 1, 1);
            Factory_MAIN_Hand.transform.position = new Vector3(StemObjTransform.parent.position.x, StemObjTransform.position.y, StemObjTransform.position.z);
        }

        Factory_MAIN_Hand.name = "PlayerShootyHand";
        Factory_MAIN_Hand.transform.parent = StemObjTransform.transform;
        Place_GUN_bunObj(argGunsBun);
        Factory_MAIN_Hand.GetComponent<BaseHandScript>().InitializedThisHand( argGunsBun);
        return Factory_MAIN_Hand;
    }

    public GameObject FactoryBuild_OffHand(bool argIsRightySetup, Transform StemObjTransform, MagsBundle argMagsbun)
    {
        Factory_OFF_Hand = Instantiate(OffHandObj, StemObjTransform.position, StemObjTransform.rotation) as GameObject;
        if (argIsRightySetup)
        {
            Factory_OFF_Hand.transform.localScale = new Vector3(-1, 1, 1);
            Factory_OFF_Hand.transform.localEulerAngles = new Vector3(StemObjTransform.rotation.x, 0, -90);
        }     
        else
        {
            Factory_OFF_Hand.transform.localScale = new Vector3(1, 1, 1);
            Factory_OFF_Hand.transform.localEulerAngles = new Vector3(StemObjTransform.rotation.x, 0, 90);
        }

        Factory_OFF_Hand.name = "PlayerLoadyHand";
        Factory_OFF_Hand.transform.parent = StemObjTransform.transform;
        Place_MAG_bunObj(argMagsbun);
        Factory_OFF_Hand.GetComponent<BaseHandScript>().InitializedThisHand( argMagsbun);
        return Factory_OFF_Hand;
    }

    void Place_GUN_bunObj(GunsBundle _argGunsBun)
    {
         Transform _gungrip = Factory_MAIN_Hand.GetComponent<BaseHandScript>().MyBundleBone;
        _argGunsBun.transform.position = _gungrip.position;
        _argGunsBun.transform.rotation = _gungrip.rotation;
        _argGunsBun.transform.parent = _gungrip;             
    }

  

    void Place_MAG_bunObj( MagsBundle _argMagsbun)
    {
        Transform _maggrip = Factory_OFF_Hand.GetComponent<BaseHandScript>().MyBundleBone;       
        _argMagsbun.transform.position = _maggrip.position;
        _argMagsbun.transform.rotation = _maggrip.rotation;
        _argMagsbun.transform.parent = _maggrip;
    }
}

