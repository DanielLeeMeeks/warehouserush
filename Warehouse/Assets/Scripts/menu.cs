using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour {

    public Canvas[] menus;
    public GameObject[] howToPlayThings;
    int howToPlayStep=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToMenu(Canvas go) {
        foreach (Canvas c in menus) {
            c.enabled = false;
            go.enabled = true;
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void moreGames() {
        Application.OpenURL("https://danielleemeeks.itch.io/");
    }

    public void loadLevel(string levelName) {
        PlayerPrefs.SetString("level", levelName);
        Application.LoadLevel("level");
    }

    public void nextHowToPlay(int step) {
        howToPlayStep = howToPlayStep + step;
        if (howToPlayStep < 0) { howToPlayStep = howToPlayThings.Length - 1; }
        if (howToPlayStep > howToPlayThings.Length-1) { howToPlayStep = 0; }

        foreach (GameObject v in howToPlayThings) { v.SetActive(false); }
        howToPlayThings[howToPlayStep].SetActive(true);

    }

}
