using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationDisplay : MonoBehaviour
{
    [SerializeField] private GameObject gridObject;
    [SerializeField] private MutationDescriptionEntry UIElement;
    private Statistics player;
    private List<MutationDescriptionEntry> mutationFields = new List<MutationDescriptionEntry>();

    private void Start()
    {
        InputManager.Instance.MutationDisplayPressed += BuildMutationDisplay;
        InputManager.Instance.MutationDisplayReleased += TearDownMutationDisplay;
    }

    private void OnDisable()
    {
        InputManager.Instance.MutationDisplayPressed -= BuildMutationDisplay;
        InputManager.Instance.MutationDisplayReleased -= TearDownMutationDisplay;
    }

    public void OnGenerationComplete()
    {
        this.player = FindObjectOfType<Player>()?.GetComponent<Statistics>();
        if (this.player == null)
        {
            Debug.LogError("Could not find player statistics.");
        }
    }

    private void BuildMutationDisplay()
    {
        this.gridObject.SetActive(true);
        foreach (Mutation m in this.player.MutationList)
        {
            MutationDescriptionEntry entry = Instantiate(this.UIElement, this.gridObject.transform);
            entry.mutationName = m.mutationName;
            entry.mutationDescription = m.description;

            entry.RefreshUI();

            this.mutationFields.Add(entry);
        }
    }

    private void TearDownMutationDisplay()
    {
        foreach (MutationDescriptionEntry mDesc in this.mutationFields)
        {
            Destroy(mDesc.gameObject);
        }
        this.mutationFields.Clear();
        this.gridObject.SetActive(false);
        
    }
}
