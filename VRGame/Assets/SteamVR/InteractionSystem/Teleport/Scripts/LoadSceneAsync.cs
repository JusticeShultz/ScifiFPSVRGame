using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneAsync : MonoBehaviour
{
    public GameObject loadscreen;

    public void Do(string switchToScene)
    {
        StartCoroutine(LoadScene(switchToScene));
    }

    IEnumerator LoadScene(string scene)
    {
        loadscreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadscreen.SetActive(false);
    }
}