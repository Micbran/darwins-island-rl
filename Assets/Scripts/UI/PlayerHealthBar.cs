using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Statistics stats = null;

    [SerializeField] private Text healthBarValue = null;
    [SerializeField] private Image healthBarForeground = null;
    [SerializeField] private Image healthBarMiddleground = null;
    [SerializeField] private float secondaryHealthBarDrainRate = 0.0002f;

    public void OnGenerationComplete()
    {
        this.stats = FindObjectOfType<Player>()?.GetComponent<Statistics>();
        if (this.stats == null)
        {
            Debug.LogError("Could not find player statistics.");
        }
        this.UpdateHealthBarValues(this.stats.MaxHealth);
        stats.OnTakeDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= UpdateHealthBar;
    }

    private void FixedUpdate()
    {
        if (this.healthBarMiddleground == null) return;
        if (healthBarMiddleground?.fillAmount <= healthBarForeground.fillAmount)
            return;
        healthBarMiddleground.fillAmount -= secondaryHealthBarDrainRate;

    }

    private void UpdateHealthBar(int currentHealth)
    {
        this.UpdateHealthBarValues(currentHealth);
    }

    private void UpdateHealthBarValues(int healthValue)
    {
        healthBarValue.text = $"{Mathf.RoundToInt(Mathf.Max(healthValue, 0))}/{this.stats.MaxHealth}";
        healthBarForeground.fillAmount = (float)(Mathf.Max(healthValue, 0)) / this.stats.MaxHealth;
    }
}
