using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.NewGame();
        }
        SceneManager.LoadScene("Level2");
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
