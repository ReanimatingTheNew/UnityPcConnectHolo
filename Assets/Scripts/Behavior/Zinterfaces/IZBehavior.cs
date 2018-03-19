// @Author Nabil Lamriben ©2018
using UnityEngine;

public interface IZBehavior  {

    void SetID(int argId);

    int GetID();

    void HasLineOfSight(bool argCanSeeyou);

    void SetHP(int value);

    void Zbeh_PauseZombieAnimation();

    void Melt();
}
