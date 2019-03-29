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
        SuccessfulLoadin.prevScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene(switchToScene));
    }

    public void GoToLastScene()
    {
        StartCoroutine(LoadScene(SuccessfulLoadin.prevScene));
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
        Destroy(gameObject);
    }
}