using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicTitleMenu : MonoBehaviour
{
    //Creates a public field in the inspector to place the SonicSceneController script in order to call it's functions.
    public SonicSceneController startGame;
    // Update is called once per frame
    void Update()
    {
        //When the user presses the enter key, the game loads the next scene.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            startGame.LoadNextLevel();
        }
    }
}
