using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatID
{
    MAX_HEALTH,
    ATTACK,
    CLUMSINESS,
}

public class Fighter
{
    int max_health;
    int health;
    int attack;
    int clumsiness;

    public Fighter(int _mhp, int _atk, int _clm)
    {
        max_health = _mhp;
        health = _mhp;
        attack = _atk;
        clumsiness = _clm;
    }

    public void dealDamage(Fighter target)
    {
        if (target.clumsiness > clumsiness)
        {
            target.takeDamage(attack);
        }
        target.takeDamage(attack);
    }

    public void takeDamage(int damage)
    {
        //always 1 damage at least
        damage = damage > 1 ? damage : 1;
        health -= damage;
    }

    public void mutate()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                changeStat(ref attack, ref max_health);
                break;
            case 1:
                changeStat(ref attack, ref clumsiness);
                break;
            case 2:
                changeStat(ref max_health, ref attack);
                break;
            case 3:
                changeStat(ref max_health, ref clumsiness);
                break;
            case 4:
                changeStat(ref clumsiness, ref attack);
                break;
            case 5:
                changeStat(ref clumsiness, ref max_health);
                break;
            default:
                break;
        }
    }

    bool changeStat(ref int increase, ref int decrease, int i_by = 2, int d_by = 3)
    {
        if (decrease > d_by)
        {
            increase += i_by;
            decrease -= d_by;
            return true;
        }
        return false;
    }

    //functions below shouldn't ever need to change
    public bool getAlive()
    {
        return health > 0;
    }
    public int getAttack()
    {
        return attack;
    }
    public int getClumsiness()
    {
        return clumsiness;
    }
    public int getMaxHealth()
    {
        return max_health;
    }
    public void heal()
    {
        health = max_health;
    }
}