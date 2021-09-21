using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Text attackValue;
    [SerializeField] private Text defenseValue;
    [SerializeField] private Text damageValue;
    [SerializeField] private Text speedValue;

    private Statistics stats;

    private void Awake()
    {
        this.stats = FindObjectOfType<Player>()?.GetComponent<Statistics>();
        if (this.stats == null)
        {
            Debug.LogError("Could not find player statistics.");
        }
    }

    private void Start()
    {
        this.UpdateUIValues();
    }

    private void OnEnable()
    {
        this.stats.OnStatsChanged += UpdateStats;
    }

    private void OnDisable()
    {
        this.stats.OnStatsChanged -= UpdateStats;
    }

    private void UpdateStats()
    {
        this.UpdateUIValues();
    }

    private void UpdateUIValues()
    {
        attackValue.text = this.stats.Attack.ToString();
        defenseValue.text = this.stats.Defense.ToString();
        damageValue.text = this.stats.Damage.ToString();
        speedValue.text = this.stats.Speed.ToString();
    }
}
