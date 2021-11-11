using System;
using UnityEngine;
using UnityEngine.UI;

public class MutationButton : MonoBehaviour
{
    public event Action<Mutation> MutationChosen = delegate { };

    public Mutation representedMutation;
    [SerializeField] private GameObject popoverField;
    [SerializeField] private Text popoverFieldText;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    private bool isHovered = false;

    public void RefreshUI ()
    {
        this.buttonText.text = this.representedMutation.mutationName;
        this.popoverFieldText.text = this.representedMutation.description;
        if (this.button.interactable)
        {
            this.popoverField.SetActive(this.isHovered);
        }
    }

    public void ChangeHoverState(bool toState)
    {
        this.isHovered = toState;
        this.RefreshUI();
    }

    public void UpgradeChosen()
    {
        this.MutationChosen?.Invoke(this.representedMutation);
    }

    public void DisableSelf()
    {
        this.isHovered = false;
        this.RefreshUI();
        this.button.interactable = false;
    }
}
