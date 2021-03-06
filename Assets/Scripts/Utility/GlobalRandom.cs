using System;
using UnityEngine;

public static class GlobalRandom
{
    private static System.Random random = new System.Random();

    private static int speedOffsetToZero = 20;
    private static (int, int)[] speedArray =
    {
        (-302, -360),         // -20, 0
        (-300, -355),         // -19, 1
        (-298, -350),         // -18, 2
        (-295, -345),         // -17, 3
        (-293, -340),         // -16, 4
        (-290, -335),         // -15, 5
        (-285, -325),         // -14, 6
        (-280, -310),         // -13, 7
        (-270, -290),         // -12, 8
        (-260, -280),         // -11, 9
        (-250, -270),         // -10, 10
        (-220, -250),         // -9, 11
        (-190, -220),         // -8, 12
        (-160, -190),         // -7, 13
        (-130, -160),         // -6, 14
        (-110, -130),         // -5, 15
        (-90, -110),         // -4, 16
        (-70, -90),         // -3, 17
        (-60, -70),         // -2, 18
        (-50, -60),         // -1, 19
        (0, 0),         // 0, 20
        (90, 160),      // 1
        (160, 230),     // 2
        (230, 300),     // 3
        (300, 360),     // 4
        (360, 420),     // 5
        (420, 480),     // 6
        (480, 530),     // 7
        (530, 580),     // 8
        (580, 630),     // 9
        (630, 670),     // 10
        (670, 710),     // 11
        (710, 740),     // 12
        (780, 820),     // 13
        (820, 860),     // 14
        (860, 890),     // 15
        (890, 920),     // 16
        (920, 950),     // 17
        (950, 980),     // 18
        (980, 1000),     // 19
        (1000, 1020),     // 20
        (1020, 1040),     // 21
        (1040, 1060),     // 22
        (1060, 1080),     // 23
        (1080, 1100),     // 24
        (1100, 1120),     // 25
        (1120, 1140),     // 26
        (1140, 1160),     // 27
        (1160, 1175),     // 28
        (1175, 1190),     // 29
        (1190, 1205),     // 30
        (1205, 1220),     // 31
        (1220, 1235),     // 32
        (1235, 1250),     // 33
        (1250, 1265),     // 34
        (1265, 1280),     // 35
        (1280, 1290),     // 36
        (1280, 1300),     // 37
        (1280, 1310),     // 38
        (1280, 1320),     // 39
        (1300, 1330),     // 40


    };

    public static int RandomInt(int max, int min = 1)
    {
        return random.Next(min, max + 1);
    }

    public static int RandomOddInt(int max)
    {
        int randOdd = random.Next(1, max + 1);
        if (randOdd % 2 == 0)
        {
            randOdd -= 1;
        }
        return randOdd;
    }

    public static int RandomOddInt(int min, int max)
    {
        int randOdd = random.Next(min, max + 1);
        if (randOdd % 2 == 0)
        {
            randOdd -= 1;
        }
        if (randOdd < min)
        {
            randOdd += 2;
        }
        return randOdd;
    }

    public static int AttackRoll()
    {
        return random.Next(1, 21);
    }

    public static int SpeedRandomization(int speed)
    {
        var speedTuple = speedArray[Mathf.Clamp(speed + speedOffsetToZero, 0, speedArray.Length - 1)];
        return random.Next(speedTuple.Item1, speedTuple.Item2);
    }

}
