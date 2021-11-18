using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class Player : Actor
{
    [SerializeField] private float inputDelay = 0.2f;
    private float inputDelayTimer = 0f;

    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private Color swapColor;

    private bool delaying = false;

    private Statistics stats;

    protected override void Awake()
    {
        this.stats = this.GetComponent<Statistics>();
        base.Awake();
    }

    private void Update ()
    {
        inputDelayTimer -= Time.deltaTime;
        if (inputDelayTimer <= 0)
        {
            if (delaying)
            {
                this.sRenderer.color = Color.white;
                delaying = false;
            }
            if (!TurnManager.Instance.IsPlayersTurn || this.isMoving)
            {
                return;
            }

            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                if (Mathf.Abs(horizontal) == 1 && Mathf.Abs(vertical) == 1)
                {
                    horizontal = 0;
                }
                AttemptMove(horizontal, vertical);
            }
        }
    }

    private void OnDisable()
    {
        return;
    }

    public override bool AttemptMove(int xDir, int yDir)
    {
        bool validMove = base.AttemptMove(xDir, yDir);

        if (validMove)
        {
            EndTurn();
        }

        return validMove;
    }

    public override void OnCantMove(Actor component)
    {
        Enemy hitEnemy = component.GetComponent<Enemy>();
        if (hitEnemy)
        {
            this.AttackEnemy(hitEnemy);
        }
    }

    private void AttackEnemy(Enemy enemy)
    {
        this.stats.MakeAttacks(enemy);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnManager.Instance.EndPlayerTurn();
        inputDelayTimer = this.inputDelay;
        sRenderer.color = this.swapColor;
        delaying = true;
    }

    protected override void AddEnergy()
    {
        this.currentEnergy += 1000 + GlobalRandom.SpeedRandomization(this.stats.Speed);
    }

    public override void KillActor()
    {
        GameManager.Instance.GameOver();
        this.gameObject.SetActive(false);
    }
}
