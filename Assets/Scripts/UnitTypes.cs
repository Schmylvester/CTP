using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Mage,
    Rogue,
    Warrior,

    UNIT_COUNT
}

public class UnitTypes : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    public Sprite getSprite(Class type)
    {
        return sprites[(int) type];
    }
}
