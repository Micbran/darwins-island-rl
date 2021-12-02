using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Statistics : MonoBehaviour
{
    public event Action<int> OnTakeDamage = delegate { };
    public event Action OnStatsChanged = delegate { };
    public event Action MutationLevelUp = delegate { };
    public event Action<Vector3> OnDeath = delegate { };


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

    private int mutationPoints;
    public int MutationPoints
    {
        get { return this.mutationPoints; }
    }

    private int mutationLevels;
    public int MutationLevels
    {
        get { return this.mutationLevels; }
    }

    public bool IsFullHealth
    {
        get { return this.CurrentHealth == this.MaxHealth; }
    }

    private List<Attack> attacks = new List<Attack>();
    public List<Attack> AttackList
    {
        get { return this.attacks.GetRange(0, this.attacks.Count); }
    }
    private List<Mutation> mutations = new List<Mutation>();
    public List<Mutation> MutationList
    {
        get { return this.mutations.GetRange(0, this.mutations.Count); }
    }


    [SerializeField] private CreatureStats starter;
    private Actor parent;
    private bool isAlive;

    private void Awake()
    {
        this.parent = this.GetComponent<Actor>();
        this.InitStats();

    }

    private void Start()
    {
    }

    private void InitStats()
    {
        this.isAlive = true;
        this.maxHealth = starter.health;
        this.currentHealth = this.maxHealth;

        this.attack = starter.attack;
        this.defense = starter.defense;
        this.damage = starter.damage;
        this.speed = starter.speed;

        this.attacks.AddRange(starter.attacks);
        this.mutationPoints = 0;

        if (starter is PlayerStats)
        {
            PlayerStats playerStarter = starter as PlayerStats;
            this.currentHealth = playerStarter.currentHealth;
            this.mutationPoints = playerStarter.mutationPoints;
            this.mutationLevels = playerStarter.mutationLevels;
            this.mutations.AddRange(playerStarter.mutations);
        }
        this.ApplyMutations();
        this.OnStatsChanged?.Invoke();
    }

    private void ApplyMutations()
    {
        foreach (Mutation mutation in this.mutations)
        {
            this.ApplyMutationEffect(mutation);
        }
    }

    private void ApplyMutationEffect(Mutation m)
    {
        this.maxHealth += m.healthChange;
        this.currentHealth = Mathf.Clamp(this.currentHealth + m.healthChange, 1, this.maxHealth);
        this.attack += m.attackChange;
        this.damage += m.damageChange;
        this.defense += m.defenseChange;
        this.speed += m.speedChange;
        if (m.bonusAttack)
        {
            this.attacks.Add(m.bonusAttack);
        }
    }

    public void DeapplyMutations()
    {
        foreach (Mutation mutation in this.mutations)
        {
            this.DeapplyMutationEffect(mutation);
        }
    }

    private void DeapplyMutationEffect(Mutation m)
    {
        this.maxHealth -= m.healthChange;
        this.currentHealth = Mathf.Clamp(this.currentHealth - m.healthChange, -999, this.maxHealth);
        this.attack -= m.attackChange;
        this.damage -= m.damageChange;
        this.defense -= m.defenseChange;
        this.speed -= m.speedChange;
        if (m.bonusAttack)
        {
            this.attacks.Remove(m.bonusAttack);
        }
    }

    public void Heal(int amount, string source)
    {
        int saveHealth = currentHealth;
        this.currentHealth = Mathf.Clamp(currentHealth + amount, 0, this.MaxHealth);
        int actualAmount = currentHealth - saveHealth;

        LogManager.Instance.AddNewResult(new BasicResult { message = $"{this.parent.actorName} was healed for {actualAmount} by {source}." });
        this.OnTakeDamage?.Invoke(this.CurrentHealth);
    }

    public void TakeDamage(int damageTaken)
    {
        this.currentHealth -= damageTaken;
        this.OnTakeDamage?.Invoke(this.currentHealth);
        bool isAlive = CheckIfAlive();
        if (isAlive && this.starter.HitSound != null)
        {
            AudioHelper.PlayClip2D(this.starter.HitSound, 1.0f);
        }
        if (this.starter.HitParticles != null)
        {
            Instantiate(this.starter.HitParticles, this.transform.position, Quaternion.identity);
        }
    }

    public void TakeAttack(int attackRoll, int damageRoll)
    {
        if (attackRoll >= this.defense)
        {
            this.TakeDamage(damageRoll);
            return;
        }
        if (this.starter.MissSound != null)
        {
            AudioHelper.PlayClip2D(this.starter.MissSound, 1.5f);
        }
    }

    public void MakeAttacks(Actor targetActor)
    {
        Statistics targetStats = targetActor.gameObject.GetComponent<Statistics>();
        if (targetStats == null)
        {
            return;
        }

        List<(int, int)> attackDamageList = new List<(int, int)>();
        List<Attack> specialAttacks = new List<Attack>();
        foreach (Attack attack in attacks)
        {
            int attackResult = GlobalRandom.AttackRoll();
            int damageResult = attack.RollAttack();
            if (attack.AttackType == AttackType.Single)
            {
                AttackResult result = new AttackResult()
                {
                    resultAttack = attack,
                    attackRollTotal = attackResult + this.attack,
                    defense = targetStats.Defense,
                    damageRoll = damageResult,
                    damageBonus = this.damage,
                    resultSource = this.parent.actorName,
                    resultTarget = targetActor.actorName
                };
                attackDamageList.Add((attackResult, damageResult));
                LogManager.Instance.AddNewResult(result);
            }
            else if (attack.AttackType == AttackType.PBAoeRange1)
            {
                specialAttacks.Add(attack);
            }
        }

        this.ResolveAoEAttacks(specialAttacks);

        foreach ((int, int) attackDamagePair in attackDamageList)
        {
            targetStats.TakeAttack(attackDamagePair.Item1 + this.attack, attackDamagePair.Item2 + this.damage);
        }

        if(this.starter.AttackSound != null)
        {
            AudioHelper.PlayClip2D(this.starter.AttackSound, 1.0f);
        }

    }

    private void ResolveAoEAttacks(List<Attack> specialAttacks)
    {
        foreach (Attack sa in specialAttacks)
        {
            List<CollisionInformation> directions = new List<CollisionInformation>();
            Vector3 currentPosition = this.transform.position;
            directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.left, 0.4f), currentPosition, Vector3.left));
            directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.right, 0.4f), currentPosition, Vector3.right));
            directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.forward, 0.4f), currentPosition, Vector3.forward));
            directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.back, 0.4f), currentPosition, Vector3.back));

            foreach (CollisionInformation ci in directions)
            {
                List<(int, int)> attackDamageList = new List<(int, int)>();
                foreach (Collider collide in ci)
                {
                    Statistics statsCheck = collide.GetComponent<Statistics>();
                    if (statsCheck != null)
                    {
                        int attackResult = GlobalRandom.AttackRoll();
                        int damageResult = sa.RollAttack();

                        AttackResult result = new AttackResult()
                        {
                            resultAttack = sa,
                            attackRollTotal = attackResult + this.attack,
                            defense = statsCheck.Defense,
                            damageRoll = damageResult,
                            damageBonus = 0,
                            resultSource = this.parent.actorName,
                            resultTarget = statsCheck.parent.actorName
                        };
                        LogManager.Instance.AddNewResult(result);
                        statsCheck.TakeAttack(attackResult + this.Attack, damageResult);
                    }
                }
            }
        }
    }

    public void IncreaseMutationPoints(int value)
    {
        this.mutationPoints += value;
        this.CheckForMutationLevelUp();
        LogManager.Instance.AddNewResult(new BasicResult() { message = $"You gained {value} mutation points!" });
        this.OnStatsChanged?.Invoke();
    }

    private void CheckForMutationLevelUp()
    {
        if (this.mutationPoints >= 50)
        {
            this.mutationLevels++;
            this.mutationPoints -= 50;
            this.MutationLevelUp?.Invoke();
        }
    }

    private bool CheckIfAlive()
    {
        if (this.currentHealth <= 0 && this.isAlive)
        {
            this.isAlive = false;
            this.OnDeath?.Invoke(this.transform.position);
            this.parent.KillActor();
            if (this.starter.DeathSound != null)
            {
                AudioHelper.PlayClip2D(this.starter.DeathSound, 1.0f);
            }
            return false;
        }
        return true;
    }


}
