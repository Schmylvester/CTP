using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    List<List<int>> all_unit_stats;

    private void Start()
    {
        all_unit_stats = new List<List<int>>();
        for(int i = 0; i < (int)Class.UNIT_COUNT; i++)
        {
            all_unit_stats.Add(new List<int>());
        }

        all_unit_stats[(int)Class.Warrior][(int)Stat.SIGHT] = 3;

        all_unit_stats[(int)Class.Rogue][(int)Stat.SIGHT] = 4;

        all_unit_stats[(int)Class.Mage][(int)Stat.SIGHT] = 3;
    }

    public int getStat(Class unit, Stat stat)
    {
        return all_unit_stats[(int)unit][(int)stat];
    }
}
