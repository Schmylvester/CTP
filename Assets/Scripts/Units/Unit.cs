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

public abstract class Unit : MonoBehaviour
{
    protected int current_health;
    protected int[] base_stats;
    protected int[] temp_stats;
    protected PlayerGrid grid;
    public Vector2Int grid_pos;
    protected List<Ability> abilities;
    protected Team team;

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
        validateStats();
        base_stats[(int)Stat.Max_HP] = 30 + (base_stats[(int)Stat.Max_HP] * 3);
        resetStats();
        current_health = getStat(Stat.Max_HP);
        abilities = new List<Ability>();
        setAbilities();

        foreach (Ability a in abilities)
        {
            a.setUser(this);
        }
    }

    public void setGrid(PlayerGrid _grid)
    { grid = _grid; }

    public void attack(Unit target, bool distance_penalty = true)
    {
        if (taunted_by != null)
        {
            resetStats();
            target = taunted_by;
            taunted_by = null;
            attack(target);
            return;
        }

        float attack_miss = 0.6f;
        //reduce miss chance by 10% for each of my accuracy stat points
        attack_miss *= Mathf.Pow(0.9f, getStat(Stat.Accuracy));
        //increase miss chance by 5% for each of my target's agility points
        attack_miss *= Mathf.Pow(1.05f, target.getStat(Stat.Agility));

        if (distance_penalty)
        {
            //for each space I am back, add 20% to the attack miss
            attack_miss *= Mathf.Pow(1.2f, getPos());
            //for each space the target is back, add 60% to the attack miss
            attack_miss *= Mathf.Pow(1.6f, target.getPos());
        }

        if (Random.Range(0.0f, 1.0f) > attack_miss)
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
                base_damage += Random.Range(1, 4);
            }
            target.takeDamage(base_damage);
        }
        else
            Debug.Log("Attack had a " + ((int)((1 - attack_miss) * 100)).ToString() + "% chance of hitting, but it missed.");
    }

    public int takeDamage(int damage)
    {
        if (defended_by != null)
        {
            Debug.Log("Attack blocked by " + defended_by.getName());
            return defended_by.takeDamage(damage);
        }
        if ((this as Rogue) != null)
            (this as Rogue).sneak_turns = 0;
        int damage_taken = 0;
        for (int i = 0; i < getStat(Stat.Defence); i++)
        {
            damage_taken += Random.Range(1, 3);
        }
        //if the attack hits, at least one damage
        damage_taken = Mathf.Max(1, damage_taken);
        changeHealth(-damage_taken);
        Debug.Log("Attack hits for " + damage_taken + " damage.");
        if (current_health <= 0)
        {
            die();
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

    public void changeHealth(int change)
    {
        current_health += change;
        current_health = Mathf.Min(current_health, getStat(Stat.Max_HP));
        current_health = Mathf.Max(current_health, 0);
        if (current_health <= 0)
            die();
    }

    public int getStat(Stat stat, bool temp = true)
    {
        return temp ? temp_stats[(int)stat] : base_stats[(int)stat];
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

    bool validateStats()
    {
        int total = 0;

        for (int i = 0; i < (int)Stat.COUNT; i++)
        {
            total += base_stats[i];
        }

        if (total < 49)
        {
            Debug.LogError(getName() + " is underpowered by " + (49 - total) + " point(s).");
            return false;
        }
        if (total > 49)
        {
            Debug.LogError(getName() + " is overpowered by " + (total - 49) + " point(s).");
            return false;
        }
        return true;
    }

    public virtual void die()
    {
        if (getEffectActive(SingleTurnEffects.CheatingDeath))
        {
            if (Random.Range(0.0f, 1.0f) < 0.8f)
            {
                Debug.Log(getName() + " cheated death.");
                current_health = 1;
                return;
            }
        }
        if (getEffectActive(SingleTurnEffects.DownWithShip))
        {
            Debug.Log(getName() + " is taking you with them.");
            foreach (Unit u in FindObjectOfType<Field>().getTeam(1 - team.player_id).getUnits(true))
            {
                attack(u);
            }
        }

        Debug.Log(getName() + " died.");
        grid.getSprite(this).color = Color.red;
    }

    public bool getTaunt()
    {
        return taunted_by != null;
    }

    public void move(char dir)
    {
        short target_col = (short)grid_pos.x;
        if (dir == 'f')
        {
            if (grid_pos.x == 7 || grid_pos.x == 3)
            {
                Debug.Log("You can't move there you're on the edge or your area.");
            }
            else
            {
                target_col++;
            }
        }
        else if (dir == 'b')
        {
            if (grid_pos.x == 0 || grid_pos.x == 4)
            {
                Debug.Log("You can't move there you're on the edge or your area.");
            }
            else
            {
                target_col--;
            }
        }
        else
        {
            Debug.LogError("Error with the direction of the movement");
            target_col = -1;
        }

        short pos = (short)grid_pos.x;
        if (grid.addUnitToCol(target_col, this))
        {
            grid.removeUnitFromCol(pos);
            grid.updateSprites();
        }
        else
        {
            Debug.Log("That unit can not move there because it is full up.");
        }
    }

    public int getPos()
    {
        //starting with the row in front, ending when there's a populated row or once the front row is checked
        return 0;
    }
}
