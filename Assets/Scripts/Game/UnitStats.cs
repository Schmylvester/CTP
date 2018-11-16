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
    [SerializeField] GridManager create_grid;
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

        //this but a json file
        initStats();

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

    void initStats()
    {
        int unit_class = (int)Class.Warrior;
        all_unit_stats[unit_class][(int)Stat.ACCURACY]      = 3;
        all_unit_stats[unit_class][(int)Stat.AGILITY]       = 2;
        all_unit_stats[unit_class][(int)Stat.ATTACK]        = 4;
        all_unit_stats[unit_class][(int)Stat.ATTACK_RANGE]  = 1;
        all_unit_stats[unit_class][(int)Stat.CHARISMA]      = 2;
        all_unit_stats[unit_class][(int)Stat.DEFENCE]       = 4;
        all_unit_stats[unit_class][(int)Stat.DODGE]         = 2;
        all_unit_stats[unit_class][(int)Stat.INTELLIGENCE]  = 2;
        all_unit_stats[unit_class][(int)Stat.MOVE]          = 2;
        all_unit_stats[unit_class][(int)Stat.SIGHT]         = 3;

        unit_class = (int)Class.Rogue;
        all_unit_stats[unit_class][(int)Stat.ACCURACY]      = 4;
        all_unit_stats[unit_class][(int)Stat.AGILITY]       = 4;
        all_unit_stats[unit_class][(int)Stat.ATTACK]        = 2;
        all_unit_stats[unit_class][(int)Stat.ATTACK_RANGE]  = 2;
        all_unit_stats[unit_class][(int)Stat.CHARISMA]      = 5;
        all_unit_stats[unit_class][(int)Stat.DEFENCE]       = 1;
        all_unit_stats[unit_class][(int)Stat.DODGE]         = 3;
        all_unit_stats[unit_class][(int)Stat.INTELLIGENCE]  = 3;
        all_unit_stats[unit_class][(int)Stat.MOVE]          = 3;
        all_unit_stats[unit_class][(int)Stat.SIGHT]         = 4;

        unit_class = (int)Class.Mage;
        all_unit_stats[unit_class][(int)Stat.ACCURACY]      = 3;
        all_unit_stats[unit_class][(int)Stat.AGILITY]       = 3;
        all_unit_stats[unit_class][(int)Stat.ATTACK]        = 3;
        all_unit_stats[unit_class][(int)Stat.ATTACK_RANGE]  = 2;
        all_unit_stats[unit_class][(int)Stat.CHARISMA]      = 2;
        all_unit_stats[unit_class][(int)Stat.DEFENCE]       = 1;
        all_unit_stats[unit_class][(int)Stat.DODGE]         = 2;
        all_unit_stats[unit_class][(int)Stat.INTELLIGENCE]  = 4;
        all_unit_stats[unit_class][(int)Stat.MOVE]          = 2;
        all_unit_stats[unit_class][(int)Stat.SIGHT]         = 3;
    }
}
