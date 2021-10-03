using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SonicSceneController : MonoBehaviour
{
    //Creates a public field in the inspector to place the SceneLoader object.
    public SceneLoader loader;

    //Loads the next scene in the build scene order.
    public void LoadNextLevel()
    {
        loader.LoadNextLevel();
    }

    //Loads the previous scene in the build scene order.
    public void RestartGame()
    {
        loader.LoadNextLevel(-1);
    }

    //Loads the Classic Arcade main menu.
    public void MainMenu()
    {
        loader.LoadMainMenu();
    }

    public void RestartLevel()
    {
        loader.LoadSceneName("GreenHillZoneAct1");
    }
}
