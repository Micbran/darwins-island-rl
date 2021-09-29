using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicResult : Result
{
    public string message;

    public override string ToResultString()
    {
        return message;
    }
}
