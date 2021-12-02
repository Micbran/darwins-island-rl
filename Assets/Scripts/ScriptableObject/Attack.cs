using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType
{
    Single = 0,
    PBAoeRange1 = 1
}

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private List<Dice> diceExpression;
    [SerializeField] private string diceExpressionString;
    [SerializeField] private bool benefitsFromDamage = true;
    [SerializeField] private AttackType type = AttackType.Single;

    public bool BenefitsFromDamage => this.benefitsFromDamage;
    public AttackType AttackType => this.type;

    private void Awake()
    {
        if (this.diceExpression.Count != 0) return;
        if (this.diceExpressionString.Length == 0) return;
        
        string[] dice = diceExpressionString.Split('+').Select(d => d == null? null: d.Trim()).ToArray();
        foreach (string die in dice)
        {
            this.diceExpression.Add(new Dice(die));
        }

        Debug.Log(this.ToString());
    }

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
        if (sum.Length < 3) return sum;
        return sum.Substring(0, sum.Length - 3);
    }
}
