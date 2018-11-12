using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Mage,
    Rogue,
    Warrior,

    UNIT_COUNT
}


public class UnitStats : MonoBehaviour
{
    [SerializeField] CreateGrid create_grid;
    [SerializeField] Sprite[] sprites;
    List<List<int>> all_unit_stats;

    private void Start()
    {
        all_unit_stats = new List<List<int>>();
        for(int i = 0; i < (int)Class.UNIT_COUNT; i++)
        {
            all_unit_stats.Add(new List<int>());
            for(int j = 0; j < (int)Stat.Count; j++)
            {
                all_unit_stats[i].Add(0);
            }
        }

        all_unit_stats[(int)Class.Warrior][(int)Stat.SIGHT] = 3;

        all_unit_stats[(int)Class.Rogue][(int)Stat.SIGHT] = 4;

        all_unit_stats[(int)Class.Mage][(int)Stat.SIGHT] = 3;

        create_grid.loadGrid();
    }

    public void getStats(Class unit, ref List<int> stats)
    {
        for(int i = 0; i < (int)Stat.Count; i++)
        {
            stats.Add(all_unit_stats[(int)unit][i]);
        }
    }

    public Sprite getSprite(Class type)
    {
        return sprites[(int)type];
    }
}
