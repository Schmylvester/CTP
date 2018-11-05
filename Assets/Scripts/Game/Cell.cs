using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    N, E, S, W,
}

public class Cell : MonoBehaviour
{
    Vector2Int cell_index;
    Cell[] neighbours;
    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        neighbours = new Cell[4] { null, null, null, null };
    }

    public void setPos(int x, int y)
    {
        cell_index = new Vector2Int(x, y);
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
}