using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public short player_id;
    int next_unit_idx = 0;
    int team_size = 5;
    Unit[] units;
    List<SummonedSkeleton> skeletons;
    PlayerGrid grid;

    public Team()
    {
        units = new Unit[team_size];
        skeletons = new List<SummonedSkeleton>();
        for (int i = 0; i < team_size; i++)
        {
            units[i] = null;
        }
    }

    public void setGridAndField(PlayerGrid _grid, Field field)
    {
        grid = _grid;
        foreach (Unit u in units)
            u.setGridAndField(grid, field);
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

    public int addSkeletons(int position)
    {
        int skeletons_summoned = 0;
        for (int i = 0; i < 4; i++)
        {
            SummonedSkeleton skeleton = new SummonedSkeleton();
            if (grid.addUnitToCol((ushort)position, skeleton))
            {
                skeleton.setTeam(this);
                skeleton.setGridAndField(grid, null);
                skeletons.Add(skeleton);
                skeletons_summoned++;
            }
            else
            {
                grid.updateSprites();
                return skeletons_summoned;
            }
        }
        return skeletons_summoned;
    }

    public void removeSkeleton(SummonedSkeleton skeleton)
    {
        skeletons.Remove(skeleton);
        grid.updateGrid();
    }

    public bool colEmpty(short col)
    {
        foreach (Unit u in units)
        {
            if (u.grid_pos.x == col && u.getHealth() > 0)
                return false;
        }
        foreach(SummonedSkeleton u in skeletons)
        {
            if (u.grid_pos.x == col && u.getHealth() > 0)
                return false;
        }
        return true;
    }
}
