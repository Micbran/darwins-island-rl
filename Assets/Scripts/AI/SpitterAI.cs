using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(ProjectileFirer))]
public class SpitterAI : MonoBehaviour, IArtificialIntelligence
{
    [SerializeField] private LayerMask collisionLayer = 8;
    // spitting range is hard set to 2

    private Enemy parentEnemy;
    private ProjectileFirer firer;
    private Player playerReference;

    private Vector3 PlayerLocation
    {
        get
        {
            return this.playerReference.transform.position;
        }
    }

    private void Awake()
    {
        this.parentEnemy = this.GetComponent<Enemy>();
        this.firer = this.GetComponent<ProjectileFirer>();
    }

    private void Start()
    {
        this.playerReference = FindObjectOfType<Player>();
    }

    public void Act()
    {
        Vector3 currentPosition = this.transform.position;

        RaycastHit playerSeeker = new RaycastHit();
        Physics.Raycast(currentPosition, this.PlayerLocation - currentPosition, out playerSeeker, Vector3.Distance(this.PlayerLocation, currentPosition), this.collisionLayer);
        if (playerSeeker.transform != null) // player not in "sight"
        {
            this.parentEnemy.EndTurn(); // Pass turn
            return;
        }

        List<CollisionInformation> directions = new List<CollisionInformation>();
        // left, right, forward, back
        directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.left, 0.4f), currentPosition, Vector3.left));
        directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.right, 0.4f), currentPosition, Vector3.right));
        directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.forward, 0.4f), currentPosition, Vector3.forward));
        directions.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.back, 0.4f), currentPosition, Vector3.back));

        List<CollisionInformation> attackDirections = new List<CollisionInformation>(directions);
        // left-left, right-right, forward-left, forward-right, back-left, back-right, forward-forward, back-back
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.left + Vector3.left, 0.4f), currentPosition, Vector3.left + Vector3.left));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.right + Vector3.right, 0.4f), currentPosition, Vector3.right + Vector3.right));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.forward + Vector3.left, 0.4f), currentPosition, Vector3.forward + Vector3.left));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.forward + Vector3.right, 0.4f), currentPosition, Vector3.forward + Vector3.right));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.back + Vector3.left, 0.4f), currentPosition, Vector3.back + Vector3.left));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.back + Vector3.right, 0.4f), currentPosition, Vector3.back + Vector3.right));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.forward + Vector3.forward, 0.4f), currentPosition, Vector3.forward + Vector3.forward));
        attackDirections.Add(new CollisionInformation(Physics.OverlapSphere(currentPosition + Vector3.back + Vector3.back, 0.4f), currentPosition, Vector3.back + Vector3.back));

        foreach (CollisionInformation collisionInfo in attackDirections)
        {
            foreach (Collider collider in collisionInfo)
            {
                Player playerCheck = collider.GetComponent<Player>();
                if (playerCheck != null)
                {
                    // Attack player
                    this.parentEnemy.OnCantMove(playerCheck);
                    this.firer.FireProjectile(playerCheck.transform.position);
                    this.parentEnemy.EndTurn();
                    return;
                }
            }
        }

        directions.Sort(
            (dir1, dir2) => Vector3.Distance(this.PlayerLocation, dir1.Center).CompareTo(Vector3.Distance(this.PlayerLocation, dir2.Center))
            );

        foreach (CollisionInformation collisionInfo in directions)
        {
            bool skipToNext = false;
            foreach (Collision collision in collisionInfo)
            {
                Actor checkEnemy = collision.gameObject.GetComponent<Actor>();
                if (checkEnemy != null) // square is occupied
                {
                    skipToNext = true;
                }
            }
            if (skipToNext)
            {
                continue;
            }
            bool didMove = this.parentEnemy.AttemptMove((int)collisionInfo.offset.x, (int)collisionInfo.offset.z);
            if (didMove)
            {
                this.parentEnemy.EndTurn();
                return;
            }
        }

        // no valid moves
        this.parentEnemy.EndTurn();
    }
}
