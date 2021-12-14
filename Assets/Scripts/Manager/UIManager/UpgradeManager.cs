using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private MutationButton MutationButton;
    [SerializeField] private GameObject ButtonContainer;
    [SerializeField] private List<Mutation> mutationMasterList = new List<Mutation>();
    [SerializeField] private Button continueButton;
    [SerializeField] private Text titleText;
    [SerializeField] private Text floorText;

    private List<Mutation> chosenMutations = new List<Mutation>();
    private List<MutationButton> mutationButtons = new List<MutationButton>();
    private PlayerStats playerStats;
    private bool upgradeChosen;


    private void Awake()
    {
        this.playerStats = GameManager.Instance.statsSave;
        this.upgradeChosen = !this.PlayerHasMutationsPending();
        this.RefreshUI();
        this.SetFloorIndicator();
        if (!upgradeChosen)
        {
            this.ChooseMutations();
        }
        else
        {
            this.UpgradeChosen();
        }
    }

    private void SetFloorIndicator()
    {
        int currentFloor = GameManager.Instance.Floor;
        if (currentFloor > 6)
        {
            this.floorText.text = "Boss floor next!";
        }
        else
        {
            this.floorText.text = this.floorText.text.Replace("{X}", currentFloor.ToString());
        }
    }

    public void UpgradeChosen()
    {
        upgradeChosen = true;
        this.RefreshUI();
    }

    public void Continue()
    {
        GameManager.Instance.TransitionLevel();
    }

    private void RefreshUI()
    {
        this.continueButton.interactable = this.upgradeChosen;
        if (this.upgradeChosen)
        {
            titleText.text = "Continue?";
        }
    }

    private bool PlayerHasMutationsPending()
    {
        return this.playerStats.mutationLevels > 0;
    }

    private void ChooseMutations()
    {
        int mutationCount = 3;
        List<Mutation> mutationMasterListCopy = this.mutationMasterList.GetRange(0, this.mutationMasterList.Count);
        for (int i = 0; i < mutationCount; i++)
        {
            Mutation chosenMutation = mutationMasterListCopy[GlobalRandom.RandomInt(mutationMasterListCopy.Count - 1, 0)];
            mutationMasterListCopy.Remove(chosenMutation);
            this.chosenMutations.Add(chosenMutation);
        }

        foreach (Mutation m in this.chosenMutations)
        {
            MutationButton reference = Instantiate(this.MutationButton, this.ButtonContainer.transform);
            reference.representedMutation = m;
            reference.MutationChosen += OnMutationChosen;
            reference.RefreshUI();
            this.mutationButtons.Add(reference);
        }
    }

    private void OnMutationChosen(Mutation chosen)
    {
        foreach (MutationButton button in mutationButtons)
        {
            button.MutationChosen -= OnMutationChosen;
            button.DisableSelf();
        }
        this.playerStats.mutations.Add(chosen);
        this.playerStats.mutationLevels--;

        if (!this.PlayerHasMutationsPending())
        {
            this.UpgradeChosen();
        }
        else
        {
            foreach (MutationButton button in mutationButtons)
            {
                Destroy(button.gameObject);
            }
            this.mutationButtons.Clear();
            this.chosenMutations.Clear();
            this.ChooseMutations();
        }
    }
}
