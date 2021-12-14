using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : LevelManager<TurnManager>
{
    [SerializeField] private bool playersTurn = false;
    public bool IsPlayersTurn
    {
        get { return this.playersTurn; }
        // DEBUG
        set { this.playersTurn = value; }
    }
    private List<Actor> actedEntities = new List<Actor>();
    private EnergyQueue actingEntities = new EnergyQueue();
    private Actor acting = null;
    private float moveTimerMax = 0.0002f;
    private float moveTimer = 0;

    public void OnGenerationComplete()
    {
        actedEntities.Clear();
        actingEntities.Clear();

        Actor[] interimList = FindObjectsOfType<Actor>();
        foreach (Actor a in interimList)
        {
            this.actingEntities.Enqueue(a);
        }
    }

    private void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer > 0) return;

        moveTimer = moveTimerMax;
        if (this.actingEntities.Count == 0 && !this.IsPlayersTurn)
        {
            this.StartNewGlobalTurn();
        }

        if (this.IsPlayersTurn)
        {
            return;
        }

        acting = this.actingEntities.Dequeue();
        if (acting == null)
        {
            return;
        }
        if (acting is Player)
        {
            this.IsPlayersTurn = true;
        }
        else
        {
            IArtificialIntelligence ai = acting.GetComponent<IArtificialIntelligence>();
            if (ai != null)
            {
                ai.Act();
            }
            else
            {
                Debug.Log(acting.actorName + " does not have an AI component.");
                acting.EndTurn();
            }

            if (acting.Energy <= 0)
            {
                this.actedEntities.Add(acting);
            }
            else
            {
                this.actingEntities.Enqueue(acting);
            }
        }
    }

    private void StartNewGlobalTurn()
    {
        int actedSize = this.actedEntities.Count;
        // ghetto pop method
        for (int i = 0; i < actedSize; i++)
        {
            Actor acted = this.actedEntities[0];
            this.actedEntities.RemoveAt(0);
            acted.NewGlobalTurn();
            if (acted.Energy > 0)
            {
                this.actingEntities.Enqueue(acted);
            }
            else
            {
                this.actedEntities.Add(acted);
            }
        }
    }

    public void EndPlayerTurn()
    {
        this.IsPlayersTurn = false;
        moveTimer = moveTimerMax;
        Actor playerRef = FindObjectOfType<Player>();
        if (playerRef.Energy <= 0)
        {
            this.actedEntities.Add(playerRef);
        }
        else
        {
            this.actingEntities.Enqueue(playerRef);
        }
    }

    public void AddActorToTurnOrder(Actor newActor)
    {
        this.actingEntities.Enqueue(newActor);
    }
}
