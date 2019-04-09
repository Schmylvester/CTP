using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeamOrder : MonoBehaviour
{
    static int total_set = 0;
    Field field;
    [SerializeField] short team;
    [SerializeField] PlayerGrid grid;
    [SerializeField] UnityEngine.UI.Image[] selectable_units;
    [SerializeField] GameObject action_man;
    int selected_unit = 0;
    bool units_assigned = false;
    InputManager input;

    private void Start()
    {
        input = FindObjectOfType<InputManager>();
        field = FindObjectOfType<Field>();
        selectable_units[selected_unit].color = new Color(0.4f, 0.7f, 1.0f);
    }

    private void Update()
    {
        if (!units_assigned)
            if (Input.GetKeyDown(KeyCode.Alpha1) || input.buttonDown(XboxButton.A, team))
                assignUnit(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2) || input.buttonDown(XboxButton.B, team))
                assignUnit(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3) || input.buttonDown(XboxButton.X, team))
                assignUnit(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4) || input.buttonDown(XboxButton.Y, team))
                assignUnit(3);
        if (total_set == 2)
        {
            action_man.SetActive(true);
            enabled = false;
        }
    }

    void assignUnit(ushort col)
    {
        Unit unit = field.getTeam(team).getUnits(false)[selected_unit];
        if (grid.addUnitToCol(col, unit))
        {
            FindObjectOfType<GameTracker>().addUnit(unit);
            selectable_units[selected_unit].color = Color.white;
            selected_unit++;
            if (selected_unit < selectable_units.Length)
                selectable_units[selected_unit].color = new Color(0.4f, 0.7f, 1.0f);
            else
            {
                total_set++;
                units_assigned = true;
                grid.updateSprites();
            }
        }
    }
}
