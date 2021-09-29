using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInformation : IEnumerator, IEnumerable
{
    public Collider[] colliders;
    public Vector3 Center
    {
        get
        {
            return baseLocation + offset;
        }
    }
    public Vector3 baseLocation;
    public Vector3 offset;

    // Enumerator
    private int position = -1;

    public CollisionInformation(Collider[] colliders, Vector3 baseLocation, Vector3 offset)
    {
        this.colliders = colliders;
        this.baseLocation = baseLocation;
        this.offset = offset;
    }

    public object Current
    {
        get { return colliders[position]; }
    }

    public bool MoveNext()
    {
        position++;
        return position < colliders.Length;
    }

    public void Reset()
    {
        position = 0;
    }

    public IEnumerator GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
