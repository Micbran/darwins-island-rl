using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnCollide : MonoBehaviour
{
    [SerializeField] private int healAmount = 5;
    [SerializeField] private AudioClip PickupSound;

    private void OnTriggerEnter(Collider other)
    {
        Player checkPlayer = other.GetComponent<Player>();
        if (checkPlayer != null)
        {
            Statistics checkStats = checkPlayer.GetComponent<Statistics>();
            if (!checkStats.IsFullHealth)
            {
                checkPlayer.GetComponent<Statistics>().Heal(healAmount, "a berry");
                if (this.PickupSound != null)
                {
                    AudioHelper.PlayClip2D(this.PickupSound, 1.0f);
                }
                Destroy(this.gameObject);
            }
            else
            {
                LogManager.Instance.AddNewResult(new BasicResult { message = "You're already at full health!" });
            }
            
        }
    }
}
