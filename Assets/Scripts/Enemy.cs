using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    protected override void OnCantMove(Actor component)
    {
        throw new System.NotImplementedException();
    }

    public override void KillActor()
    {
        IOnDestroy[] destroyActions = this.GetComponents<IOnDestroy>();
        foreach (IOnDestroy destruction in destroyActions)
        {
            destruction.DestroyAction(this);
        }
    }
}
