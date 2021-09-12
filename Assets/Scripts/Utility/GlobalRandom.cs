using System;

public static class GlobalRandom
{
    private static Random random = new Random();

    public static int RandomInt(int max)
    {
        return random.Next(1, max);
    }

    public static int AttackRoll()
    {
        return random.Next(1, 20);
    }

}
