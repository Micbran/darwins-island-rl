using System;
using UnityEngine;

public class InputManager : LevelManager<InputManager>
{
    public event Action MutationDisplayPressed = delegate { };
    public event Action MutationDisplayReleased = delegate { };
    public event Action CharacterDisplayPressed = delegate { };
    public event Action CharacterDisplayReleased = delegate { };

    private void Update()
    {
        this.CheckMutationDisplay();
        this.CheckCharacterSheetDisplay();
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

    private void CheckCharacterSheetDisplay()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.CharacterDisplayPressed?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            this.CharacterDisplayReleased?.Invoke();
        }
    }
}
