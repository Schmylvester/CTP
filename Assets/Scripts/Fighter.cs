using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    ATTACK,
    DEFENCE,
    SPEED,
    Coolness,

    Count
}

public class Fighter
{
    int max_health = 50;
    int health;
    int[] tracked_stats;

    public Fighter(int[] new_stats)
    {
        tracked_stats = new int[(int)Stat.Count];
        health = max_health;
        for (int i = 0; i < (int)Stat.Count; i++)
        {
            tracked_stats[i] = new_stats[i];
        }
    }

    public void dealDamage(Fighter target)
    {
        if (getStat(Stat.SPEED) > target.getStat(Stat.SPEED))
        {
            target.takeDamage(getStat(Stat.ATTACK) * 3);
        }
        else
        {
            target.takeDamage(getStat(Stat.ATTACK) * 2);
        }
    }

    public int takeDamage(int damage)
    {
        damage -= getStat(Stat.Coolness) * 2;
        damage -= getStat(Stat.DEFENCE);
        //always 1 damage at least
        damage = damage > 1 ? damage : 1;
        health -= damage;
        return damage;
    }

    public void mutate()
    {
        int decrease = 2;
        int increase = 3;

        int dec_stat = Random.Range(0, (int)Stat.Count);
        int inc_stat = Random.Range(0, (int)Stat.Count);

        if (tracked_stats[dec_stat] > decrease && inc_stat != dec_stat)
        {
            tracked_stats[inc_stat] += increase;
            tracked_stats[dec_stat] -= decrease;
        }

        normaliseStats();
    }

    void normaliseStats()
    {
        int stat_sum = 0;
        foreach (int i in tracked_stats)
        {
            stat_sum += i;
        }
        for (int i = stat_sum; i > 100; i--)
        {
            int r = Random.Range(0, (int)Stat.Count);
            if (tracked_stats[r] > 1)
                tracked_stats[r]--;
        }
    }

    #region ConstantFunctions
    //functions below shouldn't ever need to change
    public int getStat(Stat stat)
    {
        return tracked_stats[(int)stat];
    }
    public bool getAlive()
    {
        return health > 0;
    }
    public void heal(int by = -1)
    {
        health += by;
        if (by == -1 || health > max_health)
        {
            health = max_health;
        }
    }
    #endregion
}