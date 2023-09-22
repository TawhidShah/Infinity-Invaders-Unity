using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : Menus
{
    //This method is called when the player clicks the "Play Again" button.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MAIN MENU");
    }

}
