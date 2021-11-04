using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    private int currentFloor = 1;
    private bool interimScene = false;

    public void GameOver()
    {
        enabled = false;
        SceneManager.LoadScene("Game Over");
    }

    public void NewGame()
    {
        enabled = true;
        this.currentFloor = 1;
    }

    public void TransitionLevel()
    {
        interimScene = !interimScene;
        if (interimScene)
        {
            SceneManager.LoadScene("Between Level");
            this.currentFloor++;
            return;
        }
        SceneManager.LoadScene("Level2");
    }
}
