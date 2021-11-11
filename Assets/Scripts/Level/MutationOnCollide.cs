using UnityEngine;

public class MutationOnCollide : MonoBehaviour
{
    [SerializeField] private int mutationAmount = 5;
    private void OnTriggerEnter(Collider other)
    {
        Player checkPlayer = other.GetComponent<Player>();
        if (checkPlayer != null)
        {
            Statistics checkStats = checkPlayer.GetComponent<Statistics>();
            checkStats.IncreaseMutationPoints(this.mutationAmount);
            Destroy(this.gameObject);
        }
    }
}
