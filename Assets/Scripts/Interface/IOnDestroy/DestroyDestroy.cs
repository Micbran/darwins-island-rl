using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestroy : MonoBehaviour, IOnDestroy
{
    public void DestroyAction(Actor actor)
    {
        Destroy(actor.gameObject);
    }
}
