using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    List<int> stats;

    public void setUp(Class unit_class, UnitStats unit_stats, Cell cell)
    {
        stats = new List<int>();
        sprite.sprite = unit_stats.getSprite(unit_class);
        unit_stats.getStats(unit_class, ref stats);
        cell.lightUp(getSight() + 1, new List<Cell>());
    }

    public int getSight()
    {
        return stats[(int)Stat.SIGHT];
    }
}