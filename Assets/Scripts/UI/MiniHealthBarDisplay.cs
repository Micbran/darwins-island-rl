using UnityEngine;
using UnityEngine.UI;

public class MiniHealthBarDisplay : MonoBehaviour
{
    private Statistics stats = null;
    [SerializeField] private Image healthBarForeground = null;
    [SerializeField] private GameObject healthBarGroup = null;

    private void Awake()
    {
        this.stats = this.GetComponent<Statistics>();
        this.UpdateHealthBarValues(this.stats.CurrentHealth);
    }

    private void OnEnable()
    {
        stats.OnTakeDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int newHealth)
    {
        this.UpdateHealthBarValues(newHealth);
    }

    private void UpdateHealthBarValues(int newHealth)
    {
        if (newHealth != this.stats.MaxHealth)
        {
            this.healthBarGroup.SetActive(true);
        }
        healthBarForeground.fillAmount = (float)(Mathf.Max(newHealth, 0)) / this.stats.MaxHealth;
    }
}
