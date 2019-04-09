using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReplayGame : MonoBehaviour
{
    [SerializeField] string file_name;
    StreamReader file;
    [SerializeField] Field field;
    Team[] teams;
    [SerializeField] PlayerGrid[] player_grids;
    [SerializeField] GameObject action_man;
    [SerializeField] AllTurnActions all_actions;

    void Awake()
    {
        teams = field.getTeams();
        file = new StreamReader("Assets\\Games\\" + file_name + ".txt");
        int r_seed = int.Parse(file.ReadLine());
        Random.InitState(r_seed);
        setUnits();
        setActions(0);
    }

    void setUnits()
    {
        char[] _char = new char[20];
        {
            int i = 0;
            while (_char[i++] != 255)
            {
                file.ReadBlock(_char, i, 1);
            }
        }
        UnitData[] data = new UnitData[10];
        Unit[][] team = new Unit[2][] { new Unit[5], new Unit[5] };
        for (int i = 0; i < 10; i++)
        {
            ushort unit = (ushort)_char[i + 1];
            data[i].unit_id = (ushort)(unit / 4);
            unit = (ushort)(unit % 4);
            data[i].unit_pos_x = unit;
            if (i < 5)
                team[0][i] = UnitIDs.getUnit(data[i].unit_id);
            else
                team[1][i - 5] = UnitIDs.getUnit(data[i].unit_id);
        }
        for (int i = 0; i < 2; i++)
        {
            field.setTeam(i, team[i]);
            for (int j = 0; j < 5; j++)
                player_grids[i].addUnitToCol(data[j + (i * 5)].unit_pos_x, team[i][j]);
        }
        action_man.SetActive(true);
    }

    public void setActions(int spaces_back)
    {
        teams[0].setGridAndField(player_grids[0], field);
        teams[1].setGridAndField(player_grids[1], field);

        char[] action_char = new char[40];
        int file_index = 0;

        while (action_char[file_index++] != 255)
        {
            file.ReadBlock(action_char, file_index, 1);
            if (file_index >= 35)
            {
                file.Close();
                Debug.Log("Game Over");
                enabled = false;
                return;
            }
        }

        List<Unit> units = new List<Unit>();
        for (int i = 0; i < 10; i++)
        {
            Unit u = teams[i / 5].getUnits(true)[i % 5];
            if (u.getHealth() > 0)
            {
                units.Add(u);
            }
        }

        List<Ability> actions = new List<Ability>();
        for (int i = 0; i < units.Count; i++)
        {
            short act = (short)action_char[i + 1];
            ActionData action = new ActionData
            {
                action_idx = (short)(act / 32),
                target_x = (short)((act % 32) / 8),
                target_y = (short)((act % 8) / 2),
                target_team = (short)((act % 2))
            };

            Ability ability = null;
            switch (action.action_idx)
            {
                case 0:
                    ability = new Abilities.Attack();
                    break;
                case 1:
                case 2:
                case 3:
                    ability = units[i].getAbility(action.action_idx - 1);
                    break;
                case 4:
                    ability = new Abilities.Move();
                    ((Abilities.Move)ability).direction = action.target_x == 1 ? 'f' : 'b';
                    break;
            }
            ability.setUser(units[i]);
            ability.setTarget(field.findUnitAtPos(action.target_x, action.target_y, action.target_team));
            actions.Add(ability);
        }
        all_actions.addActionsFromFile(actions);
    }


    private void OnApplicationQuit()
    {
        file.Close();
    }
}
