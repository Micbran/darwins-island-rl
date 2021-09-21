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
        this.UpdateHealthBarValues(this.stats.MaxHealth);
    }

    private void OnEnable()
    {
        stats.OnTakeDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= UpdateHealthBar;
    }

    private void Update()
    {
        if (healthBarMiddleground.fillAmount <= healthBarForeground.fillAmount)
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
