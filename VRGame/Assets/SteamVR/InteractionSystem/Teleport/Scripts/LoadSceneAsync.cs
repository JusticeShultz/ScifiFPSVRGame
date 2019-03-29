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
        print("trying");
        StartCoroutine(LoadScene(GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath));
    }

    IEnumerator LoadScene(string scene)
    {
        loadscreen.SetActive(true);
        GameObject.Find("PlayerCollider").GetComponent<LastLoadPoint>().sceneBeforeDeath = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadscreen.SetActive(false);
        Destroy(gameObject);
    }
}