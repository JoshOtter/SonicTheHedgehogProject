using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;

    //this should match the wipe animation time
    public float transitionTime = 1f;

    //use a negative value to load the previous scene
    public void LoadNextLevel(int index = 1)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + index));
    }

    public void LoadSceneName(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    //loads level based on build index
    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    //loads level based on scene name
    private IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
}
