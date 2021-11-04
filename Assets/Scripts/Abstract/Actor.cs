using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Actor : Entity
{
    [SerializeField] private float moveTime = 0.1f;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask actorLayer;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] public string actorName;

    private readonly float floatConstant = 0.1f;

    public bool IsPlayer
    {
        get;
        private set;
    }

    private BoxCollider mainCollider;
    private Rigidbody rb;
    private float inverseMoveTime;

    protected int currentEnergy;

    public bool isMoving = false;

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
        Vector3 direction = new Vector3(xDir, 0, yDir);
        Vector3 end = start + direction;
        this.mainCollider.enabled = false;
        Physics.Raycast(start, direction, out hit, direction.magnitude + this.floatConstant, this.collisionLayer | this.actorLayer);
        this.mainCollider.enabled = true;

        if (this.isMoving)
        {
            return false;
        }

        if (hit.transform == null)
        {
            StartCoroutine(this.SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        this.isMoving = true;
        this.transform.position = end;
        //float sqrRemainingDistance = (this.transform.position - end).sqrMagnitude;

        //while (sqrRemainingDistance > 1e-13f)
        //{
        //    Vector3 newPosition = Vector3.MoveTowards(this.rb.position, end, inverseMoveTime * Time.deltaTime);
        //    rb.MovePosition(newPosition);
        //    sqrRemainingDistance = (this.transform.position - end).sqrMagnitude;
        //    yield return null;
        //}
        //this.transform.position = end;
        this.isMoving = false;
        yield return null;
    }

    public virtual bool AttemptMove(int xDir, int yDir)
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

    public abstract void OnCantMove(Actor component);

    public abstract void KillActor();

    public virtual void EndTurn()
    {
        DeductEnergy();
    }

    public virtual void NewGlobalTurn()
    {
        AddEnergy();
    }

     protected virtual void DeductEnergy()
    {
        currentEnergy -= 1000;
    }

    protected virtual void AddEnergy()
    {
        currentEnergy += 1000;
    }

    public override string ToString()
    {
        return this.actorName;
    }
}
