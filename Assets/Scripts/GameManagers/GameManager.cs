// @Author Jeffrey M. Paquette ©2016
// @Author Nabil Lamriben ©2017

//******************************
// This needs a major refactor
// needs an Objects Manager 
// needs objectplacer 
// needs a timing manager as an event base system 
// needs shotsmanager
// needs game state manager with listenners to the timing system for starting game, wave start , wave buffer, endgame
// needs player ui canvas manager 
// needs zombie manager 
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;


    public delegate void TriggerHandler(int argNum);
    public static TriggerHandler CountDownHandler;
    public void call_CountDownAudioVideo(int argNum) { if (NewMethod()) { CountDownHandler(argNum); } }
    private bool NewMethod()
    {
        return CountDownHandler != null;
    }


    public delegate void GgamePausedEvent(bool argonoff);
    public static GgamePausedEvent OnGamePaused;
    public void SignalGamePause(bool argonoff)
    {
        if (OnGamePaused != null) OnGamePaused(argonoff);
    }

    public delegate void GgameContinuedEvent(bool argonoff);
    public static  GgameContinuedEvent OnGameContinue;
    public  void SignalGameContinue(bool argonoff)
    {
        if (OnGameContinue != null) OnGameContinue(argonoff);
    }


    #region gameobjectstobeplaced

    public GameObject spawnPoint;
    string AnchorName_SpawnPoint;

    public GameObject gridMap;
    string AnchorName_PathFinder;


    public GameObject hotspot;
    string AnchorName_HotSpot;

    public GameObject scoreboard;
    string AnchorName_ScoreBoard;

    public GameObject infiniteAmmoBox;
    string AnchorName_AmmoBoxInfinite;

    public GameObject stemSystemKit;
    string AnchorName_StemBase;

    public GameObject CamBarrell;

    public GameObject consoleObject;
    string AnchorName_ConsoleObject;

    public GameObject mist;
    string AnchorName_MistEmitter;
    string AnchorName_MistEnd;
    GameObject _placeHolderMistTarget;

    string AnchorName_Target;
    public GameObject target;

    string AnchorName_StartButton;
    public GameObject startButton;

    string AnchorName_metalBarrel;
    public GameObject metalBarrel;

    string AnchorName_roomModel;
    public GameObject roomModel;

    public GameObject placeholder;

    public ARZState curgamestate;
    public ARZGameModes curgamemode;

    //*********************************************************************************************************

    public GameObject reticle;
    public GameObject youDiedScreen;
    public GameObject gameOverScreen;
    public GameObject gameCanvasObject;
    public GameObject gameCountDownScreen;


    public GameObject magnumPickupPrefab;
    public GameObject uziPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject DoubleScorePowerUp;    

    #endregion


    #region props

    [Tooltip("Setting thos value will show gridpoints as visible cubes")]
    public bool isTestMode;

    [Tooltip("Setting this value will allow 1 headshot kill")]
    public bool isHeadShotKill;
 
    int enemyCorpseLimit = 3;

    [HideInInspector]
    public bool isDead { get; private set; }
    public PlayerBehavior player { get; private set; }
  

    WaveManager _waveManager;
    GameCanvas _gameCanvas;
    StemKitMNGR _stmKitMngr;
    ScoreManager _scoreManager;
    StreaksManager _streaksManager;
    public GridMap map;

    bool isLevelLoaded = false;
    bool isRoomLoaded = false;
    bool isGridBuilt = false;
    bool isGameStarted = false;
    bool gameTimeIsUp = false;

    public List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> dummySpawnPoints = new List<GameObject>();
    List<GameObject> barriers = new List<GameObject>();
    List<GameObject> mags = new List<GameObject>();
    List<GameObject> ammoBoxes = new List<GameObject>();
    List<GameObject> walkieTalkies = new List<GameObject>();
    List<GameObject> mists = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> hotspots = new List<GameObject>();
    Queue<GameObject> deadEnemies = new Queue<GameObject>();
   // WeaponRack rack;
    TargetControle TargCTRL;
    StartButtonControle StartCTRL;
    WorldAnchorStore anchorStore;

    // test room stuff 
    public GameObject devRoomPrefab;
    public GameObject devRoomPosition;
    public bool usedevRoom;
    public bool useOneSpawn;
    GameObject devRoomObj;
    DevRoomManager devRoomManager; 

    void InitAnchorNameVariables()
    {
         AnchorName_ConsoleObject = GameSettings.Instance.GetAnchorName_ConsoleObject();

        AnchorName_StemBase = GameSettings.Instance.GetAnchorName_StemBase();

        AnchorName_SpawnPoint = GameSettings.Instance.GetAnchorName_SpawnPoint();


        AnchorName_ScoreBoard = GameSettings.Instance.GetAnchorName_ScoreBoard();

        AnchorName_Target = GameSettings.Instance.GetAnchorName_Target();

        AnchorName_StartButton = GameSettings.Instance.GetAnchorName_StartButton();

        AnchorName_metalBarrel = GameSettings.Instance.GetAnchorName_MetalBarrel();

        AnchorName_roomModel = GameSettings.Instance.GetAnchorName_RoomModel();
 
        AnchorName_AmmoBoxInfinite = GameSettings.Instance.GetAnchorName_AmmoBoxInfinite();

        AnchorName_PathFinder = GameSettings.Instance.GetAnchorName_PathFinder();

        AnchorName_MistEmitter = GameSettings.Instance.GetAnchorName_MistEmitter();

        AnchorName_MistEnd = GameSettings.Instance.GetAnchorName_MistEnd();

        AnchorName_HotSpot = GameSettings.Instance.GetAnchorName_HotSpot();
    }

