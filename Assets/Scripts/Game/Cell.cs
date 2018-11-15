using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Visibility
{
    Clear,
    Foggy,
    Hidden
}
public enum Dir
{
    N, E, S, W,
}

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    GridManager grid;
    Vector2Int cell_index;
    Cell[] neighbours;
    Visibility visibility;
    GameUnit unit_in_cell = null;

    private void Awake()
    {
        setVisible(Visibility.Hidden);
        neighbours = new Cell[4] { null, null, null, null };
    }

    public void setPos(int x, int y, GridManager _grid)
    {
        cell_index = new Vector2Int(x, y);
        grid = _grid;
    }

    public Vector2Int getPos()
    {
        return cell_index;
    }

    public void addNeighbour(Cell cell, Dir direction)
    {
        neighbours[(int)direction] = cell;
        Dir opposite_dir = (Dir)(((int)(direction) + 2) % 4);
    }

    public void setClearCells(int range, GameUnit _by)
    {
        setVisible(Visibility.Clear);
        foreach (Cell cell in grid.getCells())
        {
            if (getDistance(cell) <= range)
            {
                cell.setVisible(Visibility.Clear, _by);
            }
        }
        foreach (Cell cell in grid.getCells())
        {
            if (getDistance(cell) <= range + 2 && getDistance(cell) > range && cell.getVisible() == Visibility.Hidden)
            {
                cell.setVisible(Visibility.Foggy, _by);
            }
        }
    }

    public void setVisible(Visibility set, GameUnit _by = null)
    {
        visibility = set;
        switch (visibility)
        {
            case Visibility.Clear:
                sprite.color = Color.white;
                break;
            case Visibility.Foggy:
                sprite.color = Color.grey / 2;
                break;
            case Visibility.Hidden:
                sprite.color = Color.grey;
                break;
        }
    }

    public Visibility getVisible()
    {
        return visibility;
    }

    public int getDistance(Cell cell)
    {
        int x = Mathf.Abs(cell.getPos().x - cell_index.x);
        int y = Mathf.Abs(cell.getPos().y - cell_index.y);

        return x + y;
    }

    public void unitMove(GameUnit unit, Cell move_to)
    {
        if (unit_in_cell == unit)
        {
            unit_in_cell = null;
        }
        if (move_to == this)
        {
            unit_in_cell = unit;
        }
    }

    public GameUnit getUnitInCell()
    {
        return unit_in_cell;
    }

}