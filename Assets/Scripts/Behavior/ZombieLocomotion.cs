// @Author Nabil Lamriben ©2018
using System.Collections.Generic;
using UnityEngine;

public class ZombieLocomotion : MonoBehaviour {

    #region PrivateVars
    int _layerPlayer = 11;
    int _layerBarrier = 12;
    bool _stopPathfinding = false;
    float _reactionTimeInSeconds = 2.0f;
    float _walkSpeed = 0.5f;
    float _runSpeed = 0.65f;
    bool _useRootMotion = false;
    Quaternion _toRotation;          // destination rotation
    Stack<PathNode> _pathNodes;           // the path that the agent is following
    GameObject _currentPoint;        // current GridPoint on GridMap
    GameObject _targetPoint;         // the GridPoint that the agent is heading toward currently
    GameObject _hotspotPoint;
    Vector3 _targetPosition;         // the position that the agent is heading for
    bool _hasFoundHotspot;
    float _reactionTime;             // private version of reactionTimeInSeconds
    float _moveSpeed;                // movement speed
    bool _isMelting;                   // flag that triggers game object destruction
    Vector3 meltStartPosition;      // position of transform when melting begins          

    #endregion

    #region dependencies
    ZombieCollisionCTRL _zCollisonCtrl;
    PathFinder _pathFinder;

    ZombieBehavior _ZBEH;
	#endregion

