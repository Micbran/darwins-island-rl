using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnEnter : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    private bool ignoreNext = false;
    private void OnTriggerEnter(Collider other)
    {
        if (ignoreNext) 
        {
            ignoreNext = false;
            return;
        }
        Player player = other.GetComponent<Player>();
        if (!player)
        {
            return;
        }
        Statistics stats = other.GetComponent<Statistics>();
        if (stats)
        {
            stats.TakeDamage(this.damageAmount);
            ignoreNext = true;
        }
    }
}
