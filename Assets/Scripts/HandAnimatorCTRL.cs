using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimatorCTRL : MonoBehaviour {

    Animator _anim;

    //private void OnEnable()
    //{

    //   StemKitMNGR.OnGunFlavorChanged +=  GunFlavorChanged;
    //    StemKitMNGR.OnMAGFlavorChanged += MagFlavorChanged;
    //}
    //private void OnDisable()
    //{
    //    StemKitMNGR.OnGunFlavorChanged -= GunFlavorChanged;
    //    StemKitMNGR.OnMAGFlavorChanged -= MagFlavorChanged;
    //}

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetHAndType(int argIsMainGunHand) {
        _anim.SetInteger("AnimHandType", argIsMainGunHand);

    }

    //stemplayerhandctrl will set this when gun state change
    public void MainHandAnimateHoldGun(GunType newguntype) {
        int gt = (int)newguntype;
       // Debug.Log(gt);
        _anim.SetInteger("AnimGunType", gt);
    }

    public void offhandAnimateHOldMag(Ammunition ArgAmmo) {
        int gt = (int)ArgAmmo;
       // Debug.Log(gt);
        _anim.SetInteger("AnimMagType", gt);
    }

    public void AnyHandAnimHOld(int Index) {
        MainHandAnimateHoldGun((GunType)Index);
        offhandAnimateHOldMag((Ammunition)Index);
    }

    public void DoFireAnim()
    {
        _anim.SetTrigger("TrigFire");
    }
    public void DoOpenHandAnim()
    {
       // _anim.SetTrigger("TrigHoldAmmo");
        _anim.Play("aHand_Opened");
    }

    public void DoTrigHoldAmmo() { _anim.SetTrigger("TrigHoldAmmo"); }
   
}