	#region INITandListeners
	private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += UpdateLocomotion;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= UpdateLocomotion;
    }
   

    private void Awake()
    {
        _ZBEH = GetComponent<ZombieBehavior>();
        if (GameSettings.Instance != null)
        {
            _useRootMotion = GameSettings.Instance.IsZombieRootMotionOn;
        }
        else
        {
            Debug.Log("no GameSettings , so defaolt is USE ROOTMOTION");
            _useRootMotion = true;
        }
    }

    // Use this for initialization
    void Start() {
        _pathFinder = GetComponent<PathFinder>();
        _zCollisonCtrl = GetComponent<ZombieCollisionCTRL>();
       // _muchNeededzStateAnim = GetComponent<ZombieAnimState>();

        _toRotation = transform.rotation;

        _isMelting = false;
        _hasFoundHotspot = false;

        _pathNodes = new Stack<PathNode>();
        _targetPoint = null;

        _reactionTime = _reactionTimeInSeconds;
        _moveSpeed = _walkSpeed;

        transform.Rotate(Vector3.up, 180.0f);
        if (GameManager.Instance != null) { SetHotspotLocation(); } else { Debug.Log("no gamemanager"); }
    }
	#endregion

	#region UPDATE
	void Update()
    {
        if (GameManager.Instance == null) return;

        if (!_stopPathfinding)
            CalculateMovement();
    }
	#endregion

	#region PublicMethods
	public void MeltAndDestroy()
    {
        float dis = Vector3.Distance(transform.position, meltStartPosition);
        if (dis > 2.5f)
            Destroy(gameObject);
    }

    public void HasLineOfSight(bool argCanSeeyou)
    {
        if (argCanSeeyou)
        {
            _stopPathfinding = true;
            _currentPoint = null;
            Quaternion oldRotation = transform.rotation;
            //  transform.LookAt(new Vector3(targetPoint.transform.position.x, transform.position.y, targetPoint.transform.position.z));
            Vector3 camposNoY = new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
            transform.LookAt(camposNoY);
            _toRotation = transform.rotation;
            transform.rotation = oldRotation;

            // rotate towards targetPoint
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _toRotation, 500.0f * Time.deltaTime);
        }
        else
        {
            _stopPathfinding = false;
        }
    }
	#endregion

	#region PrivateMethods
	void UpdateLocomotion(ZombieState argNewZombieState)
    {
        if (argNewZombieState==ZombieState.MELTING)
        {
             
            meltStartPosition = transform.position;           
            _isMelting = true;
        }
    }

    void Melt()
    {
        _isMelting = true;
    }

    void CalculateMovement()
    {

        // removed return on state == ZombieState.IDLE 
        if (_ZBEH.CurZombieState == ZombieState.PAUSED || _ZBEH.CurZombieState == ZombieState.REACHING)
            return;

        if (_isMelting)
        {
            _zCollisonCtrl.MoveRigidBodyDown();
             MeltAndDestroy();
        }


        if (_ZBEH.CurZombieState == ZombieState.DEAD){
            _pathFinder.StopAllCoroutines(); 
            return;
        }

        if (_ZBEH.CurZombieState == ZombieState.ATTACKING)
            return;



        if (_currentPoint == null)
        {
            if (GameManager.Instance != null)
            {
                _currentPoint = GameManager.Instance.GetGridMap().GetClosestPoint(gameObject);
            }
            else { Debug.Log("no gm"); }

        }



        //check reaction time
        _reactionTime -= Time.deltaTime;
        if (_reactionTime <= 0.0f)
        {
            _reactionTime = _reactionTimeInSeconds;
            _targetPoint = null;
            if(_ZBEH.CurZombieState!= ZombieState.MELTING && _ZBEH.CurZombieState != ZombieState.DEAD)
            _ZBEH.CurZombieState = ZombieState.CHASING;
            _moveSpeed = _runSpeed;//walkSpeed;
                                   // multiplier_GETME = _zStateAnim.Get_RealMultiplier();
        }


        // if not following and there is no target point, check path
        if (_targetPoint == null)
        {
            if (_pathNodes == null)
            {
                _ZBEH.CurZombieState = ZombieState.IDLE;
                _pathNodes = new Stack<PathNode>();
            }

            if (_pathNodes.Count > 0)
            {
                _targetPoint = _pathNodes.Pop().gridPoint;
                _targetPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.player.gridPosition == null)
                        GameManager.Instance.player.SetGridPosition();
                }
                else { Debug.Log("no gm"); }




                if (!_pathFinder.isFinding)
                {
                    if (_hasFoundHotspot || _hotspotPoint == null)
                    {
                        if (GameManager.Instance != null)
                        {
                            _pathFinder.FindPath(_currentPoint, GameManager.Instance.player.gridPosition);
                        }
                        else { Debug.Log("no gm"); }


                    }
                    else
                    {
                        _pathFinder.FindPath(_currentPoint, _hotspotPoint);
                        _hasFoundHotspot = true;
                    }
                }
            }
        }

        else
        {

            // get rotation towards targetPoint
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(new Vector3(_targetPoint.transform.position.x, transform.position.y, _targetPoint.transform.position.z));
            _toRotation = transform.rotation;
            transform.rotation = oldRotation;

            // rotate towards targetPoint
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _toRotation, 500.0f * Time.deltaTime);

        }

        // move forward only if we do not use root motion and we are done rotating
        if (!_useRootMotion && _ZBEH.CurZombieState != ZombieState.IDLE && Quaternion.Angle(transform.rotation, _toRotation) <= 1.0f)
            _zCollisonCtrl.MoveRigidBodyForward(_moveSpeed);
    }
   
    void SetPath()
    {
        _pathNodes = _pathFinder.finalPath;
    }

    void SetHotspotLocation()
    {
        if (GameManager.Instance != null)
        {
            List<GameObject> hotspots = GameManager.Instance.GetHotspots();
            int randIndex = Random.Range(0, hotspots.Count);
            _hotspotPoint = GameManager.Instance.GetGridMap().GetClosestPoint(hotspots[randIndex]);
        }
        else { Debug.Log("no gm"); }


    }
	#endregion

	#region ontriRegion
	void OnTriggerEnter(Collider other)
    {
        if (_ZBEH.CurZombieState == ZombieState.DEAD || _ZBEH.CurZombieState == ZombieState.PAUSED)
            return;

        if (other.CompareTag("GridPoint"))
        {
            _currentPoint = other.gameObject;
            _currentPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.blue;

            if (_targetPoint != null)
            {
                if (_currentPoint == _targetPoint)
                {
                    _currentPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.cyan;

                    _targetPoint = null;
                }

            }
        }
        else if (other.gameObject.layer == _layerPlayer)
        {
            _ZBEH.CurZombieState = ZombieState.ATTACKING;
           // _muchNeededzStateAnim.Attack();
        }
        else if (other.gameObject.layer == _layerBarrier)
        {
            // if zombie has collided with barrier then switch to reach state
            // _muchNeededzStateAnim.Reach();
            _ZBEH.CurZombieState = ZombieState.REACHING;
        }
    }
	#endregion
}
