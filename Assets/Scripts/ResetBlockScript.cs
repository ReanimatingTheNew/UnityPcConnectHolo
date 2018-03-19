// @Author Nabil Lamriben ©2017
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetBlockScript : MonoBehaviour {

    string NameOfSceneToReload;
    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        NameOfSceneToReload = scene.name;
    }
    public void TakeHit(Bullet bullet)
    {
        GameManager.Instance.LoadScene(NameOfSceneToReload);
    }
}
