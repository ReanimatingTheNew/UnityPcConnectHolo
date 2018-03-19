using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventTesterListener : MonoBehaviour
{
    public EventSingleInt m_MyEvent;

    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new EventSingleInt();

        m_MyEvent.AddListener(Ping);
    }

    void Update()
    {

       if (Input.GetKeyDown(KeyCode.Space)  && m_MyEvent != null)
        {
            m_MyEvent.Invoke(5);
        }
    }

    void Ping(int i)
    {
        Debug.Log("Ping" + i);
    }
}