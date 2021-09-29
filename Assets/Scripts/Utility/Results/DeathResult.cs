using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathResult : Result
{
    public string actorName;

    public override string ToResultString()
    {
        return $"The {this.actorName} is slain!";
    }
}
