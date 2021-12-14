using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    [SerializeField] public PlayerStats statsSave;
    [SerializeField] private PlayerStats debugReset;
    private int currentFloor = 1;
    public int Floor => this.currentFloor;
    private bool isStaticLevel = false;
    public bool IsStaticLevel => this.isStaticLevel;    
    private bool interimScene = false;

    public override void Awake()
    {
        base.Awake();
        if (resetRun) return;
        statsSave.health = debugReset.health;
        statsSave.attack = debugReset.attack;
        statsSave.defense = debugReset.defense;
        statsSave.damage = debugReset.damage;
        statsSave.speed = debugReset.speed;
        statsSave.attacks = debugReset.attacks.GetRange(0, debugReset.attacks.Count);

        statsSave.currentHealth = debugReset.currentHealth;
        statsSave.mutationLevels = debugReset.mutationLevels;
        statsSave.mutationPoints = debugReset.mutationPoints;
        statsSave.mutations = debugReset.mutations.GetRange(0, debugReset.mutations.Count);
        resetRun = true;
    }

    public void GameOver()
    {
        enabled = false;
        SceneManager.LoadScene("Game Over");
    }

    public void NewGame()
    {
        enabled = true;
        resetRun = false;
        this.isStaticLevel = false;
        this.interimScene = false;
        this.currentFloor = 1;
    }

    public void TransitionLevel()
    {
        this.UpdatePlayerStats();
        interimScene = !interimScene;
        if (interimScene)
        {
            SceneManager.LoadScene("Between Level");
            this.currentFloor++;
            return;
        }
        if (currentFloor <= 6)
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            this.isStaticLevel = true;
            SceneManager.LoadScene("BossArena");
        }

    }

    public void EndGame()
    {
        SceneManager.LoadScene("Win Screen");
    }

    private void UpdatePlayerStats()
    {
        Player playerRef = FindObjectOfType<Player>();
        if (playerRef == null)
        {
            Debug.LogError("Cannot find player to save.");
            return;
        }
        Statistics playerStats = playerRef.GetComponent<Statistics>();
        if (playerStats == null)
        {
            Debug.LogError("Cannot find player stats to save.");
            return;
        }
        playerStats.DeapplyMutations();

        this.statsSave.health = playerStats.MaxHealth;
        this.statsSave.attack = playerStats.Attack;
        this.statsSave.defense = playerStats.Defense;
        this.statsSave.damage = playerStats.Damage;
        this.statsSave.speed = playerStats.Speed;
        this.statsSave.attacks = playerStats.AttackList.GetRange(0, playerStats.AttackList.Count);

        this.statsSave.currentHealth = playerStats.CurrentHealth;
        this.statsSave.mutationLevels = playerStats.MutationLevels;
        this.statsSave.mutationPoints = playerStats.MutationPoints;
        this.statsSave.mutations = playerStats.MutationList.GetRange(0, playerStats.MutationList.Count);
    }

    // Check for debug input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            // Heal player to full
            FindObjectOfType<Player>()?.DebugHealFull();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            // Give player full mutation point
            FindObjectOfType<Player>()?.DebugGiveMutationPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.TransitionLevel();
        }
    }
}
