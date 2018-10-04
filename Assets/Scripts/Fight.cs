using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Evolution evolution;
    public void fight(Fighter a, Fighter b)
    {
        int i = 0;
        while(a.health > 0 && b.health > 0 && i++ < 1000)
        {
            a.dealDamage(b);
            if(b.health > 0)
            {
                b.dealDamage(a);
            }
            else
            {
                evolution.addWinner(a);
            }
            if(a.health <= 0)
            {
                evolution.addWinner(b);
            }
        }

        if(i > 500)
        {
            Debug.Log("A.H: " + a.health + " A.A: " + a.attack + " B.H: " + b.health + " B.A: " + b.attack);
        }
    }
}
