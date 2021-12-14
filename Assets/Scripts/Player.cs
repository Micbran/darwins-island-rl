using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class Player : Actor
{
    [SerializeField] private float inputDelay = 0.1f;
    private float inputDelayTimer = 0f;

    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private Color swapColor;

    private Statistics stats;

    protected override void Awake()
    {
        this.stats = this.GetComponent<Statistics>();
        base.Awake();
    }

    private void Update ()
    {
        this.inputDelayTimer -= Time.deltaTime; // there's like, theoretically an underflow bug with this
        if (this.inputDelayTimer > 0) return;
        if (!TurnManager.Instance.IsPlayersTurn || this.isMoving)
        {
            return;
        }

        this.sRenderer.color = Color.white;

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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            LogManager.Instance.AddNewResult(new BasicResult() { message = $"{this.actorName} waits." });
            this.EndTurn();
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
            this.EndTurn();
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
        this.inputDelayTimer = this.inputDelay;
        sRenderer.color = this.swapColor;
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

    public void DebugHealFull()
    {
        this.stats.DebugHealFull();
    }

    public void DebugGiveMutationPoint()
    {
        this.stats.DebugGiveMutationPoint();
    }
}
