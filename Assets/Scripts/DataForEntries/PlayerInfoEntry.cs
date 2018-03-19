// @Author Nabil Lamriben ©2017
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInfoEntry : MonoBehaviour
{

     public InputField TextBoxField;

    Data_PlayerInfo cur_data_PlayerInfo;

    Data_PlayerPoints CurPlayerPoints;

    Data_PlayerSession CurrDataSession;

    SessionDataManager _sessmngr;

    SingleStoreObjectLoader SSOB;
    string StrEntered;

    bool hasbeenActivated;
     bool textwasentered;

    public Text txt;
    //this object will be created at the beggining of each Game session. It is not persistant;
    //Todo: persistant score grabber needs not be persistant at all. in fact i can just use it here and create a playerpoints object  in ValueChanged
    void Start()
    {
        SSOB = GetComponent<SingleStoreObjectLoader>();
        Init2();
        StartCoroutine(StartLinesIn2Seconds());
    }

     void LineFromAtoBRed(Vector3 A, Vector3 B)
    {
        GameObject lineObj = new GameObject("DragLine", typeof(LineRenderer));
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        line.SetWidth(0.04f, 0.02f);
        Material whiteDiffuseMat = new Material(Shader.Find("Mobile/Particles/Additive"));
        whiteDiffuseMat.color = Color.red;
        line.material = whiteDiffuseMat;
        //line.material.color = Color.red;
        line.SetVertexCount(2);
        line.SetPosition(0, A);
        line.SetPosition(1, B);
     //   line.SetColors(Color.red, Color.red);
      //  lineObj.AddComponent<KillTimer>().StartTimer(0.05f);

        
    }

     void LineFromAtoBWhite(Vector3 A, Vector3 B)
    {
        GameObject lineObj = new GameObject("DragLine", typeof(LineRenderer));
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        line.SetWidth(0.04f, 0.02f);
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        line.material = whiteDiffuseMat;
        line.SetVertexCount(2);
        line.SetPosition(0, A);
        line.SetPosition(1, B);
        line.SetColors(Color.white, Color.white);
       // lineObj.AddComponent<KillTimer>().StartTimer(0.05f);     
    }

    void DrawRedLines() {
        foreach (DoubleVector dv in PersistantScoreGrabber.Instance._ZombieHitLines) {
            // LineFromAtoBRed(dv.start, dv.end);
            LineFromAtoBRed(ANCPIVOT.TransformPoint(dv.start), ANCPIVOT.TransformPoint(dv.end));
        }
    }

    Transform ANCPIVOT;

    void DrawWhiteLines()
    {
        foreach (DoubleVector dv in PersistantScoreGrabber.Instance._ZombieMissLines)
        {
            LineFromAtoBWhite(ANCPIVOT.TransformPoint(dv.start), ANCPIVOT.TransformPoint(dv.end));

        }
    }

    IEnumerator StartLinesIn2Seconds()
    {
        Debug.Log("going to autogame in 5 sec");
        yield return new WaitForSeconds(5);
        ANCPIVOT = SSOB.getPathfinderWorldReff();
        InitLinesLocationsandDRAW();
    }
    void InitLinesLocationsandDRAW() {
        if (!GameSettings.Instance.IsHideBulletsPaths)
        {
            DrawWhiteLines();
            DrawRedLines();
        }
    }
    void Init2()
    {
       


        TextBoxField.ActivateInputField();
        TextBoxField.onValueChanged.AddListener(delegate { CatchSend(); });

        _sessmngr = GetComponent<SessionDataManager>();

        if (PersistantScoreGrabber.Instance == null) { CurPlayerPoints = new Data_PlayerPoints(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1); }
        else
            CurPlayerPoints = PersistantScoreGrabber.Instance.Get_Data_Player();


        txt.text = "";
        txt.text =  "Player Final Score     = " + CurPlayerPoints.score + "\n" + 
                    " Accuracy = "+CalcAccuracy(CurPlayerPoints.totalshots, CurPlayerPoints.miss) + "\n"+
                    "Player wavessurvived   = " + CurPlayerPoints.wavessurvived + "\n" +
                    "Player points Lost     = " + CurPlayerPoints.pointslost + "\n" +
                    "Player deaths          = " + CurPlayerPoints.deaths + "\n" + "\n" +

                    "Player headshots       = " + CurPlayerPoints.headshots + "\n" +
                    "Player streakcount     = " + CurPlayerPoints.streakcount + "\n" +
                    "Player maxstreak       = " + CurPlayerPoints.maxstreak + "\n" +
                    "Player zombies killed  = " + CurPlayerPoints.kills + "\n" +
                    "Player Shots missed    = " + CurPlayerPoints.miss + "\n" +
                    "Player totalshots      = " + CurPlayerPoints.totalshots + "\n" +
                    "Player numberofReloads = " + CurPlayerPoints.numberofReloads + "\n" +
                    "Player wavessurvived   = " + CurPlayerPoints.wavessurvived + "\n";
    }

    string CalcAccuracy(int allshots, int miss)
    {
        int landedcalced = allshots - miss;
    
            float fired = (float)allshots;
            float landed = (float)landedcalced;
            if (fired > 0)
            {
                float accuracy = (landed / fired) * 100;
                return accuracy.ToString("0.00") + " %";
            }
    


        return "0 %";


    }
    string charstr;
    public void detectPressedKeyOrButton()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                charstr = kcode.ToString().ToLower();
                Debug.Log(charstr);

                if (charstr.CompareTo("rightcontrol") == 0) { Debug.Log("Dyo going to maine"); SceneManager.LoadScene("MainMenu"); }
            }

        }
    }
 
    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
 
        string[] strarra = StrEntered.Split(',');
        if (strarra.Length == 4)
        {

            if (cur_data_PlayerInfo == null) cur_data_PlayerInfo = new Data_PlayerInfo();
            cur_data_PlayerInfo.PlayerFirstName = strarra[0];
            cur_data_PlayerInfo.PlayerLastName = strarra[1];
            cur_data_PlayerInfo.PlayerUserName = strarra[2];
            cur_data_PlayerInfo.PlayerEmail = strarra[3];

           // Debug.Log("you  entered " + cur_data_PlayerInfo.ToString());

            CurPlayerPoints = PersistantScoreGrabber.Instance.Get_Data_Player();
            
            Data_PlayerSession thisSession = new Data_PlayerSession(System.DateTime.Now, cur_data_PlayerInfo, CurPlayerPoints);

            _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(thisSession);
            textwasentered = true;


            StartCoroutine(AUTOGOTOGAME());
        }
        else
        {
            Debug.Log("inbvalid input , must re make inputfield active and start all over after deleting th einput text field");
            ResetInputFieldAndTxt();
            TextBoxField.ActivateInputField();
        }
    }

    void ResetInputFieldAndTxt() {
        TextBoxField.text = "";
        inputstring = "";
    }

    string inputstring;
    public void CatchSend() {

        // Debug.Log("on text changed text entered = " + TextBoxField.text);
        inputstring = TextBoxField.text;
      //  txt.text = inputstring;
        //  Debug.Log("input string " + inputstring);
        if (inputstring.Length > 0)
        {
            object lastchar = inputstring.ToCharArray().GetValue(inputstring.Length - 1);
            if (lastchar != null && lastchar is char)
            {
                char x = (char)lastchar;
                // Debug.Log("last char is + " + x);
                if (x == '*') {
                    Escape();
                }
            }
        }
    }


    void Escape() {
        Debug.Log("input string " + inputstring);
        string[] strarra = inputstring.Split(',');
        if (cur_data_PlayerInfo == null) cur_data_PlayerInfo = new Data_PlayerInfo();

        if (strarra.Length == 4)
        {
            cur_data_PlayerInfo.PlayerFirstName = strarra[0];
            cur_data_PlayerInfo.PlayerLastName = strarra[1];
            cur_data_PlayerInfo.PlayerUserName = strarra[2];
            cur_data_PlayerInfo.PlayerEmail = strarra[3];

            Debug.Log("you  entered " + cur_data_PlayerInfo.ToString());

            if (PersistantScoreGrabber.Instance == null) { CurPlayerPoints = new Data_PlayerPoints(1, 1, 1, 1, 1, 1, 1,1,1,1,1); }
            else
                CurPlayerPoints = PersistantScoreGrabber.Instance.Get_Data_Player();

            Debug.Log("the scores are " + CurPlayerPoints.ToString());


            Data_PlayerSession thisSession = new Data_PlayerSession(System.DateTime.Now, cur_data_PlayerInfo, CurPlayerPoints);

            _sessmngr.SaveSession_to_ALLSessions_AndSaveTOFile(thisSession);
            textwasentered = true;


            StartCoroutine(AUTOGOTOGAME());
        }
        else
        {
            ResetInputFieldAndTxt();
            TextBoxField.ActivateInputField();
        }
    }

    IEnumerator AUTOGOTOGAME()
    {
        Debug.Log("going to autogame in 5 sec");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("EditMapAUTO");
    }

   


    private void Update()
    {

        // detectPressedKeyOrButton();
        //if (hasbeenActivated && !textwasentered)
        //{
        //    DoActivateTextField();
        //}
    }

}