#endregion

    private void Awake()
    {         
        curgamemode = GameSettings.Instance.GameMode; 
        InitAnchorNameVariables();

        if (Instance == null)
        {
            curgamestate = ARZState.Pregame;
     
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    #region GunLines
    // Use this for initialization
    List<GameObject> LineObjects;
    public void ADDLine(GameObject ArgLine) {
        LineObjects.Add(ArgLine);
    }
    public List<GameObject> GEtLines() { return LineObjects; }
    public List<DoubleVector> ZombieHitLines;
    public List<DoubleVector> ZombieMissLines;
    Transform PATHFINDER_ANC;
    private void Locate_Lines_ANC_Pivot(GameObject go)
    {
        PATHFINDER_ANC= go.transform;
    }
    public void AddZombieHitRelativeToPAthfinder(Vector3 start, Vector3 end) {
        DoubleVector d = new DoubleVector(PATHFINDER_ANC.InverseTransformPoint(start), PATHFINDER_ANC.InverseTransformPoint(end));
        ZombieHitLines.Add(d);
    }
    public void Add_FailedShots_RelativeToPAthfinder(Vector3 start, Vector3 end) {
        DoubleVector d = new DoubleVector(PATHFINDER_ANC.InverseTransformPoint(start), PATHFINDER_ANC.InverseTransformPoint(end));
        ZombieMissLines.Add(d);
    }
    #endregion


    #region START
    void Start()
    {

        //devRoomPrefab.SetActive(false);
        _gameCanvas = gameCanvasObject.GetComponent<GameCanvas>();
        player = Camera.main.GetComponent<PlayerBehavior>();
        _waveManager = FindObjectOfType<WaveManager>();
        _scoreManager = GetComponent<ScoreManager>();
        _streaksManager = GetComponent<StreaksManager>();
        LineObjects = new List<GameObject>();
        ZombieHitLines = new List<DoubleVector>();
        ZombieMissLines = new List<DoubleVector>();
        _scoreManager.ResetScore();
        if (usedevRoom && (devRoomPrefab != null))
        {
            // devRoomPrefab.SetActive(true);
            devRoomObj = Instantiate(devRoomPrefab, devRoomPosition.transform);
            devRoomManager = devRoomObj.GetComponent<DevRoomManager>();

            if (useOneSpawn) {
                spawnPoints.Add(devRoomManager.devSpawnPoint7);
            }
            else
            {
                spawnPoints.Add(devRoomManager.devSpawnPoint1);
                spawnPoints.Add(devRoomManager.devSpawnPoint2);
                spawnPoints.Add(devRoomManager.devSpawnPoint3);
                spawnPoints.Add(devRoomManager.devSpawnPoint4);
                spawnPoints.Add(devRoomManager.devSpawnPoint5);
                spawnPoints.Add(devRoomManager.devSpawnPoint6);
            }
  
            hotspots.Add(devRoomManager.devHotspot1);
            hotspots.Add(devRoomManager.devHotspot2);
            hotspots.Add(devRoomManager.devHotspot3);

            Locate_Lines_ANC_Pivot(devRoomManager.devPAthfinder);
            TargCTRL = devRoomManager.devtarget.GetComponent<TargetControle>();
            StartCTRL = devRoomManager.devButton.GetComponent<StartButtonControle>();

            LevelLoaded();
      
        }
    }
    //todo: find a better way to initialize the gun in had in any other way than a timer... bad programming
    void SetGun() {
        StemKitMNGR.Call_GunSetChangeTo(GunType.MAGNUM);
        StemKitMNGR.CALL_UpdateAvailableGUnIndex(3);    
        StemKitMNGR.CALL_ToggleAllowExtraButtons(true);
        StemKitMNGR.CALL_ToggleStemInput(true);
        
        if(_stmKitMngr==null)Debug.Log("we loaded the stemstation, but cannot get the stemkitMNGrScript");
    }

    public void CheckStartGame()
    {
        //Debug.Log("checkstart");
        if (!isGameStarted)
        {
            if (isRoomLoaded && isGridBuilt && isLevelLoaded)
            {

                //must turn off target and start button from here .. what a hack
                if(TargCTRL != null) TargCTRL.MakeInvisible();

                if (StartCTRL != null) StartCTRL.MakeInvisible();
                StartGame();
            }
        }
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    void StartGame()
    {

        StemKitMNGR.CALL_ToggleAllowExtraButtons(false);
        TargCTRL.MakeInvisible();
       // DebugConsole.print("1 Gamestarted");
        if (mists.Count > 0)
        {
            //DebugConsole.print("we got mists");
            MistMover mm = mists[0].gameObject.GetComponent<MistMover>();
            if (mm)
            {
                //  DebugConsole.print("we got mistmover");
                if (_placeHolderMistTarget != null)
                {
                    DebugConsole.print("mist can move to mist target");
                    mm.StartMistMove(_placeHolderMistTarget.transform.position);
                }
                else
                {
                    DebugConsole.print("mist is moving to player");
                    mm.StartMistMove(
                      new Vector3(
                          mists[0].gameObject.transform.position.x,
                          mists[0].gameObject.transform.position.y,
                          GameObject.FindObjectOfType<Camera>().transform.position.z));
                }
            }
        }

        isGameStarted = true;
        if (player.gridPosition == null) player.SetGridPosition();

        // begin first wave in 5 seconds
        _waveManager.BeginNextWave( GameSettings.Instance.FirstBuffer);
        WaveStartedGraphics();
    }
    #endregion


    #region gameLogicKillattackplayer
    private void OnDestroy()
    {
        // clear instance variable if gamemanager is destroyed
        if (Instance == this)
            Instance = null;
    }

    public void AttackPlayer()
    {
        _streaksManager.Set_StreakBreake();
        _gameCanvas.TakeHit();
    }

    public void KillePlayer()
    {
        _streaksManager.Set_StreakBreake();
        _gameCanvas.KillePlayer();
    }

    public void AwardWeapon(GunType gun)
    {
        Debug.Log("Fuck yo couch");
    }

    public List<GameObject> GetSpawnPoints()
    {
        return spawnPoints;
    }

    public List<GameObject> GetDummySpawnPoints()
    {
        return dummySpawnPoints;
    }

    public List<GameObject> GetBarriers()
    {
        return barriers;
    }

    public List<GameObject> GetHotspots()
    {
        return hotspots;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public bool IsLevelLoaded()
    {
        return isLevelLoaded;
    }

    public bool IsRoomLoaded()
    {
        return isRoomLoaded;
    }

    public bool IsGridBuilt()
    {
        return isGridBuilt;
    }

    public GridMap GetGridMap()
    {
        return map;
    }

    public void RoomLoaded()
    {
        isRoomLoaded = true;
        if (_waveManager == null)
            _waveManager = FindObjectOfType<WaveManager>();

        if (_waveManager.isWaveLoaded)
            WorldAnchorStore.GetAsync(AnchorStoreReady);
        else
        {
            // if wave is not loaded check back in a half second
            TimerBehavior t = new TimerBehavior();
            t.StartTimer(0.5f, RoomLoaded);
        }
  
    }

    public void ActivateDevRoom() {
        Debug.Log("Roomloader loaded devroom");
        isRoomLoaded = true;
        if (_waveManager == null)
            _waveManager = FindObjectOfType<WaveManager>();

        if (_waveManager.isWaveLoaded) LoadObjectsFromDevRoom();
        else
        {
            // if wave is not loaded check back in a half second
            TimerBehavior t = new TimerBehavior();
            t.StartTimer(0.5f, RoomLoaded);
        }
    }

    void LoadObjectsFromDevRoom() {

        //todo Load spawnpoint, grid shit , and place fps controller with a pewpew gun that works woth mouse aim and cick
    }

    public void GridBuilt(GridMap map)
    {
        Debug.Log("creating map");

        this.map = map;
        isGridBuilt = true;
    }

    public void LevelLoaded()
    {
        isLevelLoaded = true;

        // sort spawn points by distance
        SortSpawnPoints();

        if (!isGridBuilt)
        {

            Debug.Log("creating grid");
            CreateGrid();
        }

        Debug.Log("yo, set first person gun !!");
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(2.5f, SetGun);
    }

    private void SortSpawnPoints()
    {
        spawnPoints.Sort(RankSpawnPointsByDistance);
    }

    private void CreateGrid()
    {
        GameObject.FindObjectOfType<GridMap>().CreateGrid(!isTestMode);
    }
  
    public void ClearAllToWhite()
    {
        if (isTestMode) {
            foreach (GameObject p in map.GetGridMap())
             {
             p.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
    #endregion


    #region SKORES

    //for display
    public ScoreManager GetScoreMAnager() { return this._scoreManager; }


    public void KillEnemyZombie(GameObject argZombieTOkill) {
        enemies.Remove(argZombieTOkill);
        deadEnemies.Enqueue(argZombieTOkill);
        _waveManager.OnKill(argZombieTOkill);
        CheckCorpseCount();
    }


    #endregion

    #region STREAKS
    public StreaksManager GetStreakManager() { return _streaksManager; }

    #endregion

    public GameObject CreateEnemy(GameObject spawnPoint, GameObject enemy)
    {
        // Debug.Log("9 GAMEMANAGER INSTANTIATING");
        GameObject e = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity) as GameObject;
        enemies.Add(e);
        _scoreManager.Increment_ZombiesCreated();
        ClearAllToWhite();
        return e;
    }

    public void TimesUp()
    {
        curgamestate = ARZState.EndGame;
        gameTimeIsUp = true;
        StemKitMNGR.CALL_ToggleStemInput(false);
    }

    public void PlayerDied_GameManager()
    {
        isDead = true;
        StemKitMNGR.CALL_ToggleStemInput(false);
        _scoreManager.Increment_DeathsCNT();

        // turn off gun and reticle
        if (reticle != null)
            reticle.SetActive(false);       

        // pause all enemies
        foreach (GameObject g in enemies){
            g.GetComponent<ZombieBehavior>().Zbeh_PauseZombieAnimation();
        }

        if (gameTimeIsUp)
        {
            Debug.Log("gameover yo");
            // game over
            _gameCanvas.FinalScore(_scoreManager.Get_PointsTotal());
            gameOverScreen.SetActive(true);
            _waveManager.StopTheGame();
           
        }
        else
        {
            //*******************************************************************************************************************
            //THIS NEVER HAPPENED gametimeIsup is Never set to true bcause nothing ever calst TimeUp() , instead we use HArdStop
            //*******************************************************************************************************************
            int CurWavePoints = _scoreManager.Get_PointsCurWave();

            _gameCanvas.PointsLost(CurWavePoints);

            _scoreManager.Update_Add_PointsTotalLost(CurWavePoints);
            _scoreManager.Update_Remove_PointsTotal(CurWavePoints);
            _scoreManager.Reset_WavePoints();

            ScoreDebugCon.Instance.update_WAVEPoints(0);
            

            youDiedScreen.SetActive(true);

            // tell wave manager whether or not to reload wave via if time is up
            //OnGameOver_WaveManager(false);
            _waveManager.KeepPlaying();
          
        }

        _gameCanvas.PlayGameOverAudio();
    }

    //public PlayerInfoEntry pdi;

    public void HardStop()
    {

        // tell wave manager whether or not to reload wave via if time is up
        // waveManager.OnGameOver_WaveManager(true);
        _waveManager.StopTheGame();
        StemKitMNGR.CALL_ToggleStemInput(false);

        // pause all enemies
        foreach (GameObject g in enemies)
        {
            g.GetComponent<ZombieBehavior>().Zbeh_PauseZombieAnimation();
        }
      
       // game over
        _gameCanvas.FinalScore(_scoreManager.Get_PointsTotal());
        gameOverScreen.SetActive(true);
        _gameCanvas.PlayGameOverAudio();

        curgamestate = ARZState.EndGame;

        PersistantScoreGrabber.Instance.DoGrabScores();
        PersistantScoreGrabber.Instance.DoGrabLines();
        StartCoroutine(AUTOGOTO_DataEntry());

    }

    IEnumerator AUTOGOTO_DataEntry()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("DataEntry");
    }


    public void ResetWave()
    {
        // destroy all enemies
        foreach (GameObject g in enemies)
        {
            Destroy(g);
        }
        enemies = new List<GameObject>();

        foreach (ZombieBehavior z in GameObject.FindObjectsOfType<ZombieBehavior>())
        {
            Destroy(z.gameObject);
        }
        // get rid of blood splatters
        _gameCanvas.ResetDamage();

        // turn on gun and reticle
        if (reticle != null) {
            if(usedevRoom)
                reticle.SetActive(true);
        }
        //if no gun->reset gunfor wave
        isDead = false;

        //disable game over tag along screen
        youDiedScreen.SetActive(false);

        WaveStartedGraphics();
        StemKitMNGR.CALL_ToggleStemInput(true);
    }

    public void GM_Handle_WaveCompleteByPoppingNUMplusplus()
    {
        _scoreManager.Increment_WavesPlayedCNT();
        // reset wave points
        _scoreManager.Reset_WavePoints();

//could add accuracy bonus here for each wave, if we pas points to this method 

        // activate canvas spash
        _gameCanvas.WaveCompleteWeaponUpgare();

        // load next wave and launch in 10 seconds
        _waveManager.WaveCompleted_soPopANewOne();

        // activate next wave splash in 6 seconds
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(GameSettings.Instance.StartRomanIn, WaveStartedGraphics);     
    }

    public void WaveStartedGraphics()
    {
        _gameCanvas.WaveStarted(_waveManager.GetWaveRomanNumeral());
    }

    void CheckCorpseCount()
    {
        if (deadEnemies.Count > enemyCorpseLimit)
        {
            DestroyDeadEnemy();
        }
    }

    void RemoveTarget()
    {
        Destroy(target);
    }

    void DestroyDeadEnemy()
    {
        //DebugConsole.print("gm sending MELT");
        GameObject enemy = deadEnemies.Dequeue();
        if (enemy != null) {
            ZombieBehavior zb = enemy.GetComponent<ZombieBehavior>();
            if (zb != null) {
                zb.CurZombieState = ZombieState.MELTING;
            }
        }
        //enemy.SendMessage("Melt");
    }

    void LoadObjects()
    {
        if (_waveManager.GetWave() == null)
        {
            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(0.5f, LoadObjects);
            return;
        }

        if (!IsInDevRoom())
        {
            // gather all stored anchors
            string[] ids = anchorStore.GetAllIds();
            for (int index = 0; index < ids.Length; index++)
            {
                if (ids[index] == AnchorName_StemBase)
                {
                    if (curgamemode != ARZGameModes.GameNoStem) {

                        reticle.gameObject.SetActive(false);

                       

                            // if anchor is stem system
                            // instantiate stem system prefab it should have been set to left or right on start
                            GameObject obj = Instantiate(stemSystemKit) as GameObject;
                            anchorStore.Load(ids[index], obj);

                            // delete anchor component
                            WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                            if (attachedAnchor != null)
                                DestroyImmediate(attachedAnchor);

                        _stmKitMngr = obj.GetComponent<StemKitMNGR>();


                    }
                   

                }
                else if (ids[index] == AnchorName_ConsoleObject)
                {
                    // if anchor is console object
                    // instantiate console prefab
                    GameObject obj = Instantiate(consoleObject) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);
                }
                else if (ids[index] == AnchorName_ScoreBoard)
                {
                    // if anchor is scoreboard
                    // instantiate scoreboard from anchor data
                    GameObject obj = Instantiate(scoreboard) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                }
                //--------------------------------------------------------------------
                else if (ids[index] == AnchorName_Target)
                {
                    // if anchor is scoreboard
                    // instantiate scoreboard from anchor data
                    GameObject obj = Instantiate(target) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                    TargCTRL = obj.GetComponent<TargetControle>(); 
                }
                //-------------------------------------------------------------------- 
                else if (ids[index] == AnchorName_StartButton)
                {
                     
                    GameObject obj = Instantiate(startButton) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                    StartCTRL = obj.GetComponent<StartButtonControle>();
                }
                //-------------------------------------------------------------------- AnchorName_StartButton



                else if (ids[index] == AnchorName_metalBarrel)
                {
                    // if anchor is weapons rack
                    // instantiate weapons rack from anchor data
                    GameObject obj = Instantiate(metalBarrel) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                  //  obj.transform.Rotate(Vector3.up, 180.0f); or not
                   // rack = obj.GetComponent<WeaponRack>(); 
                }


                else if (ids[index] == AnchorName_roomModel)
                {
                    // if anchor is weapons rack
                    // instantiate weapons rack from anchor data
                    GameObject obj = Instantiate(roomModel) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                    //  obj.transform.Rotate(Vector3.up, 180.0f); or not
                    // rack = obj.GetComponent<WeaponRack>(); 
                }

                else if (ids[index].Contains(AnchorName_AmmoBoxInfinite))
                {
                    // if anchor is infinite ammo box
                    // instantiate infinite ammo box from anchor data
                    GameObject obj = Instantiate(infiniteAmmoBox) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);
                }
                else if (ids[index] == AnchorName_PathFinder)
                {
                    // if anchor is pathfinder
                    // instantiate pathfinder from anchor data
                    GameObject obj = Instantiate(gridMap) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);


                    Locate_Lines_ANC_Pivot(obj);
                }
            

           
                else if (ids[index].Contains(AnchorName_SpawnPoint))
                {
                    // if anchor is a spawn point
                    // instantiate spawn point from anchor data
                    GameObject obj = Instantiate(spawnPoint) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // add spawn point to collection
                    spawnPoints.Add(obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);
                }
                else if (ids[index].Contains(AnchorName_HotSpot))
                {
                    // if anchor is a hotspot
                    // instantiate hotspot from anchor data
                    GameObject obj = Instantiate(hotspot) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    // add spawn point to collection
                    hotspots.Add(obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);
                }
           
                else if (ids[index].Contains(AnchorName_MistEmitter))
                {
                    // if anchor is mist
                    // instantiate mist from anchor data
                    GameObject obj = Instantiate(mist) as GameObject;
                    anchorStore.Load(ids[index], obj);

                    mists.Add(obj);

                    // delete anchor component
                    WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);
                }
                else if (ids[index].Contains(AnchorName_MistEnd))
                {
                    //MistMover MistMoverScript = FindObjectOfType<MistMover>();

                    //// if none exist then instantiate one
                    //if (MistMoverScript == null)
                    //{
                    //    GameObject obj = Instantiate(mist) as GameObject;
                    //    MistMoverScript = obj.GetComponent<MistMover>();
                    //}

                    // instantiate placeholder at world anchor position
                    _placeHolderMistTarget = Instantiate(placeholder) as GameObject;
                    anchorStore.Load(ids[index], _placeHolderMistTarget);

                    // delete anchor component
                    WorldAnchor attachedAnchor = _placeHolderMistTarget.GetComponent<WorldAnchor>();
                    if (attachedAnchor != null)
                        DestroyImmediate(attachedAnchor);

                    // add placeholder to airstrike object as air strike end
                    //MistMoverScript.SetMistEnd(placeholderObject);
                }
            }
        }

        if (_placeHolderMistTarget == null)
        {
            DebugConsole.print("mist target is NULL!");
        }
 
        LevelLoaded();

    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        foreach (GameObject dead in deadEnemies)
        {
            Destroy(dead);
        }
        deadEnemies.Clear();
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
        LoadObjects();
    }

    private static int RankSpawnPointsByDistance(GameObject x, GameObject y)
    {
        float xDistance = GetDistanceToCamera(x);
        float yDistance = GetDistanceToCamera(y);

        if (xDistance < yDistance)
        {
            return 1;
        }
        else if (xDistance > yDistance)
        {
            return -1;
        }
        else return 0;
    }

    private static float GetDistanceToCamera(GameObject o)
    {
        // gets horizontal distance from o to Camera.main
        return Vector2.Distance(new Vector2(o.transform.position.x, o.transform.position.z),
            new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z));
    }
  
    public bool IsInDevRoom()
    {
        return (devRoomManager != null);
    }
}
