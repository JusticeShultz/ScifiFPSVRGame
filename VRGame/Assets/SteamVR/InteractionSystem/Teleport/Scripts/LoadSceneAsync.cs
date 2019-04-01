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
        GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath = SceneManager.GetActiveScene().name;
        print(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath);
        loadscreen.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadscreen.SetActive(false);
        Destroy(gameObject);
    }
}