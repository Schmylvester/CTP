using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
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
        short x = -1;
        if (unit.getTeam().player_id == 0)
            x = col;
        else
            x = (short)(7 - col);
        unit.grid_pos = new Vector2Int(x, units_in_col[col]);
        columns[col].GetChild(units_in_col[col]).GetComponent<SpriteRenderer>().sprite = sprites.getSprite(unit);
        units_in_col[col]++;
        unit.movePosition(col);
        return true;
    }

    public void doneAssigningPositons()
    {
        for (int i = 0; i < units_in_col.Length; i++)
        {
            for (int j = units_in_col[i]; j < columns[i].childCount; j++)
            {
                columns[i].GetChild(j).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    public SpriteRenderer getSprite(Unit unit)
    {
        int x;
        if (unit.getTeam().player_id == 0)
            x = unit.grid_pos.x;
        else
            x = (short)(7 - unit.grid_pos.x);
        return columns[x].GetChild(unit.row).GetComponent<SpriteRenderer>();
    }
}
