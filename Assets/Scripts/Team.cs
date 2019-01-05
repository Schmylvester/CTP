using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public int player_id;
    int next_unit_idx = 0;
    int team_size = 5;
    Unit[] units;
    List<SummonedSkeleton> skeletons;

    public Team()
    {
        units = new Unit[team_size];
        skeletons = new List<SummonedSkeleton>();
        for (int i = 0; i < team_size; i++)
        {
            units[i] = null;
        }
    }

    public void addUnit(Unit unit)
    {
        units[next_unit_idx] = unit;
        units[next_unit_idx].setTeam(this);
        next_unit_idx++;
    }

    public void clearUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i] = null;
        }
        skeletons.Clear();
        next_unit_idx = 0;
    }

    public bool stillInGame()
    {
        return countDeadUnits() < team_size;
    }

    public int countDeadUnits()
    {
        int return_count = 0;
        foreach (Unit unit in units)
        {
            if (unit.getHealth() <= 0)
            {
                return_count++;
            }
        }
        return return_count;
    }

    public void buffTeam(float buff, Unit exclude)
    {
        foreach (Unit target in units)
        {
            if (target != exclude)
            {
                for (int i = 1; i < (int)Stat.COUNT; i++)
                {
                    target.changeStat((Stat)i, buff);
                }
            }
        }
    }

    public Unit[] getUnits(bool include_skeletons)
    {
        Unit[] return_units;
        if (include_skeletons)
        {
            return_units = new Unit[team_size + skeletons.Count];
        }
        else
        {
            return_units = new Unit[team_size];
        }
        int i;
        for (i = 0; i < team_size; i++)
        {
            return_units[i] = units[i];
        }
        if (include_skeletons)
        {
            for (; i < return_units.Length; i++)
            {
                return_units[i] = skeletons[i - team_size];
            }
        }

        return return_units;
    }

    public void addSkeletons(int position)
    {
        int slots = 4;
        Unit[] all_units = getUnits(true);
        for (int i = 0; i < all_units.Length; i++)
        {
            if (all_units[i].getPosition() == position)
                slots--;
        }
        for (int i = 0; i < slots; i++)
        {
            skeletons.Add(new SummonedSkeleton());
            skeletons[skeletons.Count - 1].movePosition(position);
        }
    }

    public void compressPositions(int num_cols)
    {
        short[] col_counts = countUnitsCol(num_cols);

        for (int early_col = 0; early_col < num_cols; early_col++)
        {
            for (int populated_col = early_col + 1; populated_col < num_cols; populated_col++)
            {
                if (col_counts[early_col] == 0 && col_counts[populated_col] != 0)
                {
                    foreach (Unit u in units)
                    {
                        if (u.getPosition() == populated_col)
                            u.movePosition(early_col);
                    }
                    col_counts = countUnitsCol(num_cols);
                }
            }
        }
    }

    short[] countUnitsCol(int num_cols)
    {
        short[] col_counts = new short[num_cols];
        for (int i = 0; i < num_cols; i++)
            col_counts[i] = 0;
        foreach (Unit u in units)
        {
            if (u.getHealth() > 0)
                col_counts[u.getPosition()]++;
        }
        return col_counts;
    }
}
