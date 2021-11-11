using System;
using UnityEngine;

public class InputManager : LevelManager<InputManager>
{
    public event Action MutationDisplayPressed = delegate { };
    public event Action MutationDisplayReleased = delegate { };

    private void Update()
    {
        CheckMutationDisplay();
    }

    private void CheckMutationDisplay()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.MutationDisplayPressed?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            this.MutationDisplayReleased?.Invoke();
        }
    }
}
