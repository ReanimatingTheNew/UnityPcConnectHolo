// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class About : MonoBehaviour {

    [Tooltip("Text field with version information.")]
    public Text versionText;

	// Use this for initialization
	void Start () {
        versionText.text = "ARZ Arcade";// + Application.version.ToString();
	}
}
