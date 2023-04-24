using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public float waitTime = 1f; // The amount of time to wait before loading the next scene

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay("Menu"));
    }

    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Menu");
    }
}
