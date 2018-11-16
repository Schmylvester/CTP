using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputState
{
    Unit_Select,
    Unit_Move,
    Unit_Attack,
}

public class InputManager : MonoBehaviour
{
    [SerializeField] int players;
    int active_player = 0;

    [SerializeField] GridManager grid;
    InputState state = InputState.Unit_Select;
    GameUnit selected_unit = null;
    Cell selected_cell = null;

    Color attack_flash = new Color(1, 0.9f, 0.9f);
    Color move_flash = new Color(0.9f, 1, 0.9f);

    int moves_per_turn = 2;
    int moves_taken = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 world_click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell nearest_cell = getClosestCell(world_click, grid.getCells());
            if (state == InputState.Unit_Select)
            {
                selected_unit = nearest_cell.getUnitInCell();
                if (selected_unit)
                {
                    if (selected_unit.getTeam() == active_player)
                    {
                        selected_cell = nearest_cell;
                        setCellsFlash(selected_cell, selected_unit.getRange(), attack_flash);
                        setCellsFlash(selected_cell, selected_unit.getStat(Stat.MOVE), move_flash);
                        state = InputState.Unit_Move;
                    }
                }
            }
            else if (state == InputState.Unit_Move)
            {
                if (selected_unit)
                {
                    if (!nearest_cell.getUnitInCell() &&
                       nearest_cell.getDistance(selected_cell)
                        <= selected_unit.getStat(Stat.MOVE))
                    {
                        setCellsFlash(selected_cell, selected_unit.getRange(), Color.clear);
                        selected_unit.move(nearest_cell);
                        selected_cell = nearest_cell;
                    }
                    else
                    {
                        setCellsFlash(selected_cell, selected_unit.getRange(), Color.clear);
                    }
                    setCellsFlash(selected_cell, selected_unit.getStat(Stat.ATTACK_RANGE), attack_flash);
                    state = InputState.Unit_Attack;
                }
            }
            else if (state == InputState.Unit_Attack)
            {
                GameUnit target = nearest_cell.getUnitInCell();
                if (target)
                {
                    if (target.getTeam() != active_player
                       && nearest_cell.getDistance(selected_cell)
                       <= selected_unit.getStat(Stat.ATTACK_RANGE))
                    {
                        selected_unit.attack(target);
                    }
                }

                state = InputState.Unit_Select;
                setCellsFlash(selected_cell, selected_unit.getStat(Stat.ATTACK_RANGE), Color.clear);
                selected_unit = null;
                selected_cell = null;
                moves_taken++;
                if (moves_taken >= moves_per_turn)
                {
                    turnOver();
                }
            }
        }
    }

    void turnOver()
    {
        moves_taken = 0;
        active_player = (active_player + 1) % players;
        grid.setUpVisibility();
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


    void setCellsFlash(Cell mid_cell, int range, Color flash_colour)
    {
        foreach(Cell cell in grid.getCells())
        {
            if(cell.getDistance(mid_cell) <= range)
            {
                cell.setFlash(flash_colour);
            }
        }
    }

    public int getActivePlayer()
    {
        return active_player;
    }
}