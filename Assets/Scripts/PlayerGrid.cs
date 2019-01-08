﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    [SerializeField] short player;
    [SerializeField] PlayerGrid opponent_grid;
    [SerializeField] UnitSprites sprites;
    [SerializeField] Transform[] columns;
    short[] units_in_col;

    private void Start()
    {
        units_in_col = new short[columns.Length];
        for (int i = 0; i < units_in_col.Length; i++)
            units_in_col[i] = 0;
    }

    public bool addUnitToCol(short col, Unit unit)
    {
        if (units_in_col[col] >= columns[col].childCount)
        {
            return false;
        }
        short x = col;
        unit.grid_pos = new Vector2Int(x, units_in_col[col]);
        columns[col].GetChild(units_in_col[col]).GetComponent<SpriteRenderer>().sprite = sprites.getSprite(unit);
        units_in_col[col]++;
        return true;
    }

    public void updateGrid()
    {
        for(int i = 0; i < units_in_col.Length; i++)
        {
            units_in_col[i] = 0;
        }
        foreach (Unit u in FindObjectOfType<Field>().getTeam(player).getUnits(true))
        {
            addUnitToCol((short)u.grid_pos.x, u);
        }
    }

    public bool checkAdd(short col)
    {
        if (units_in_col[col] >= columns[col].childCount)
        {
            return false;
        }
        return true;
    }

    public void updateSprites()
    {
        for (int i = 0; i < units_in_col.Length; i++)
        {
            for (int j = units_in_col[i]; j < columns[i].childCount; j++)
            {
                columns[i].GetChild(j).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        return;
    }

    public SpriteRenderer getSprite(Unit unit)
    {
        if (unit.getTeam().player_id != player)
        {
            return opponent_grid.getSprite(unit);
        }
        else
        {
            return columns[unit.grid_pos.x].GetChild(unit.grid_pos.y).GetComponent<SpriteRenderer>();
        }
    }
}