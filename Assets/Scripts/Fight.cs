using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Evolution evolution = null;
    public void fight(Fighter a, Fighter b)
    {
        while (a.getAlive() && b.getAlive())
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
    }
}
