using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Max_HP,
    Attack,
    Defence,
    Intelligence,
    Accuracy,
    Agility,
    Speed,

    COUNT
}
public enum SingleTurnEffects
{
    Ambushed = 1,
    Charmed = 2,
    SwanSong = 4,
    CheatingDeath = 8,
    DownWithShip = 16,
}

public abstract class Unit
{
    protected int current_health;
    protected int[] base_stats;
    protected int[] temp_stats;
    protected Field field;
    protected PlayerGrid grid;
    public Vector2Int grid_pos;
    protected List<Ability> abilities;
    protected Team team;
    public SetTurnActions actions;
    public bool berserk = false;
    int effects = 0;
    public Unit taunted_by = null;
    public Unit defended_by = null;

    protected abstract void setAbilities();
    protected abstract void setStats();
    public abstract string getName();

    public Unit()
    {
        base_stats = new int[(int)Stat.COUNT];
        temp_stats = new int[(int)Stat.COUNT];
        setStats();
        base_stats[(int)Stat.Max_HP] = 300 + (base_stats[(int)Stat.Max_HP] * 3);
        resetStats();
        current_health = getStat(Stat.Max_HP);
        abilities = new List<Ability>();
        setAbilities();

        foreach (Ability a in abilities)
        {
            a.setUser(this);
        }
    }

    public void setGridAndField(PlayerGrid _grid, Field _field)
    {
        grid = _grid;
        field = _field;
        for (int i = 0; i < abilities.Count; i++)
            abilities[i].setField(field);
    }

    public int attack(Unit target, ActionFeedbackText feedback, bool distance_penalty = true)
    {
        if (taunted_by != null)
        {
            resetStats();
            target = taunted_by;
            taunted_by = null;
            return attack(target, feedback);
        }

        float attack_miss = 0.6f;
        //reduce miss chance by 1.5% for each of my accuracy stat points
        attack_miss *= Mathf.Pow(0.985f, getStat(Stat.Accuracy));
        //increase miss chance by 0.5% for each of my target's agility points
        attack_miss *= Mathf.Pow(1.005f, target.getStat(Stat.Agility));

        if (distance_penalty)
        {
            //for each space I am back, add 20% to the attack miss
            attack_miss *= Mathf.Pow(1.2f, getPos());
            //for each space the target is back, add 60% to the attack miss
            attack_miss *= Mathf.Pow(1.6f, target.getPos());
        }
        attack_miss = Mathf.Min(attack_miss, 0.96f);
        if (Random.Range(0.0f, 1.0f) > attack_miss || this == target)
        {
            if ((this as Rogue) != null)
            {
                int sneak = (this as Rogue).sneak_turns;
                if (sneak > 1)
                    modifyStat(Stat.Attack, (float)sneak);
            }
            int base_damage = 0;
            for (int i = 0; i < getStat(Stat.Attack); i++)
            {
                base_damage += Random.Range(2, 5);
            }
            int dam = target.takeDamage(base_damage, feedback);
            return dam;
        }
        else
            feedback.printMessage(getName() + "'s attack had a " + ((int)((1 - attack_miss) * 100)).ToString() + "% chance of hitting " + target.getName() + " but it missed.");
        return 0;
    }


    public int takeDamage(int damage, ActionFeedbackText feedback)
    {
        if (defended_by != null)
        {
            feedback.printMessage("Attack blocked by " + defended_by.getName());
            return defended_by.takeDamage(damage, feedback);
        }
        if ((this as Rogue) != null)
            (this as Rogue).sneak_turns = 0;
        int damage_taken = damage;
        for (int i = 0; i < getStat(Stat.Defence); i++)
        {
            damage_taken -= Random.Range(1, 4);
        }
        //if the attack hits, at least 15 damage
        damage_taken = Mathf.Max(15, damage_taken);
        changeHealth(-damage_taken, feedback);
        if (current_health <= 0)
        {
            die(feedback);
        }
        return damage_taken;
    }

    public void advanceTurn()
    {
        resetStats();
        taunted_by = null;
        defended_by = null;
        effects = 0;
    }

    public void setTeam(Team _team)
    {
        team = _team;
    }

    public int getHealth()
    {
        return current_health;
    }

    public int changeHealth(int change, ActionFeedbackText feedback)
    {
        if (current_health == 0)
        {
            grid.getSprite(this).color = Color.white;
            grid.getSprite(this).flipY = false;
        }
        int start_health = current_health;
        current_health += change;
        current_health = Mathf.Min(current_health, getStat(Stat.Max_HP));
        current_health = Mathf.Max(current_health, 0);
        if (current_health <= 0)
            die(feedback);
        return current_health - start_health;
    }

    public int getStat(Stat stat)
    {
        int ret = temp_stats[(int)stat];
        return ret;
    }

