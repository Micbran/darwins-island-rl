using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnCollide : MonoBehaviour
{
    [SerializeField] private int healAmount = 5;
    private void OnTriggerEnter(Collider other)
    {
        Player checkPlayer = other.GetComponent<Player>();
        if (checkPlayer != null)
        {
            Statistics checkStats = checkPlayer.GetComponent<Statistics>();
            if (!checkStats.IsFullHealth)
            {
                checkPlayer.GetComponent<Statistics>().Heal(healAmount, "a berry");
                Destroy(this.gameObject);
            }
            else
            {
                LogManager.Instance.AddNewResult(new BasicResult { message = "You're already at full health!" });
            }
            
        }
    }
}
