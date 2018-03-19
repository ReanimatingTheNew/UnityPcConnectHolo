using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMag  {

      bool TryDecrementBulletCount();
    GameObject GetBulletFromMag();
    int GetBulletsCount_inMag();
    void Refill();
    void InitCanPlayCollisionSound();
}
