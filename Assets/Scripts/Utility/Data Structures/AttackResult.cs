using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResult
{
    public Attack resultAttack;
    public int attackRollTotal;
    public int defense;
    public int damageRoll;
    public int damageBonus;
    public string resultSource;
    public string resultTarget;

    public override string ToString()
    {
        string attackResult = attackRollTotal < defense ? "misses" : $"deals {damageRoll} ({resultAttack} + {damageBonus}) damage";
        return $"{this.resultSource} attacks {this.resultTarget} ({this.attackRollTotal} vs. {this.defense}) and {attackResult}.";
    }
}
