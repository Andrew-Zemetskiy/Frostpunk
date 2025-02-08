using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    
    public void PlayGame()
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
