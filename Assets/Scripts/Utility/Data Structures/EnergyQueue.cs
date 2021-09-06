using System.Collections.Generic;

public class EnergyQueue
{
    private List<Actor> actorQueue;
    private EnergyComparer energyComparer;

    public int Count
    {
        get { return this.actorQueue.Count; }
    }

    public EnergyQueue()
    {
        this.actorQueue = new List<Actor>();
        energyComparer = new EnergyComparer();
    }

    public void Enqueue(Actor newActor)
    {
        this.actorQueue.Add(newActor);
        this.Readjust();
    }

    public Actor Dequeue()
    {
        if (this.actorQueue.Count == 0)
        {
            return null;
        }

        Actor saveActor = this.actorQueue[0];
        this.actorQueue.RemoveAt(0);

        return saveActor;
    }

    public void Clear()
    {
        this.actorQueue.Clear();
    }

    private void Readjust()
    {
        this.actorQueue.Sort(this.energyComparer);
    }

    public override string ToString()
    {
        return string.Join(", ", this.actorQueue);
    }
}
