using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadSceneAsync : MonoBehaviour
{
    //public GameObject loadscreen;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

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

        if (SceneManager.GetActiveScene().name != "Lose")
        {
            GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath = SceneManager.GetActiveScene().name;
            //print(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath);
        }

        GameObject.Find("ScreenFade").GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(1.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        //while (!asyncLoad.isDone)
        //{
            //yield return null;
        //}

        //temp disable for testing
        //GameObject.Find("ScreenFade").GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }
}