using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : Entity
{
    [SerializeField] private float moveTime = 0.1f;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private bool isPlayer = false;
    public bool IsPlayer
    {
        get;
        private set;
    }

    private BoxCollider mainCollider;
    private Rigidbody rb;
    private float inverseMoveTime;

    private int currentEnergy;

    public int Energy
    {
        get { return this.currentEnergy; }
    }
    
    protected virtual void Awake()
    {
        this.mainCollider = this.GetComponent<BoxCollider>();
        this.rb = GetComponent<Rigidbody>();

        this.inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit hit)
    {
        Vector3 start = this.transform.position;
        Vector3 length = new Vector3(xDir, 0, yDir);
        Vector3 end = start + new Vector3(xDir, 0, yDir);
        Vector3 direction = end - start;

        this.mainCollider.enabled = false;
        Physics.Raycast(start, direction, out hit, length.magnitude, this.blockingLayer);
        this.mainCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(this.SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (this.transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > 1e-13f)
        {
            Vector3 newPosition = Vector3.MoveTowards(this.rb.position, end, inverseMoveTime * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrRemainingDistance = (this.transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual bool AttemptMove(int xDir, int yDir)
    {
        RaycastHit hit;
        bool canMove = Move(xDir, yDir, out hit);

        // empty space
        if (hit.transform == null)
        {
            return true;
        }

        Actor hitComponent = hit.transform.GetComponent<Actor>();

        // Actor in the way
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected abstract void OnCantMove(Actor component);

    public virtual void EndTurn()
    {
        DeductEnergy();
    }

     private void DeductEnergy()
    {
        currentEnergy -= 1000;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
