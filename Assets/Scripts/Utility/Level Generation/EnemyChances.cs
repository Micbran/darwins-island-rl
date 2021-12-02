using UnityEngine;

public class EnemyChances
{
    private static (int, int, int, int)[] EnemyByLevel =
    {
        // lizard, bee, spider, acid
        (70, 30, 00, 00), // 1
        (50, 40, 00, 10), // 2
        (40, 40, 00, 20), // 3
        (40, 30, 10, 20), // 4
        (30, 30, 15, 25), // 5
        (30, 30, 20, 20), // 6
        (30, 30, 20, 20), // 7
        (30, 30, 20, 20), // 8
        (30, 30, 20, 20), // 9
        (30, 30, 20, 20), // 10
        (30, 30, 20, 20), // 11
        (30, 30, 20, 20), // 12
        (30, 30, 20, 20), // 13
        (30, 30, 20, 20), // 14
        (30, 30, 20, 20), // 15
        (30, 30, 20, 20), // 16
        (30, 30, 20, 20), // 17
        (30, 30, 20, 20), // 18
        (30, 30, 20, 20), // 19
        (30, 30, 20, 20), // 20
    };
    private int lizardChance;
    public int Lizard => this.lizardChance;

    private int beeChance;
    public int Bee => this.beeChance;

    private int spiderChance;
    public int Spider => this.spiderChance;

    private int acidChance;
    public int AcidSpitter => this.acidChance;

    public EnemyChances(int floor)
    {
        this.SetDefaultValues(floor);
    }

    private void SetDefaultValues(int floor)
    {
        (int, int, int, int) chances = EnemyByLevel[Mathf.Clamp(floor - 1, 0, EnemyByLevel.Length - 1)];
        this.lizardChance = chances.Item1;
        this.beeChance = chances.Item2;
        this.spiderChance = chances.Item3;
        this.acidChance = chances.Item4;
    }
}
