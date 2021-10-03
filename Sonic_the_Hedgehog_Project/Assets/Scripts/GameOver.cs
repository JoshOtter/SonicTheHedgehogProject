using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public SonicSceneController gameOver;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameOver.RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameOver.MainMenu();
        }
    }
}
