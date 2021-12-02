using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature.asset", menuName = "Creature Stats")]
public class CreatureStats : ScriptableObject
{
    public int health;
    public int attack;
    public int damage;
    public int defense;
    public int speed;

    public List<Attack> attacks;

    public AudioClip AttackSound;
    public AudioClip MissSound;
    public AudioClip HitSound;
    public AudioClip DeathSound;
    public ParticleSystem HitParticles;
}
