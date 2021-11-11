using UnityEngine;
using UnityEngine.UI;

public class PlayerMutationBar : MonoBehaviour
{
    private Statistics player = null;
    private readonly int mutationBarMax = 50;

    [SerializeField] private Text mutationBarValue = null;
    [SerializeField] private Text mutationLevelValue = null;
    [SerializeField] private Image mutationBarForeground = null;

    public void OnGenerationComplete()
    {
        this.player = FindObjectOfType<Player>()?.GetComponent<Statistics>();
        if (this.player == null)
        {
            Debug.LogError("Could not find player statistics.");
        }
        this.UpdateMutationBarValues(this.player.MutationPoints);
        player.OnStatsChanged += UpdateMutationBar;
        player.MutationLevelUp += MutationLevelUp;
    }

    private void OnDisable()
    {
        player.OnStatsChanged -= UpdateMutationBar;
        player.MutationLevelUp -= MutationLevelUp;
    }

    private void UpdateMutationBar()
    {
        this.UpdateMutationBarValues(this.player.MutationPoints);
    }

    private void UpdateMutationBarValues(int currentMutationPoints)
    {
        mutationBarValue.text = $"{Mathf.RoundToInt(Mathf.Max(currentMutationPoints, 0))}/{this.mutationBarMax}";
        mutationBarForeground.fillAmount = (float)(Mathf.Max(currentMutationPoints, 0)) / this.mutationBarMax;
    }

    private void MutationLevelUp()
    {
        this.UpdateMutationBarValues(this.player.MutationPoints);
        mutationLevelValue.text = $"{this.player.MutationLevels}";
    }


}
