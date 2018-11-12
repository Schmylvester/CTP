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

    public void addUnit(Class unit_type, Vector2Int cell)
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
        new_unit.GetComponent<GameUnit>().setUp(unit_type, stats, unit_cell);
    }
}
