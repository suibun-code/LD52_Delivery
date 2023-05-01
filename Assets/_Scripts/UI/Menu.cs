using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GoToMenu()
    {
        Debug.Log("Loading menu scene...");
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        Debug.Log("Loading game scene...");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
