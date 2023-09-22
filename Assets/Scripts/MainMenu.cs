using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menus
{
    //This method is called when the player clicks the "Play" button.
    public void PlayGame()
    {
        SceneManager.LoadScene("GAME");
    }

    //This method is called when the player clicks the "Quit" button.
    public void DeleteHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
