using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is the parent class of all menus, contains method needed in all menus.
public class Menus : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
}
