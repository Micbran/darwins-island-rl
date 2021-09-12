using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class Player : Actor
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private float inputDelay = 0.2f;
    private float inputDelayTimer = 0f;

    [SerializeField] private List<MeshRenderer> artMeshes;
    [SerializeField] private Material swapMaterial;

    private bool delaying = false;

    private Statistics stats;

    protected override void Awake()
    {
        this.stats = this.GetComponent<Statistics>();
        this.currentHealth = this.maxHealth;
        base.Awake();
    }

    private void Update ()
    {
        inputDelayTimer -= Time.deltaTime;
        if (inputDelayTimer <= 0)
        {
            if (delaying)
            {
                foreach (MeshRenderer mesh in this.artMeshes)
                {
                    Material tempMaterial = mesh.material;
                    mesh.material = this.swapMaterial;
                    this.swapMaterial = tempMaterial;
                }
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
                AttemptMove(horizontal, vertical);
            }
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
            this.AttackEnemy(hitEnemy);
        }
    }

    private void CheckIfGameOver()
    {
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
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
        foreach (MeshRenderer mesh in this.artMeshes)
        {
            Material tempMaterial = mesh.material;
            mesh.material = this.swapMaterial;
            this.swapMaterial = tempMaterial;
        }
        delaying = true;
    }

    public override void KillActor()
    {
        GameManager.Instance.GameOver();
        this.gameObject.SetActive(false);
    }
}
