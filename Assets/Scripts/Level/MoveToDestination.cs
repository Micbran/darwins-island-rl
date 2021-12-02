using UnityEngine;

public class MoveToDestination : MonoBehaviour
{
    public Vector3 destination = Vector3.zero;
    public float speed = 0.5f;

    private Transform baseTransform;

    private void Awake()
    {
        this.baseTransform = this.transform;
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.baseTransform.position, this.destination, this.speed);
        if (Vector3.Distance(this.baseTransform.position, this.destination) <= 0.01f)
        {
            Destroy(this.gameObject);
        }
    }
}
