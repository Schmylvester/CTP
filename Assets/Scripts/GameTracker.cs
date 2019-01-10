using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct UnitData
{
    public ushort unit_id;
    public ushort unit_pos_x;
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
    bool saved = false;

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
                data.unit_id = UnitIDs.getID(unit);
                data.unit_pos_x = (ushort)unit.grid_pos.x;
                setUnit(data);
            }
        }
    }

    void setUnit(UnitData data)
    {
        int new_data = 0;

        new_data += data.unit_id * 4;
        new_data += data.unit_pos_x;

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

    public void actionsSetForTurn()
    {
        actions += (char)255;
    }

    private void OnApplicationQuit()
    {
        if(!saved)
            gameEnd();
    }

    public void gameEnd()
    {
        if (!saved)
        {
            file_writer = new StreamWriter("Assets\\Games\\" + dateWithoutSpace() + ".txt", true);
            if (file_writer != null)
            {
                file_writer.WriteLine(random_seed.ToString());
                file_writer.Write(units);
                file_writer.Write((char)(255));
                file_writer.Write(actions);
                file_writer.Close();
            }
            saved = true;
        }
    }

    public void disableSave()
    {
        saved = true;
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
}
