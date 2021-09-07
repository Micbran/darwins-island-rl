using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    protected override void Awake()
    {
        currentHealth = maxHealth;
        base.Awake();
    }

    private void Update ()
    {
        if (!TurnManager.Instance.IsPlayersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove(horizontal, vertical);
        }
    }

    private void OnDisable()
    {
        return;
    }

    protected override bool AttemptMove(int xDir, int yDir)
    {
        bool validMove = base.AttemptMove(xDir, yDir);

        if (validMove)
        {
            EndTurn();
        }

        return validMove;
    }

    protected override void OnCantMove(Actor component)
    {
        Enemy hitEnemy = component.GetComponent<Enemy>();
        if (hitEnemy)
        {
            this.DamageEnemy(hitEnemy);
        }
    }

    private void CheckIfGameOver()
    {
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void DamageEnemy(Enemy enemy)
    {
        return;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnManager.Instance.EndPlayerTurn();
        Debug.Log("Player ended turn.");
    }
}
