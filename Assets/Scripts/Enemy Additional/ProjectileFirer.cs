using UnityEngine;

public class ProjectileFirer : MonoBehaviour
{
    [SerializeField] private MoveToDestination projectileToFire = null;

    public void FireProjectile(Vector3 destination)
    {
        MoveToDestination proj = Instantiate(this.projectileToFire, this.transform.position, Quaternion.identity);
        proj.destination = destination;
    }
}
