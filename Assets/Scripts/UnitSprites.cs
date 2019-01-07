using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSprites : MonoBehaviour {
    [SerializeField] Sprite[] units_sprites;

    public Sprite getSprite(Unit unit)
    {
        int idx = -1;
        if (unit.getName() == "Bard")
            idx = 0;
        else if (unit.getName() == "Cleric")
            idx = 1;
        else if (unit.getName() == "Knight")
            idx = 2;
        else if (unit.getName() == "Necromancer")
            idx = 3;
        else if (unit.getName() == "Paladin")
            idx = 4;
        else if (unit.getName() == "Pirate")
            idx = 5;
        else if (unit.getName() == "Ranger")
            idx = 6;
        else if (unit.getName() == "Rogue")
            idx = 7;
        else if (unit.getName() == "Vampire")
            idx = 8;
        else if (unit.getName() == "Skeleton")
            idx = 9;

        return units_sprites[idx];
    }
}
