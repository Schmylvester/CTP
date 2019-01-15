using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Attack, Defence, Speed,

    Count
}

public class Unit
{
    Vector2Int position;
    
    int max_total_stats = 40;
    int[] tracked_stats;

    public Unit(int[] new_stats)
    {
        tracked_stats = new int[(int)Stat.Count];
        for (int i = 0; i < (int)Stat.Count; i++)
        {
            tracked_stats[i] = new_stats[i];
        }
    }

    public Unit attack(Unit target)
    {
        int my_stats = getStat(Stat.Attack) + getStat(Stat.Defence) + getStat(Stat.Speed);
        int their_stats = target.getStat(Stat.Attack) + target.getStat(Stat.Defence) + target.getStat(Stat.Speed);

        if (my_stats > their_stats)
            return this;
        return target;
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
        for (int i = stat_sum; i > max_total_stats; i--)
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
    #endregion
}