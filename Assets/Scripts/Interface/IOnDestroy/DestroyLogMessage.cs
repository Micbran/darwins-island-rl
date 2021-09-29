using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLogMessage : MonoBehaviour, IOnDestroy
{
    public void DestroyAction(Actor actor)
    {
        LogManager.Instance.AddNewResult(new DeathResult
        {
            actorName = actor.actorName
        });
    }
}
