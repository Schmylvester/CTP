using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter
{
    public int max_health;
    public int health;
    public int attack;

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void dealDamage(Fighter target)
    {
        if (target.max_health > max_health)
        {
            target.takeDamage(1);
        }
        else
        {
            target.takeDamage(attack);
        }
    }
}