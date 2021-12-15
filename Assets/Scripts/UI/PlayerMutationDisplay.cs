using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        List<Mutation> copyList = this.player.MutationList;
        IEnumerable<IGrouping<Mutation, Mutation>> groupList = copyList.GroupBy(m => m);
        foreach (IGrouping<Mutation, Mutation> group in groupList)
        {
            MutationDescriptionEntry entry = Instantiate(this.UIElement, this.gridObject.transform);
            entry.mutationName = group.Key.mutationName;
            entry.mutationDescription = group.Key.description;
            entry.mutationNumber = group.Count().ToString();
            entry.mutationStackText = this.DetermineMutationStackText(group.Key, group.Count());

            entry.RefreshUI();

            this.mutationFields.Add(entry);
        }
    }

    private string DetermineMutationStackText(Mutation mutation, int stacks)
    {
        string finalString = mutation.stackDescription;
        MatchCollection matches = Regex.Matches(finalString, @"{([0-9])+}");
        foreach (Match m in matches)
        {
            int stackValue = int.Parse(m.Groups[1].Captures[0].ToString());
            int totalValue = stackValue * stacks;
            Regex reg = new Regex(@"{[0-9]+}");
            finalString = reg.Replace(finalString, totalValue.ToString(), 1);
        }

        return finalString;
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
