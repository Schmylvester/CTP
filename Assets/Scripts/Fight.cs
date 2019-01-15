using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Evolution evolution = null;
    public void fight(Unit a, Unit b)
    {
        evolution.addWinner(a.attack(b));
    }
}
