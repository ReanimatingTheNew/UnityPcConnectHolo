// @Author Nabil Lamriben ©2017
// ok this is  ahack and a half 
// todonabil better button and dont destroy this 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartReset : MonoBehaviour {
    public TextMesh tm;
    string NameOfSceneToReload;
    public GameObject StartBlock;
    public GameObject ResetBlock;
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        NameOfSceneToReload = scene.name;

     
        //Debug.Log("Active scene is '" + scene.name + "'.");
    }

    public void ShowReset() {
        StartBlock.SetActive(true);
        ResetBlock.SetActive(true);
    }

    public void ShowBoth()
    {
        StartBlock.SetActive(true);
        ResetBlock.SetActive(true);
    }

    public void HideBoth()
    {
        StartBlock.SetActive(false);
        ResetBlock.SetActive(false);
    }


    public void StartTheGame()
    {
       GameManager.Instance.CheckStartGame();
       Destroy(this.gameObject);
    }
    public void ResetTheGame()
    {
        //StartBlock.SetActive(true);
        //GameManager.Instance.DeActivateStartReset();
        GameManager.Instance.LoadScene(NameOfSceneToReload);      
    }
    public void CallScreen(object o)
    {
        if (o is string)
        {
            if (o.ToString() == "StartGameBlock") { StartTheGame(); }
            else
                 if (o.ToString() == "ResetGameBlock") { ResetTheGame(); }

        }
    }
}
