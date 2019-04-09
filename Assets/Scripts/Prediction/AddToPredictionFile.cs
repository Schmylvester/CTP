using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AddToPredictionFile : MonoBehaviour
{
    public static void addTeam(Unit[] units)
    {
        ushort team = 0;
        for (int i = 0; i < 5; i++)
            team += (ushort)(UnitIDs.getID(units[i]) * Mathf.Pow(9, i));
        StreamWriter file_writer = new StreamWriter("Assets\\Prediction\\TeamPrediction.txt", true);
        file_writer.WriteLine(team);
        file_writer.Close();
    }

    public static void addAction(Field field, Ability ability_selected)
    {
        PredictionInput[] inputs = getInput(field, ability_selected.getUser());
        PredictionOutput output = getOutput(ability_selected);
        addAction(inputs, output);
    }

    public static PredictionInput[] getInput(Field field, Unit active_unit)
    {
        List<PredictionInput> inputs = new List<PredictionInput>();
        inputs.Add(ActionPrediction.getPredictionInput(active_unit));
        short team = active_unit.getTeam().player_id;
        foreach (Unit u in field.getTeam(team).getUnits(true))
        {
            if (u != active_unit)
                inputs.Add(ActionPrediction.getPredictionInput(u));
        }
        foreach (Unit u in field.getTeam(1 - team).getUnits(true))
        {
            inputs.Add(ActionPrediction.getPredictionInput(u));
        }

        return inputs.ToArray();
    }

    static PredictionOutput getOutput(Ability ability)
    {
        Unit actor = ability.getUser();
        Unit target = ability.getTarget();
        ushort target_loc = 0;
        if (target != null)
        {
            if (target.getTeam() != actor.getTeam())
            {
                target_loc += 16;
            }
            target_loc += (ushort)(target.grid_pos.y * 4);
            target_loc += (ushort)(target.grid_pos.x);
        }

        ushort act_out = 0;
        if (ability.ability_name == "Attack")
        {
            act_out = 0;
        }
        else if (ability.ability_name == "Move")
        {
            act_out = 4;
        }
        else
        {
            for (ushort i = 0; i < 3; i++)
            {
                if (ability.ability_name == actor.getAbility(i).ability_name)
                {
                    act_out = (ushort)(i + 1);
                }
            }
        }
        return new PredictionOutput() { action = act_out, target = target_loc, };
    }

    static void addAction(PredictionInput[] inputs, PredictionOutput output)
    {
        string line = "";
        foreach (PredictionInput input in inputs)
        {
            //unit id 0-8
            for (int i = 0; i < 9; i++)
            {
                if (input.unit_id[i] == 1)
                {
                    line += i.ToString();
                }
            }
            //unit pos 0-3
            line += (Mathf.RoundToInt(input.unit_x * 4)).ToString();
            //unit health
            line += (char)(input.health * 250);
        }
        //outputs
        line += char.MaxValue;
        line += output.action.ToString();
        line += (char)output.target;

        StreamWriter file_writer = new StreamWriter("Assets\\Prediction\\ActionPrediction.txt", true);
        file_writer.Write(line);
        file_writer.Close();
    }
}
