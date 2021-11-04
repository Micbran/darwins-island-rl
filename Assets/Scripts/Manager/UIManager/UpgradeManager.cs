using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    private bool upgradeChosen = false;

    public void UpgradeChosen()
    {
        upgradeChosen = true;
        this.continueButton.interactable = true;
    }

    public void Continue()
    {
        GameManager.Instance.TransitionLevel();
    }
}
