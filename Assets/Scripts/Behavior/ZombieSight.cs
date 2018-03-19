// @Author Nabil Lamriben ©2018
using UnityEngine;

public class ZombieSight : MonoBehaviour
{
	#region PrivateVars
	int _layer_mask;
	int _mylayer = (1 << 31) | (1 << 11);// ( ((4<<1)|(2<<1)|(0<<1)));	
	Transform myTarget;
    Vector3 _EyesLocation;
    #endregion

    #region dependencies
	ZombieBehavior _ZBEH;
	#endregion

	#region INIT
	void Awake()
    {
        _ZBEH = GetComponent<ZombieBehavior>();
    } 

    void Start()
    {
		myTarget = Camera.main.transform;
        _layer_mask = (1 << 31) | (1 << 11); //looking at spatialMapping and Player layers only , not zombies 
        _EyesLocation = this.transform.position  + (Vector3.up * 1.5f);
    }
	#endregion

	#region UPDATE
	void Update()
    {
        if (_ZBEH.CurZombieState == ZombieState.DEAD) return;

        _EyesLocation = this.transform.position  + (Vector3.up * 1.2f);

        Debug.DrawRay(_EyesLocation, myTarget.transform.position - _EyesLocation, Color.green);

        RaycastHit hitInfo;

        if (Physics.Raycast(_EyesLocation, myTarget.transform.position - _EyesLocation,  out hitInfo ,20,  _mylayer))
        {
            if (hitInfo.collider.gameObject.tag == "SpatialMesh")
            {
                _ZBEH.HasLineOfSight(false);
            }
            else
            if (hitInfo.collider.gameObject.tag == "MainCamera")
            {
                _ZBEH.HasLineOfSight(true);
            }
        }
    }
	#endregion
}
