using System;
using UnityEngine;
 

public class Data_Builder : MonoBehaviour {


    Data_PlayerInfo aliceinfo;
    Data_PlayerInfo bobinfo;
    Data_PlayerInfo cindyinfo;

    Data_PlayerPoints alicePoints;
    Data_PlayerPoints bobPoints;
    Data_PlayerPoints cinsyPoints;

    Data_PlayerSession alicesession;
    Data_PlayerSession bobsession;
    Data_PlayerSession cindysession;

 

    public Data_PlayerSession GEtBobSession() { return bobsession; }
    public Data_PlayerSession GEtAlicecession() { return alicesession; }
    SessionDataManager _sessmngr;
    void Start () {
        _sessmngr = GetComponent<SessionDataManager>();
        aliceinfo = new Data_PlayerInfo("alice", "alexander","aa", "aa@gmail.com");
        bobinfo = new Data_PlayerInfo("bob", "bristol","bb", "bBob@yahoo.com");
        cindyinfo = new Data_PlayerInfo("cindy", "lopper","cc", "cindylll@gmail.com");
        alicePoints = new Data_PlayerPoints(111, 142, 1143, 111,187171, 67, 8,2,3,45,3);
        bobPoints = new Data_PlayerPoints(22652, 2223, 22332, 2, 234,2367, 22, 2, 3, 45, 3);
        cinsyPoints = new Data_PlayerPoints(3563, 38, 3, 309, 36773, 333, 3, 2, 3, 45, 3);
        alicesession = new Data_PlayerSession(DateTime.Now.AddMinutes(1), aliceinfo, alicePoints);
        bobsession = new Data_PlayerSession(DateTime.Now.AddMinutes(20), bobinfo, bobPoints);
        cindysession = new Data_PlayerSession(DateTime.Now.AddMinutes(120), cindyinfo, cinsyPoints);

    }

    void Test_save1() {
        _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(alicesession);
    }

    void Test_save2()
    {
        _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(bobsession);
    }

  void Test_Save3() {
        _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(cindysession);
    }



    // Update is called once per frame


    void DoStuff() {
       _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(bobsession);
    }
    void Update () {
		
	}
}
