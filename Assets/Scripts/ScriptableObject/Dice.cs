using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice.asset", menuName = "Dice")]
public class Dice : ScriptableObject
{
    [SerializeField] private int number;
    [SerializeField] private int sides;

    public int LastResult { get; private set; }

    public int Sides
    {
        get
        {
            return this.sides;
        }

        set
        {

            this.sides = Mathf.Clamp(value, 1, 100);
        }
    }

    public int Number
    {
        get
        {
            return this.number;
        }

        set
        {
            this.number = Mathf.Clamp(value, 1, 100);
        }
    }

    public Dice(int sides, int number)
    {
        this.sides = sides;
        this.number = number;
    }

    public Dice(string diceExpression)
    {
        diceExpression = diceExpression.ToLower();
        string[] split = diceExpression.Split('d');
        if (split.Length == 2)
        {
            this.number = int.Parse(split[0]);
            this.sides = int.Parse(split[1]);
        }
        else if (split.Length == 1)
        {
            this.number = int.Parse(split[0]);
            this.sides = 1;
        }
        else
        {
            Debug.LogError("Error parsing dice expression: " + diceExpression);
        }
    }

    public int RollDice()
    {
        int sum = 0;
        for (int i = 0; i < this.number; i++)
        {
            sum += GlobalRandom.RandomInt(this.sides);
        }

        this.LastResult = sum;
        return sum;
    }

    public override string ToString()
    {
        if (this.sides > 1)
        {
            return $"{this.number}d{this.sides}";
        }
        else
        {
            return $"{this.number}";
        }
    }
}
