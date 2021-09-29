using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class Enemy : Actor
{
    private Statistics stats;

    protected override void Awake()
    {
        this.stats = this.GetComponent<Statistics>();
        base.Awake();
    }

    public override void OnCantMove(Actor component)
    {
        Player player = component.GetComponent<Player>();
        if (player)
        {
            this.AttackActor(player);
        }
    }

    private void AttackActor(Actor actor)
    {
        this.stats.MakeAttacks(actor);
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
