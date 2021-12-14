using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Text nameField;
    [SerializeField] private Text attackValue;
    [SerializeField] private Text defenseValue;
    [SerializeField] private Text damageValue;
    [SerializeField] private Text speedValue;
    [SerializeField] private Text healthValue;
    [SerializeField] private Text floorValue;
    [Space(10)]
    [SerializeField] private GameObject CharacterSheetCanvas;

    private Statistics stats;

    public void OnGenerationComplete()
    {
        this.stats = FindObjectOfType<Player>()?.GetComponent<Statistics>();
        if (this.stats == null)
        {
            Debug.LogError("Could not find player statistics.");
            return;
        }
        this.UpdateUIValues();
        this.stats.OnStatsChanged += UpdateStats;
    }

    private void Start()
    {
        InputManager.Instance.CharacterDisplayPressed += EnableCharacterSheet;
        InputManager.Instance.CharacterDisplayReleased += DisableCharacterSheet;
    }

    private void OnDisable()
    {
        this.stats.OnStatsChanged -= UpdateStats;
        InputManager.Instance.CharacterDisplayPressed -= EnableCharacterSheet;
        InputManager.Instance.CharacterDisplayReleased -= DisableCharacterSheet;
    }

    private void UpdateStats()
    {
        this.UpdateUIValues();
    }

    private void UpdateUIValues()
    {
        this.nameField.text = this.stats.ActorName;
        this.attackValue.text = this.stats.Attack.ToString();
        this.defenseValue.text = this.stats.Defense.ToString();
        this.damageValue.text = this.stats.Damage.ToString();
        this.speedValue.text = this.stats.Speed.ToString();
        this.healthValue.text = $"{this.stats.CurrentHealth}/{this.stats.MaxHealth}";
        this.floorValue.text = GameManager.Instance.Floor.ToString();
    }

    private void EnableCharacterSheet()
    {
        this.CharacterSheetCanvas.SetActive(true);
        this.UpdateUIValues();
    }

    private void DisableCharacterSheet()
    {
        this.CharacterSheetCanvas.SetActive(false);
    }
}
