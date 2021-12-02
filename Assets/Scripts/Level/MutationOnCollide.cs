using UnityEngine;

public class MutationOnCollide : MonoBehaviour
{
    [SerializeField] private int mutationAmount = 5;
    [SerializeField] private AudioClip PickupSound;

    private void OnTriggerEnter(Collider other)
    {
        Player checkPlayer = other.GetComponent<Player>();
        if (checkPlayer != null)
        {
            Statistics checkStats = checkPlayer.GetComponent<Statistics>();
            checkStats.IncreaseMutationPoints(this.mutationAmount);
            if (this.PickupSound != null)
            {
                AudioHelper.PlayClip2D(this.PickupSound, 1.0f);
            }
            Destroy(this.gameObject);
        }
    }
}
