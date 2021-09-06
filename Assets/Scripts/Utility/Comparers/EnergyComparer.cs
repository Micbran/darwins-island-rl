using System.Collections.Generic;

public class EnergyComparer : IComparer<Actor>
{
    // Descending order of energy (highest first)
    public int Compare(Actor x, Actor y)
    {
        if (x.Energy < y.Energy)
        {
            return 1;
        }
        if (x.Energy > y.Energy)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
