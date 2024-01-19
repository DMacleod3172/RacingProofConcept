using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApp : MonoBehaviour
{
        private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the game
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}

