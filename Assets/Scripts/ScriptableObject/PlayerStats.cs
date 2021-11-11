using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats.asset", menuName = "Player Stats")]
public class PlayerStats : CreatureStats
{
    public int currentHealth;

    public int mutationPoints;
    public int mutationLevels;

    public List<Mutation> mutations;
}
