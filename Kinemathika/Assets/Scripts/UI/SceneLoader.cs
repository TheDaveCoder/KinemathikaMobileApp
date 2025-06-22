using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public void LoadScene(int startIndex)
    {
        StartCoroutine(LoadSceneAsynchronously(startIndex));
    }

    IEnumerator LoadSceneAsynchronously(int startIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(startIndex);
        loadingScreen.SetActive(true);  
        while (!operation.isDone)
        {

            loadingBar.value = operation.progress;
            yield return null;
        }
    }
}