    public void changeStat(Stat stat, float change)
    {
        base_stats[(int)stat] = (int)(base_stats[(int)stat] * change);
        resetStat(stat);
    }
    public void changeStat(Stat stat, int change)
    {
        base_stats[(int)stat] += change;
        resetStat(stat);
    }

    public void modifyStat(Stat stat, float mod)
    {
        temp_stats[(int)stat] = (int)(temp_stats[(int)stat] * mod);
    }
    public void modifyStat(Stat stat, int mod)
    {
        temp_stats[(int)stat] += mod;
    }

    protected void resetStat(Stat stat)
    {
        temp_stats[(int)stat] = base_stats[(int)stat];
    }

    public void resetStats()
    {
        for (int i = 0; i < (int)Stat.COUNT; i++)
        {
            resetStat((Stat)i);
        }
    }

    public Team getTeam()
    {
        return team;
    }

    public void setActiveEffect(SingleTurnEffects effect)
    {
        if (!getEffectActive(effect))
            effects += (int)effect;
    }

    public bool getEffectActive(SingleTurnEffects effect)
    {
        return (effects / (int)effect) % 2 == 1;
    }

    public Ability getAbility(int index)
    {
        return abilities[index];
    }

    protected bool validateStats(int exp_total)
    {
        int total = 0;
        for (int i = 0; i < (int)Stat.COUNT; i++)
        {
            total += base_stats[i];
        }

        if (total < exp_total)
        {
            Debug.LogError(getName() + " is underpowered by " + (exp_total - total) + " point(s).");
            return false;
        }
        if (total > exp_total)
        {
            Debug.LogError(getName() + " is overpowered by " + (total - exp_total) + " point(s).");
            return false;
        }
        return true;
    }

    public virtual void die(ActionFeedbackText feedback)
    {
        if (getEffectActive(SingleTurnEffects.CheatingDeath))
        {
            if (Random.Range(0.0f, 1.0f) < 0.8f)
            {
               feedback.printMessage(getName() + " cheated death.");
                current_health = 1;
                return;
            }
        }
        if (getEffectActive(SingleTurnEffects.DownWithShip))
        {
            feedback.printMessage(getName() + " is taking you with them.");
            foreach (Unit u in field.getTeam(1 - team.player_id).getUnits(true))
            {
                attack(u, feedback);
            }
        }

        if (getEffectActive(SingleTurnEffects.SwanSong))
        {
            feedback.printMessage(getName() + " was singing a really nice song, so now their team all get two actions next turn.");
            actions.swan_active = true;
        }

        feedback.printMessage(getName() + " died.");
        grid.getSprite(this).color = Color.white - new Color(0, 0, 0, 0.5f);
        grid.getSprite(this).flipY = true;
        if(getName() == "Skeleton" && Random.Range(0,2) == 0)
        {
            team.removeSkeleton((SummonedSkeleton)this);
        }
    }

    public bool getTaunt()
    {
        return taunted_by != null;
    }

    public bool moveValid(char dir, ActionFeedbackText feedback)
    {
        short target_col = (short)grid_pos.x;
        if (dir == 'f')
        {
            if (grid_pos.x == 0)
            {
                feedback.printMessage("Player " + (team.player_id + 1) + "\nYou can't move into the enemy zone.", MessageType.Error);
                return false;
            }
            else
            {
                target_col--;
            }
        }
        else if (dir == 'b')
        {
            if (grid_pos.x == 3)
            {
                feedback.printMessage("Player " + (team.player_id + 1) + "\nYou can't move off the grid.", MessageType.Error);
                return false;
            }
            else
            {
                target_col++;
            }
        }
        else
        {
            Debug.LogError("Error with the direction of the movement");
            target_col = -1;
        }

        short pos = (short)grid_pos.x;
        if (grid.checkAdd(target_col))
        {
            return true;
        }
        else
        {
            feedback.printMessage("Player " + (team.player_id + 1) + "\nThat unit can not move there because it is full up.", MessageType.Error);
            return false;
        }
    }

    public void move(char dir, ActionFeedbackText feedback)
    {
        ushort target_col = (ushort)grid_pos.x;
        if (dir == 'f')
        {
            target_col--;
        }
        else if (dir == 'b')
        {
            target_col++;
        }
        short pos = (short)grid_pos.x;
        if (grid.addUnitToCol(target_col, this))
        {
            grid.updateGrid();
            grid.updateSprites();
        }
        else
        {
            feedback.printMessage("At the start of the turn, " + getName() + " wanted to move, and they could, but now they can not.");
        }
    }

    public int getPos()
    {
        int pos = grid_pos.x;
        for (short i = (short)grid_pos.x; i >= 0; i--)
        {
            if (team.colEmpty(i))
            {
                pos--;
            }
        }
        return pos;
    }
}
