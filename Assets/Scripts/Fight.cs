using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Evolution evolution;
    public void fight(Fighter a, Fighter b)
    {
        int break_trap = 0;
        while(a.health > 0 && b.health > 0 && break_trap++ < 1000)
        {
            a.dealDamage(b);
            if(b.health > 0)
            {
                b.dealDamage(a);
            }
            else
            {
                evolution.addWinner(a);
                break;
            }
            if(a.health <= 0)
            {
                evolution.addWinner(b);
                break;
            }
        }

        if(break_trap > 500)
        {
            Debug.Log("A.H: " + a.health + " A.A: " + a.attack + " B.H: " + b.health + " B.A: " + b.attack);
        }
    }
}
