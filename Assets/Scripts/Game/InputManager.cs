using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputState
{
    Unit_Select,
    Unit_Move,

}

public class InputManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    InputState state = InputState.Unit_Select;
    GameUnit selected_unit = null;
    Cell selected_cell = null;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 world_click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell nearest_cell = getClosestCell(world_click, grid.getCells());
            if (state == InputState.Unit_Select)
            {
                selected_unit = nearest_cell.getUnitInCell();
                if (selected_unit)
                {
                    selected_cell = nearest_cell;
                    //setCellsFlash(selected_cell, selected_unit.getMove(), new Color(0.1f, 0, 0.1f, 0));
                    state = InputState.Unit_Move;
                }
            }
            else if(state == InputState.Unit_Move)
            {
                if (selected_unit)
                {
                    selected_unit.move(nearest_cell);
                    //setCellsFlash(selected_cell, selected_unit.getMove(), new Color(0.1f, 0, 0.1f, 0));
                    selected_cell = null;
                    selected_unit = null;
                    state = InputState.Unit_Select;
                }
                else
                {
                    state = InputState.Unit_Select;
                }
            }
        }
    }

    Cell getClosestCell(Vector2 click_pos, List<Cell> cells)
    {
        float closest = float.MaxValue;
        Cell cell_closest = null;

        foreach (Cell cell in cells)
        {
            float dist = Vector2.Distance(click_pos, cell.transform.position);
            if (dist < closest)
            {
                closest = dist;
                cell_closest = cell;
            }
        }

        return cell_closest;
    }
}
