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
    Vector2Int cell_index;
    Cell[] neighbours;
    Visibility visibility;

    private void Awake()
    {
        setVisible(Visibility.Hidden);
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

    public void lightUp(int range, List<Cell> lit)
    {
        if (lit.Contains(this))
        {
            return;
        }
        else
        {
            lit.Add(this);
            range--;
            if (range > 0)
            {
                foreach (Cell c in neighbours)
                {
                    if (c)
                        c.lightUp(range, lit);
                }
            }

            setVisible(Visibility.Clear);
        }
    }

    private void Update()
    {
        if (Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position)) < 10)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            foreach (Cell c in neighbours)
            {
                if (c) 
                c.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
        else if(GetComponent<SpriteRenderer>().color == Color.red)
        {
            setVisible(visibility);
            foreach (Cell c in neighbours)
            {
                if (c)
                    c.setVisible(visibility);
            }
        }
    }

    void setVisible(Visibility set)
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
}