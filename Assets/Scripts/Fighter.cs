using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter
{
    public int max_health;
    public int health;
    public int attack;
    public int clumsiness;

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void dealDamage(Fighter target)
    {
        if (target.clumsiness > clumsiness)
        {
            target.takeDamage(attack);
        }

        target.takeDamage(attack);
    }
}