using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private List<Dice> diceExpression;

    public int RollAttack()
    {
        int damage = 0;
        foreach (Dice dice in diceExpression)
        {
            damage += dice.RollDice();
        }
        return damage;
    }

    public override string ToString()
    {
        string sum = "";
        foreach (Dice dice in diceExpression)
        {
            sum += dice.ToString() + " + ";
        }

        return sum.Substring(0, sum.Length - 3);
    }
}
