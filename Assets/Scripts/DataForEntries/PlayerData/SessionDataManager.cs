using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SysDiag = System.Diagnostics;
using System.Linq;


#if !UNITY_EDITOR && UNITY_METRO
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class SessionDataManager : MonoBehaviour {
    //C:\Users\nabil\AppData\LocalLow\Total Respawn\ARZHaloloween
    // C:/Users/juliePC/AppData/LocalLow/Total Respawn/ARZHaloloween
    string AllSessionsFileName="ARZAllSessions";

    string ArzDirPath
    {
        get
        {
#if !UNITY_EDITOR && UNITY_METRO
                    return ApplicationData.Current.RoamingFolder.Path;
#else
            return Application.persistentDataPath;
#endif
        }
    }

    DirectoryInfo ArzDir;

    bool AllSessionsFileExists;
    int fileCount = 0;
    private void Start()
    {

        ArzDir = new DirectoryInfo(ArzDirPath);
        FileInfo[] info = ArzDir.GetFiles("*.*");
        Debug.Log("there are " + info.Length + " files in here");
        AllSessionsFileExists = false;
        foreach (FileInfo f in info)
        {
            if (f.Name.Contains(AllSessionsFileName)) {
                AllSessionsFileExists = true;
                fileCount++;
            }
        }

        Debug.Log(AllSessionsFileName+ " count= " + fileCount + "should only be 1 ");
    }

    Data_PlayerAllSessions _AllSessionsObject;


 

 

    string BuildAllSessionsNameWithExtenssion()
    {
        return "/"  +AllSessionsFileName +".txt";
    }



    public void SaveSession_to_ALLSessions_AndSaveTOFile(Data_PlayerSession argDataPlayerSession)
    {
        string FullFilePath = ArzDirPath + BuildAllSessionsNameWithExtenssion();

        if (File.Exists(FullFilePath))
        {
            string dataAsJson = File.ReadAllText(FullFilePath);
            _AllSessionsObject = CreateALLSessionObjectFromJsonString(dataAsJson);
        }
        else
        {
            _AllSessionsObject = new Data_PlayerAllSessions();
        }

       
        _AllSessionsObject.AddSession(argDataPlayerSession);
        //PRINTFirstNamesUSERNAME();

        // save the Data_PlayerAllSessions
        string backtoJson = JsonUtility.ToJson(_AllSessionsObject);
        Write_AllSessions_String_toFile(backtoJson);
    }

    Data_PlayerAllSessions CreateALLSessionObjectFromJsonString(string argAllSess) {
        return JsonUtility.FromJson<Data_PlayerAllSessions>(argAllSess);
    }
    
    void Write_AllSessions_String_toFile(string argSesionJsonString)
    {
        string fullpath = ArzDirPath + BuildAllSessionsNameWithExtenssion();
        File.WriteAllText(fullpath, argSesionJsonString);
    }



    void PRINTFirstNamesUSERNAME() {
        Debug.Log("SAVED********");
        foreach (Data_PlayerSession psess in _AllSessionsObject.ListAllSessions) {
            Debug.Log(" saved user" + psess.PInfo.PlayerFirstName + "" + psess.PInfo.PlayerUserName);

        }
        Debug.Log("************");

    }
}

