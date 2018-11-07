using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUnit : MonoBehaviour
{
    [SerializeField] GameObject unit_prefab;
    [SerializeField] Transform grid_parent;
    Cell[] cells;
    [SerializeField] UnitTypes types;

    public void init()
    {
        cells = grid_parent.GetComponentsInChildren<Cell>();
    }

    public void addUnit(Class unit_type, Vector2Int cell)
    {
        Transform cell_loc = null;
        for(int i = 0; i < cells.Length; i++)
        {
            if(cells[i].getPos() == cell)
            {
                cell_loc = cells[i].transform;
            }
        }
        GameObject new_unit = Instantiate(unit_prefab, cell_loc);
        new_unit.GetComponent<SpriteRenderer>().sprite = types.getSprite(unit_type);
    }
}
