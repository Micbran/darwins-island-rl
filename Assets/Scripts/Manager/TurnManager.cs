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
        Debug.Log(this.actingEntities);
    }
}
