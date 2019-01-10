using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIDs : MonoBehaviour
{
    public static ushort getID(Unit u)
    {
        if (u as Bard != null)
            return 0;
        if (u as Cleric != null)
            return 1;
        if (u as Knight != null)
            return 2;
        if (u as Necromancer != null)
            return 3;
        if (u as Paladin != null)
            return 4;
        if (u as Pirate != null)
            return 5;
        if (u as Ranger != null)
            return 6;
        if (u as Rogue != null)
            return 7;
        if (u as Vampire != null)
            return 8;
        Debug.LogError("Invalid unit");
        return 80;
    }

    public static Unit getUnit(ushort id)
    {
        switch (id)
        {
            case 0:
                return new Bard();
            case 1:
                return new Cleric();
            case 2:
                return new Knight();
            case 3:
                return new Necromancer();
            case 4:
                return new Paladin();
            case 5:
                return new Pirate();
            case 6:
                return new Ranger();
            case 7:
                return new Rogue();
            case 8:
                return new Vampire();
        }
        return null;
    }
}
