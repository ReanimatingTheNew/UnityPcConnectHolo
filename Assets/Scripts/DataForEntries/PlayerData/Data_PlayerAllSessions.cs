using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Data_PlayerAllSessions  {

    public List<Data_PlayerSession> ListAllSessions;

    public Data_PlayerAllSessions() {
        ListAllSessions = new List<Data_PlayerSession>();
    }

    public void AddSession(Data_PlayerSession argSession) {
        ListAllSessions.Add(argSession);
    }
}
