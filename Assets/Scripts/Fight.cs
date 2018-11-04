using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Evolution evolution = null;
    public void fight(Fighter a, Fighter b)
    {
        int timer = 0;
        while (a.getAlive() && b.getAlive() && timer++ < 100)
        {
            a.dealDamage(b);
            if (b.getAlive())
            {
                b.dealDamage(a);
                if (!a.getAlive())
                {
                    evolution.addWinner(b);
                }
            }
            else
            {
                evolution.addWinner(a);
            }
        }
        if(timer == 100 )
        {
            evolution.addWinner(b);
            Debug.LogError("Not a good");
        }
    }
}
