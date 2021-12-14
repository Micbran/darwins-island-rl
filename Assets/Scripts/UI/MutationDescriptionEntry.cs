using UnityEngine;
using UnityEngine.UI;

public class MutationDescriptionEntry : MonoBehaviour
{
    public string mutationName;
    public string mutationDescription;
    public string mutationNumber;
    [SerializeField] private Text mutationLabel;
    [SerializeField] private Text mutationText;

    private void Awake()
    {
        this.RefreshUI();
    }

    public void RefreshUI()
    {
        this.mutationLabel.text = this.mutationName + $" (x{this.mutationNumber})";
        this.mutationText.text = this.mutationDescription;
    }
}
