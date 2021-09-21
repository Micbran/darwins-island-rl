using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Statistics : MonoBehaviour
{
    public event Action<int> OnTakeDamage = delegate { };
    public event Action OnStatsChanged = delegate { };


    private int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return this.currentHealth;
        }
    }

    private int maxHealth;
    public int MaxHealth
    {
        get
        {
            return this.maxHealth;
        }
    }

    private int attack;
    public int Attack
    {
        get { return this.attack; }
    }
    private int defense;
    public int Defense
    {
        get { return this.defense; }
    }
    private int damage;
    public int Damage
    {
        get { return this.damage; }
    }
    private int speed;
    public int Speed
    {
        get { return this.speed; }
    }

    private List<Attack> attacks = new List<Attack>();

    [SerializeField] private CreatureStats starter;
    private Actor parent;

    private void Awake()
    {
        this.parent = this.GetComponent<Actor>();
        InitStats();
    }

    private void InitStats()
    {
        this.currentHealth = starter.health;
        this.maxHealth = this.currentHealth;

        this.attack = starter.attack;
        this.defense = starter.defense;
        this.damage = starter.damage;
        this.speed = starter.speed;

        this.attacks.AddRange(starter.attacks);
    }

    public void TakeDamage(int damageTaken)
    {
        Debug.Log($"Took damage: {damageTaken}, {this.currentHealth} -> {this.currentHealth - damageTaken}");
        this.currentHealth -= damageTaken;
        this.OnTakeDamage?.Invoke(this.currentHealth);
        CheckIfAlive();
    }

    public void TakeAttack(int attackRoll, int damageRoll)
    {
        if (attackRoll >= this.defense)
        {
            Debug.Log($"Got hit: {attackRoll} vs. {this.defense} for {damageRoll}!");
            this.TakeDamage(damageRoll);
        }
    }

    public void MakeAttacks(Actor targetActor)
    {
        Statistics targetStats = targetActor.gameObject.GetComponent<Statistics>();
        if (targetStats == null)
        {
            return;
        }

        foreach (Attack attack in attacks)
        {
            int attackResult = GlobalRandom.AttackRoll();
            int damageResult = attack.RollAttack();
            Debug.Log("Attack made: " + attack.ToString() + $" -> {attackResult + this.attack} ({attackResult} + {this.attack}) result for {damageResult} damage.");
            targetStats.TakeAttack(attackResult + this.attack, damageResult + this.damage);
        }
        
    }

    private void CheckIfAlive()
    {
        if (this.currentHealth <= 0)
        {
            this.parent.KillActor();
        }
    }


}
