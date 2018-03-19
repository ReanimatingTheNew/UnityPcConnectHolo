// @Author Nabil Lamriben ©2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum ARZState  {
    Pregame,
    WaveBuffer,
    WavePlay,
    WaveEnded,
    WaveOverTime,
    EndGame
}

public enum ARZGameModes {
    GameNoStem,
    GameLeft,
    GameRight,
    GameTest
}

public enum ZombieState
{
    IDLE,
    WALKING,
    CHASING,
    ATTACKING,
    DEAD,
    PAUSED,
    REACHING,
    CRAWL,
    STUNNED,
    MELTING
};

public enum GunType
{
    NONE,
    MAGNUM,
    PISTOL,
    UZI,
    SHOTGUN,
}

public enum PowerUpType
{
    DoublePoints,
    SlowMo,
    InVinsible,

}

public enum Ammunition
{
    NONE,
    MAGNUM,
    PISTOL, 
    UZI,
    SHOTGUN
};


public enum GunState
{
    // CLIP_IN, no need, we have gun.iscliploaded
    // CLIP_OUT, no need, we have gun.iscliploaded
    MAGizIN,
    CLIP_FULL,
    CANSHOOT,

    CLIP_EMPTY,

    MAGizOUT, 
    RELOADING,
    JAMMED
};


public enum WeaponContext {

    Colt_GUN_MainHand,
    M1911_GUN_MainHand,
    Mac11_GUN_MainHand,
    SOShotgun_GUN_MainHand,

    Colt_MAG_OffHand,
    M1911_MAG_OffHand,
    Mac11_MAG_OffHand,
    SOShotgun_MAG_OffHand,

    Colt_MAG_inGun,
    M1911_MAG_inGun,
    Mac11_MAG_inGun,
    SOShotgun_MAG_inGun,

    Colt_GUN_onRack,
    M1911_GUN_onRack,
    Mac11_GUN_onRack,
    SOShotgun_GUN_onRack,

    Colt_MAG_onRack,
    M1911_MAG_onRack,
    Mac11_MAG_onRack,
    SOShotgun_MAG_onRack
}


public enum ARZHandType
{
    HandGun, HandMag
}

public enum CellState {
Empty, 
Started,
Running,
Ended

}

//keep in this order cuz i use them as indecies in reload meter image cell array 
public enum ReloadMeterState {
    SEGMENT_0,
    SEGMENT_1,
    SEGMENT_2,
    Anim0,
    Anim1,
    Anim2,
    Sleeping
}


//esp 12 e
/// points = 0;
/// wavePoints = 0;
/// shotCount = 0;
/// headShotCount = 0;
/// torsoShotCount = 0;
/// limbShotCount = 0;
/// targetHitCount = 0;
/// killCount = 0;
/// enemiesCreatedCount = 0;
/// 


public enum ScoreType
{
    points_0 ,
    wavePoints_1,
    shotCount_2,
    headShotCount_3,
    torsoShotCount_4,
    limbShotCount_5,
    targetHitCount_6,
    killCount_7,
    enemiesCreatedCount_8,
    timesReloaded_9,
    timesDied_10
}

public enum ARZReloadLevel
{
    EASY,
    MEDIUM,
    HARD
}
public enum UDPmachine
{
    MSI_2,
    Jalt_3,
    Holo_01,
    Holo_02,

}