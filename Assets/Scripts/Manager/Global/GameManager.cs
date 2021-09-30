using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    private int currentFloor = 1;

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
        if (this.currentFloor == 1)
        {
            SceneManager.LoadScene("Between Level");
        }
        else if (this.currentFloor == 2)
        {
            SceneManager.LoadScene("Level1");
        }
        currentFloor++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Set to player's turn.");
            TurnManager.Instance.IsPlayersTurn = true;
        }
    }
}
