using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mutation.asset", menuName = "Mutation")]
public class Mutation : ScriptableObject
{
    public int Id;
    public string mutationName;
    public int healthChange;
    public int defenseChange;
    public int attackChange;
    public int damageChange;
    public int speedChange;

    public Attack bonusAttack;

    [TextArea(15, 20)]
    public string description;
}


