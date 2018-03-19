// @Author Nabil Lamriben ©2017
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class FocussedBoxReceiver : MonoBehaviour, IFocusable, IInputClickHandler
{
    public void OnFocusEnter()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void OnFocusExit()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

  

    public void OnInputClicked(InputClickedEventData eventData)
    {
        SendMessageUpwards("CallScreen", this.gameObject.name);
    }

    public void TakeHit(Bullet bullet)
    {
        Debug.Log("hit level0");
        SendMessageUpwards("CallScreen", this.gameObject.name);
    }
}
