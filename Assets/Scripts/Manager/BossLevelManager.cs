using System.Collections.Generic;
using UnityEngine;

public class BossLevelManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [Space(10)]
    [SerializeField] private List<Transform> spawnTransforms = new List<Transform>();
    [SerializeField] private Statistics bossStats;
    [SerializeField] private LevelChange stairsPrefab;
    [Space(10)]
    [SerializeField] private Actor Bee;
    [SerializeField] private Actor AcidSpitter;
    [SerializeField] private Actor Spider;

    private int bossMaxHP;
    private int bossCurrHP;

    private bool trigger75 = false;
    private bool trigger50 = false;
    private bool trigger25 = false;

    private void Awake()
    {

    }

    private void Start()
    {
        this.bossMaxHP = this.bossStats.MaxHealth;
        this.bossCurrHP = this.bossStats.CurrentHealth;
    }

    private void OnEnable()
    {
        this.bossStats.OnTakeDamage += OnBossTakeDamage;
        this.bossStats.OnDeath += OnBossDeath;
    }

    private void OnDisable()
    {
        
        this.bossStats.OnTakeDamage -= OnBossTakeDamage;
        this.bossStats.OnDeath -= OnBossDeath;
    }

    private void OnBossTakeDamage(int newHealth)
    {
        this.bossCurrHP = newHealth;
        float healthRatio = (float)this.bossCurrHP / (float)this.bossMaxHP;
        if (!this.trigger75 && healthRatio <= 0.75)
        {
            this.trigger75 = true;
            this.OnBoss75Health();
        }
        if (!this.trigger50 && healthRatio <= 0.50)
        {
            this.trigger50 = true;
            this.OnBoss50Health();
        }
        if (!this.trigger25 && healthRatio <= 0.25)
        {
            this.trigger25 = true;
            this.OnBoss25Health();
        }
    }

    private void OnBoss75Health()
    {
        List<Vector3> spawnPositions = this.GetSpawnPositions();
        foreach (Vector3 position in spawnPositions)
        {
            Actor reference = Instantiate(this.Bee, position, Quaternion.identity);
            this.turnManager.AddActorToTurnOrder(reference);
        }
    }

    private void OnBoss50Health()
    {
        List<Vector3> spawnPositions = this.GetSpawnPositions();
        foreach (Vector3 position in spawnPositions)
        {
            Actor reference = Instantiate(this.Spider, position, Quaternion.identity);
            this.turnManager.AddActorToTurnOrder(reference);
        }
    }

    private void OnBoss25Health()
    {
        List<Vector3> spawnPositions = this.GetSpawnPositions();
        foreach (Vector3 position in spawnPositions)
        {
            Actor reference = Instantiate(this.AcidSpitter, position, Quaternion.identity);
            this.turnManager.AddActorToTurnOrder(reference);
        }
    }

    private List<Vector3> GetSpawnPositions()
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        List<Transform> spawnTransformsCopy = this.spawnTransforms.GetRange(0, this.spawnTransforms.Count);
        int iterations = 0;
        while(spawnPositions.Count < 3)
        {
            if (iterations >= 100) break;
            iterations++;
            Transform spawnRef = spawnTransformsCopy[GlobalRandom.RandomInt(this.spawnTransforms.Count - 2, 0)];
            Collider[] collisions = Physics.OverlapSphere(spawnRef.position, 0.4f);
            bool isOccupied = false;
            foreach (Collider collider in collisions)
            {
                Actor check = collider.GetComponent<Actor>();
                if (check != null)
                {
                    isOccupied = true;
                }
            }
            if (isOccupied) continue;

            spawnPositions.Add(spawnRef.position);
            spawnTransformsCopy.Remove(spawnRef);
        }

        return spawnPositions;
    }

    private void OnBossDeath(Vector3 lastPosition)
    {
        Instantiate(this.stairsPrefab, lastPosition, Quaternion.identity);
        MusicManager.Instance.CurrentAudioSource.Stop();
    }
}
