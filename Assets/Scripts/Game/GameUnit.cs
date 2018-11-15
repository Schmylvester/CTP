using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    List<int> stats;
    GridManager grid;
    Cell current_cell;

    public void setUp(Class unit_class, UnitStats unit_stats, Cell cell, GridManager _grid)
    {
        grid = _grid;
        stats = new List<int>();
        sprite.sprite = unit_stats.getSprite(unit_class);
        unit_stats.getStats(unit_class, ref stats);
        current_cell = cell;
        move(cell);
    }

    public int getMove()
    {
        return stats[(int)Stat.MOVE];
    }
    public int getSight()
    {
        return stats[(int)Stat.SIGHT];
    }

    public void move(Cell new_cell)
    {
        if (new_cell.getDistance(current_cell) <= getMove())
        {
            transform.parent = new_cell.transform;
            transform.localPosition = Vector3.zero;
            current_cell = new_cell;
            grid.unitMove(this, new_cell);
        }
    }

    public Cell getCell()
    {
        return current_cell;
    }
}