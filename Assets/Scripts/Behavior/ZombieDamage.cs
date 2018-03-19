// @Author Nabil Lamriben ©2018

using UnityEngine;

public class ZombieDamage : MonoBehaviour {

	#region PublicVars
	public int HitPointOverride = 20;
	#endregion

	#region PrivateVars
	int _hp;
    int _headShot_Lethal = 100;
    int _headShot_NonLethal = 80;
    int _bodyShot_Lethal = 60;
    int _bodyShot_NonLethal = 10;
    int _limbShot_Lethal = 50;
    int _limbShot_NonLethal = 5;
    #endregion

    #region Dependencies
    ZombieAnimState _zStateAnim_needed_forTrigHEadsot;
    ZombieEffects _zEffects;

	ZombieBehavior _ZBEH;
	#endregion

	#region INIT
	void Awake()
    {
        _ZBEH = GetComponent<ZombieBehavior>();

    }

    private void Start()
    {
       // ZombieInfo = GetComponentInChildren<TextMesh>();
        _zStateAnim_needed_forTrigHEadsot = GetComponent<ZombieAnimState>();
        _zEffects = GetComponent<ZombieEffects>();
        _hp = HitPointOverride;
    }
	#endregion

	#region PublicMethods
	public void SetHP(int value)
    {
        if (GameSettings.Instance != null)
        {
            if (GameSettings.Instance.IsStaticHitPointsON) { _hp = HitPointOverride; }
            else
            { _hp = HitPointOverride = value; }
        }
        else
        {
            Debug.Log("no gamesettings, zombie hp stays as is");
            _hp = HitPointOverride;
        }

    }

    public void TakeHit(Bullet bullet)
    {
        if (_ZBEH.CurZombieState == ZombieState.DEAD)
            return;

        string tag = bullet.hitInfo.collider.gameObject.tag;

        switch (tag)
        {
            case "ZombieHead":
                Rgister_HeadShot(bullet);
                break;
            case "ZombieTorso":
                UpdateBloodandScore_TorsoShot(bullet);
                break;
            case "ZombieLimb":
                UpdateBloodandScore_LimbShot(bullet);
                break;
            default:
                UpdateBloodandScore_LimbShot(bullet);
                break;
        }
    }

	public void Kill()
	{

		if (_ZBEH.CurZombieState == ZombieState.DEAD) return;

		_ZBEH.CurZombieState = ZombieState.DEAD;
		// stop attacking
		_zStateAnim_needed_forTrigHEadsot.StopAttackTimer();
		this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		Update_GameManager_Shouldb_onDeathEvent();
	}
	#endregion

	#region PrivateMethods
	void TakeHeadDamage(Bullet bullet)
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isHeadShotKill)
                _hp = -1;
            else
                _hp -= ((bullet.damage * 2) + 5);
        }
        else
        {
            Debug.Log("no gamemanager so no 1 headshotkill");
            _hp -= ((bullet.damage * 2) + 5);
        }
    }

    void Rgister_HeadShot(Bullet bullet)
    {
        if (bullet == null) return;
        _zEffects.Boold_On_Head(bullet);
        playsplat.Instance.PlaySplatSound();
        TakeHeadDamage(bullet);

        _zStateAnim_needed_forTrigHEadsot.Trigger_HeadShotAnim();


        if (_hp <= 0)
        {
            AddSCorePoints(_headShot_Lethal);
            Kill();
        }
        else
        {
           // if (GameSettings.Instance.IsTestModeON) ZombieInfo.text = "head hit NON lethal, hp= " + hp;

            AddSCorePoints(_headShot_NonLethal);

        }

    }

    void UpdateBloodandScore_TorsoShot(Bullet bullet)
    {
        if (bullet == null) return;

        // instantiate blood effect
        _zEffects.Boold_On_Torso(bullet);

        // take torso damage
        _hp -= bullet.damage;

        if (_hp <= 0)
        {        
            Kill();
            AddSCorePoints(_bodyShot_Lethal);
        }
        else
        {          
            AddSCorePoints(_bodyShot_NonLethal);
        }

    }

    void UpdateBloodandScore_LimbShot(Bullet bullet)
    {
        if (bullet == null) return;

        // instantiate blood effect
        _zEffects.Boold_On_Limb(bullet);

        // take limb damage
        _hp -= Mathf.RoundToInt(bullet.damage * 0.5f);

        if (_hp <= 0)
        {
            AddSCorePoints(_limbShot_Lethal);
            Kill();
        }
        else
        {
            AddSCorePoints(_limbShot_NonLethal);
        }
    }

    void AddSCorePoints(int argZpoints)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GetScoreMAnager().Update_Add_PointsTotal(argZpoints);
            GameManager.Instance.GetScoreMAnager().Update_Add_PointsCurWave(argZpoints);
        }
        else { Debug.Log("no Gm"); }
    }

    void Update_GameManager_Shouldb_onDeathEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GetScoreMAnager().Increment_ZombiesKilledCNT();
            GameManager.Instance.KillEnemyZombie(gameObject);
        }
        else
        {
            Debug.Log("zombie died , but no gamamanager ");
            return;
        }

    }
    #endregion

}
