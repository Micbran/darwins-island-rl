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
        Actor actor = other.GetComponent<Actor>();
        if (!actor)
        {
            return;
        }
        Statistics stats = other.GetComponent<Statistics>();
        if (stats)
        {
            stats.TakeDamage(this.damageAmount);
            LogManager.Instance.AddNewResult(new BasicResult() { message = $"{actor.actorName} takes {this.damageAmount} damage from lava!" });
            ignoreNext = true;
        }
    }
}
