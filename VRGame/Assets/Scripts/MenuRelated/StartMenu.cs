using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// contains functions to bind to start menu buttons
public class StartMenu : MonoBehaviour {

    public float heightScaleValue;

    LoadSceneAsync sceneLoader;

    GameObject player;

    GameObject mainMenu;
    GameObject settingsMenu;
    GameObject levelMenu;
    GameObject calibWall;

    Vector3 newScale;

    private void Start()
    {
        player = GameObject.Find("Player");

        mainMenu = transform.GetChild(0).gameObject;
        settingsMenu = transform.GetChild(1).gameObject;
        levelMenu = transform.GetChild(2).gameObject;
        calibWall = GameObject.Find("CalibrationWall");

        newScale = player.transform.localScale;

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        calibWall.SetActive(false);
        levelMenu.SetActive(false);

        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadSceneAsync>();
    }

    // starts with tutorial
    public void StartGame()
    {
        sceneLoader.Do("Tutorial");
    }

    public void OpenLevel(string level)
    {
        sceneLoader.Do(level);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        calibWall.SetActive(true);
    }

    public void LevelSelect()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void ViewHighscores()
    {

    }

    public void RecalibrateHeight()
    {
        UnityEngine.XR.InputTracking.Recenter(); // doesn't work?
    }

    // true if making taller
    public void Scale(bool up)
    {
        // newScale.y += up ? heightScaleValue : -heightScaleValue;
        newScale *= 1 + (up ? heightScaleValue : -heightScaleValue);
        player.transform.localScale = newScale;
        UnityEngine.XR.InputTracking.Recenter();
    }

    public void BackSettings()
    {
        settingsMenu.SetActive(false);
        calibWall.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void BackLevels()
    {
        levelMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
