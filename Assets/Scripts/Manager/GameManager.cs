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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Set to player's turn.");
            TurnManager.Instance.IsPlayersTurn = true;
        }
    }
}
