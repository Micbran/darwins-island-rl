using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : Entity
{
    [SerializeField] private float moveTime = 0.1f;
    [SerializeField] private LayerMask blockingLayer;

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
        Vector3 end = start + new Vector3(xDir, 0, yDir);

        this.mainCollider.enabled = false;
        Physics.Raycast(start, end, out hit, end.magnitude, this.blockingLayer);
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

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;

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
