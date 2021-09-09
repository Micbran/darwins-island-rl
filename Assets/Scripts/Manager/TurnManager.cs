using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : Manager<TurnManager>
{
    private bool playersTurn = true;
    public bool IsPlayersTurn
    {
        get { return this.playersTurn; }
        // DEBUG
        set { this.playersTurn = value; }
    }
    private List<Actor> actedEntities = new List<Actor>();
    private EnergyQueue actingEntities = new EnergyQueue();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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
        if (this.actingEntities.Count == 0)
        {
            this.StartNewGlobalTurn();
        }

        if (this.IsPlayersTurn)
        {
            return;
        }

        Actor acting = this.actingEntities.Dequeue();
        if (acting is Player)
        {
            Debug.Log("Actor is player.");
            this.IsPlayersTurn = true;
        }
        else // filler code for now
        {
            Debug.Log("Actor is not player.");
            acting.EndTurn();
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
}
