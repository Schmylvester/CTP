using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUnit : MonoBehaviour
{
    [SerializeField] GameObject unit_prefab;
    [SerializeField] Transform grid_parent;
    Cell[] cells;
    [SerializeField] UnitStats stats;

    public void init()
    {
        cells = grid_parent.GetComponentsInChildren<Cell>();
    }

    public GameUnit addUnit(Class unit_type, Vector2Int cell, GridManager grid)
    {
        Cell unit_cell = null;
        Transform cell_loc = null;
        for(int i = 0; i < cells.Length; i++)
        {
            if(cells[i].getPos() == cell)
            {
                cell_loc = cells[i].transform;
                unit_cell = cells[i];
            }
        }
        GameObject new_unit = Instantiate(unit_prefab, cell_loc);
        switch (cell.y)
        {
            case 0:
                new_unit.name = "Noddy";
                break;
            case 1:
                new_unit.name = "Eric";
                break;
            case 2:
                new_unit.name = "Wonk";
                break;
            case 3:
                new_unit.name = "Archer";
                break;
            case 4:
                new_unit.name = "God";
                break;
            case 5:
                new_unit.name = "Hecker";
                break;
            case 6:
                new_unit.name = "Mr. Meme";
                break;
        }
        GameUnit unit = new_unit.GetComponent<GameUnit>();
        unit.setUp(unit_type, stats, unit_cell, grid);

        return unit;
    }
}
