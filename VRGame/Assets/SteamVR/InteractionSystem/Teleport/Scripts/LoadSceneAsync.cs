using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadSceneAsync : MonoBehaviour
{
    public GameObject loadscreen;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (loadscreen == null)
            loadscreen = GameObject.Find("Fade");

        if (SceneManager.GetActiveScene().name == "VeryveryStartForRealsThisTime") Do("StartMenu");
    }

    public void Do(string switchToScene)
    {
        StartCoroutine(LoadScene(switchToScene));
    }

    public void GoToLastScene()
    {
        print(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath);
        
        StartCoroutine(LoadScene(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath));
    }

    IEnumerator LoadScene(string scene)
    {
        //print("I'm trying to load " + scene);
        GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath = SceneManager.GetActiveScene().name;
        print(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath);

        if (loadscreen == null)
            loadscreen = GameObject.Find("Fade");

        loadscreen.GetComponent<SpriteRenderer>().enabled = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        loadscreen.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }
}