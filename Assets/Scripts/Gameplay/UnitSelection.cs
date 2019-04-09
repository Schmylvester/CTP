using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    List<Unit> unit_list;
    int selected_unit = 0;
    int selected_remove = -1;
    List<ushort> units_on_team;
    float last_frame_input;

    [SerializeField] int player_id;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image unit_image;
    [SerializeField] Text unit_name;
    [SerializeField] Text stats;
    [SerializeField] Text[] ability;
    [SerializeField] Image[] units_selected;
    [SerializeField] Image ready_light;
    [SerializeField] Field unit_manager;
    [SerializeField] InputManager input;
    [SerializeField] PredictTeam predict;

    bool player_ready = false;

    private void Awake()
    {
        unit_list = new List<Unit>() {
            new Bard(),
            new Cleric(),
            new Knight(),
            new Necromancer(),
            new Paladin(),
            new Pirate(),
            new Ranger(),
            new Rogue(),
            new Vampire()
        };

        changeSelection(0);

        units_on_team = new List<ushort>();
    }

    private void Update()
    {
        AxisState ls_state = AxisState.Null;
        float ls_hz_input = input.getAxisAndState(Axis.Left_Horizontal, player_id, ref ls_state);
        AxisState dp_state = AxisState.Null;
        float dp_hz_input = input.getAxisAndState(Axis.D_Horizontal, player_id, ref dp_state);

        if ((ls_hz_input > 0 && ls_state == AxisState.Triggered_This_Frame) ||
            dp_hz_input > 0 && dp_state == AxisState.Triggered_This_Frame)
        {
            changeSelection(1);
        }
        else if ((ls_hz_input < 0 && ls_state == AxisState.Triggered_This_Frame)
            || (dp_hz_input < 0 && dp_state == AxisState.Triggered_This_Frame))
        {
            changeSelection(-1);
        }

        if (input.buttonDown(XboxButton.A, player_id))
        {
            addUnit();
        }
        if (input.buttonDown(XboxButton.B, player_id))
        {
            removeUnit();
        }
        if (input.buttonDown(XboxButton.Start, player_id))
        {
            readyUp();
        }
        if (input.buttonDown(XboxButton.LB, player_id))
        {
            changeRemoveSelected(-1);
        }
        if (input.buttonDown(XboxButton.RB, player_id))
        {
            changeRemoveSelected(1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player_id == 0)
        {
            predictNext();
        }
    }

    void changeSelection(int dir)
    {
        selected_unit += dir;

        if (selected_unit >= unit_list.Count)
        {
            selected_unit = 0;
        }
        if (selected_unit < 0)
        {
            selected_unit = (ushort)(unit_list.Count - 1);
        }

        unit_image.sprite = sprites[selected_unit];
        Unit selected = unit_list[selected_unit];
        unit_name.text = selected.getName();
        stats.text = selected.getStat(Stat.Max_HP)
            + "\n\n" + selected.getStat(Stat.Attack)
            + "\n\n" + selected.getStat(Stat.Defence)
            + "\n\n" + selected.getStat(Stat.Intelligence)
            + "\n\n" + selected.getStat(Stat.Accuracy)
            + "\n\n" + selected.getStat(Stat.Agility)
            + "\n\n" + selected.getStat(Stat.Speed);
        for (int i = 0; i < 3; i++)
            ability[i].text = selected.getAbility(i).ability_name + ":\n" + selected.getAbility(i).ability_description;
    }

    void changeRemoveSelected(int direction)
    {
        if (selected_remove >= 0)
        {
            units_selected[selected_remove].color = Color.white;
        }
        selected_remove += direction;
        if (selected_remove < 0 && units_on_team.Count > 0)
        {
            selected_remove = units_on_team.Count - 1;
        }
        if (selected_remove >= units_on_team.Count)
        {
            selected_remove = 0;
        }
        if (selected_remove >= 0)
        {
            units_selected[selected_remove].color = new Color(1.0f, 0.6f, 0.6f, 1.0f);
        }
    }

    void teamUpdated()
    {
        for (int i = 0; i < units_selected.Length; i++)
        {
            units_selected[i].sprite = null;
        }
        for (int i = 0; i < units_on_team.Count; i++)
        {
            units_selected[i].sprite = sprites[units_on_team[i]];
        }
    }

    void addUnit()
    {
        if (units_on_team.Count < 5)
        {
            units_on_team.Add((ushort)selected_unit);
            teamUpdated();
            changeRemoveSelected(1);
        }
    }

    void removeUnit()
    {
        if (player_ready || selected_remove < 0)
            return;
        if (units_on_team[selected_remove] != -1)
        {
            units_on_team.RemoveAt(selected_remove);
            teamUpdated();
            changeRemoveSelected(-1);
        }
    }

    void readyUp()
    {
        if (player_ready || units_on_team.Count < 5)
        {
            player_ready = false;
            unit_manager.ready_count--;
            ready_light.color = Color.white;
            return;
        }
        player_ready = true;
        ready_light.color = Color.green;
        unit_manager.ready_count++;
        Unit[] team = new Unit[units_on_team.Count];
        for (int i = 0; i < units_on_team.Count; i++)
        {
            team[i] = UnitIDs.getUnit(units_on_team[i]);
        }
        AddToPredictionFile.addTeam(team);
        unit_manager.setTeam(player_id, team);
    }

    void predictNext()
    {
        Unit[] so_far = new Unit[units_on_team.Count];
        for (int i = 0; i < units_on_team.Count; i++)
        {
            so_far[i] = UnitIDs.getUnit(units_on_team[i]);
        }
        predict.attemptPrediction(so_far);
    }
}