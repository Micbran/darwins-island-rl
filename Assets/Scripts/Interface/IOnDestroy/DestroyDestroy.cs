using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestroy : IOnDestroy
{
    public override void DestroyAction(Actor actor)
    {
        Destroy(actor.gameObject);
    }
}
