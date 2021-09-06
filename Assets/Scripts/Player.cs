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
            AttemptMove<Enemy>(horizontal, vertical);
            TurnManager.Instance.IsPlayersTurn = false;
        }
    }

    private void OnDisable()
    {
        return;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        EndTurn();
    }

    protected override void OnCantMove<T>(T component)
    {
        Enemy hitEnemy = component as Enemy;
        this.DamageEnemy(hitEnemy);
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
}
