// @Author Nabil Lamriben ©2018
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollisionCTRL : MonoBehaviour {

	#region dependencies
	Rigidbody rb;
	CapsuleCollider cc;
    ZombieBehavior _ZBEH;
	#endregion

	#region INITandListeners
	private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += UpdateCollidersState;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= UpdateCollidersState;
    }

    private void Awake()
    {
        _ZBEH = GetComponent<ZombieBehavior>();
        rb = gameObject.GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

    }
	#endregion

	#region PublicMethods
	public void MoveRigidBodyForward(float movespeed)
	{
		rb.MovePosition(transform.position + (transform.forward * Time.deltaTime * movespeed));
	}
	public void MoveRigidBodyDown()
	{
		rb.MovePosition(transform.position + (Vector3.down * Time.deltaTime * 0.5f));
	}
	#endregion

	#region PrivateMethods
	void UpdateCollidersState(ZombieState argNewZombieState)
    {
         switch (argNewZombieState)
        {
            case ZombieState.IDLE:

            case ZombieState.WALKING:
                 break;
            case ZombieState.CHASING:
                 break;
            case ZombieState.REACHING:
                 break;
            case ZombieState.DEAD:
                DisableBoxColliders(this.transform);
                CapsuleColliderEnable(false);
                FreezAllRigidBodyConstraints();
                break;
            case ZombieState.PAUSED:
                    break;
            case ZombieState.MELTING:
				ClearRigidBodyConstraints();
				break;
 
        }

    }

    Transform DisableBoxColliders(Transform argTrans)
    {
        if (argTrans.gameObject.GetComponent<BoxCollider>())
            argTrans.gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform c in argTrans)
        {
            var result = DisableBoxColliders(c);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    void CapsuleColliderEnable(bool argOnOff)
    {
        cc.enabled = argOnOff;
    }

    void ClearRigidBodyConstraints() { rb.constraints = RigidbodyConstraints.None; }

    void FreezAllRigidBodyConstraints() { rb.constraints = RigidbodyConstraints.FreezeAll; }
	#endregion
}
