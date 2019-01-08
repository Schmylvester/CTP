using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct UnitData
{
    public short unit_id;
    public short unit_pos_x;
    public short unit_pos_y;
}

public struct ActionData
{
    public short action_idx;
    public short target_x;
    public short target_y;
    public short target_team;
}

public class GameTracker : MonoBehaviour
{
    List<Unit> unit_list = new List<Unit>();
    short random_seed;
    string actions = "";
    string units = "";
    StreamWriter file_writer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        random_seed = (short)Random.Range(0, short.MaxValue);
        Random.InitState(random_seed);
    }

    public void addUnit(Unit u)
    {
        unit_list.Add(u);
        if (unit_list.Count == 10)
        {
            List<Unit> temp_list = new List<Unit>();
            foreach (Unit unit in unit_list)
                if (unit.getTeam().player_id == 0)
                    temp_list.Add(unit);
            foreach (Unit unit in unit_list)
                if (unit.getTeam().player_id == 1)
                    temp_list.Add(unit);

            foreach (Unit unit in temp_list)
            {
                UnitData data = new UnitData();
                data.unit_id = getID(unit);
                data.unit_pos_x = (short)unit.grid_pos.x;
                data.unit_pos_y = (short)unit.grid_pos.y;
                setUnit(data);
            }
        }
    }

    void setUnit(UnitData data)
    {
        int new_data = 0;

        new_data += data.unit_id * 16;
        new_data += data.unit_pos_x * 4;
        new_data += data.unit_pos_y;

        units += (char)new_data;
    }

    public void addAction(ActionData data)
    {
        int new_data = 0;

        new_data += data.action_idx * 32;
        new_data += data.target_x * 8;
        new_data += data.target_y * 2;
        new_data += data.target_team;

        actions += (char)new_data;
    }

    public void gameEnd()
    {
        file_writer = new StreamWriter("Assets\\Games\\" + dateWithoutSpace() + ".txt", true);
        if (file_writer != null)
        {
            file_writer.WriteLine(random_seed.ToString());
            file_writer.WriteLine(units);
            file_writer.WriteLine(actions);
            file_writer.Close();
        }
    }

    string dateWithoutSpace()
    {
        string ret_str = "";
        string with_space = System.DateTime.Now.ToString();
        foreach (char c in with_space)
        {
            if (c != '/' && c != ' ' && c != ':')
            {
                ret_str += c;
            }
        }
        return ret_str;
    }

    short getID(Unit u)
    {
        if (u as Bard != null)
            return 0;
        if (u as Cleric != null)
            return 1;
        if (u as Knight != null)
            return 2;
        if (u as Necromancer != null)
            return 3;
        if (u as Paladin != null)
            return 4;
        if (u as Pirate != null)
            return 5;
        if (u as Ranger != null)
            return 6;
        if (u as Rogue != null)
            return 7;
        if (u as Vampire != null)
            return 8;
        return -1;
    }
}
