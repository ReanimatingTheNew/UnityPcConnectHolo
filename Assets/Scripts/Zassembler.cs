// @Author Nabil Lamriben ©2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zassembler : MonoBehaviour {
    //ArmL<->Shoulder_L     ArmR<->Shoulder_R
    public GameObject part_LeftArm;
    public GameObject part_RightArm;

    //  Body  Spine or chest WTFdudes will i have to breakit up in chest and hips as welll vuz there aint none


    // ForeArmL<->Elbow_L   ForeArmR<->Elbow_R
    public GameObject part_LeftForeArm;
    public GameObject part_RightForeArm;

    // HandL<->Wrist_L       HandR<->Wrist_R
    public GameObject part_LeftHand;
    public GameObject part_RightHand;

    //          Head<->Head_M
    public GameObject part_Head;

    //Knee_L<->Knee_L       Knee_R<->Knee_R
    public GameObject part_LeftShin;
    public GameObject part_RightShin;

    //  LegL<->Hip_L          LegR<->Hip_R  
    public GameObject part_LeftLeg;
    public GameObject part_RightLeg;

    // Neck<->Neck_M
    public GameObject part_Neck;


    public GameObject part_Bod;
    public GameObject part_Spine;
    public GameObject part_Chest;
    public GameObject part_Hips;





    public Transform Deform_Head;
    public Transform Deform_Neck;
    public Transform Deform_LeftArm;
    public Transform Deform_RightArm;
    public Transform Deform_LeftForeArm;
    public Transform Deform_RightForeArm;
    public Transform Deform_LeftHand;
    public Transform Deform_RightHand;

    void AttachHead()
    {
        GameObject headgo = Instantiate(part_Head, Deform_Head.position, Quaternion.identity) as GameObject;
        headgo.GetComponent<SkinnedMeshRenderer>().rootBone = Deform_Head;
       // headgo.transform.parent = Deform_Head;
    }

    void Start () {
        AttachHead();



    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
