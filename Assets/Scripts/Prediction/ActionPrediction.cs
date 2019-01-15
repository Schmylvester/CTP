using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using System.IO;

public struct PredictionInput   //per unit
{
    public short[] unit_id;
    public float unit_x;
    public float health;
}

public struct PredictionOutput
{
    public ushort action;
    public ushort target;
}

public struct Prediction
{
    public PredictionInput[] input;
    public PredictionOutput output;
    public float dist;
    public void setDist(float d) { dist = d; }
}

public class ActionPrediction : MonoBehaviour
{
    [SerializeField] ushort kNN = 5;
    Field field;
    List<Prediction> all_prev_actions = new List<Prediction>();

    private void Start()
    {
        field = FindObjectOfType<Field>();
        StartCoroutine(loadPreviousActions());
    }

    /// <summary>
    /// Opens the file and gets all the data ready
    /// </summary>
    /// <returns>nuttin</returns>
    IEnumerator loadPreviousActions()
    {
        StreamReader file_reader = new StreamReader("Assets\\Prediction\\ActionPrediction.txt");
        int count_break = 0;
        while (!file_reader.EndOfStream)
        {
            List<PredictionInput> input = new List<PredictionInput>();
            char[] char_out = new char[1];
            short short_out;
            while (short.TryParse(((char)file_reader.Peek()).ToString(), out short_out))
            {
                if (count_break++ == int.MaxValue)
                {
                    Debug.LogError("There was an error with the while loops");
                    Application.Quit();
                    yield return null;
                }
                PredictionInput one_in = new PredictionInput() { unit_id = new short[9] };
                file_reader.Read(char_out, 0, 1);
                short unit = short.Parse(char_out[0].ToString());
                for (short i = 0; i < Globals.unit_count; i++)
                {
                    if (i == unit)
                    {
                        one_in.unit_id[i] = 1;
                    }
                    else
                    {
                        one_in.unit_id[i] = 0;
                    }
                }
                file_reader.Read(char_out, 0, 1);
                one_in.unit_x = (short)char_out[0];
                file_reader.Read(char_out, 0, 1);
                one_in.health = (float)(char_out[0]) / 250;

                input.Add(one_in);
            }
            file_reader.Read(char_out, 0, 1);
            file_reader.Read(char_out, 0, 1);
            ushort act = ushort.Parse(char_out[0].ToString());
            file_reader.Read(char_out, 0, 1);
            ushort target = char_out[0];
            PredictionOutput output = new PredictionOutput() { action = act, target = target };
            Prediction previous_behaviour = new Prediction()
            {
                input = input.ToArray(),
                output = output,
                dist = float.MaxValue
            };
            all_prev_actions.Add(previous_behaviour);
        }
        file_reader.Close();
        yield return null;
    }

    /// <summary>
    /// Based on the current game state, makes a prediction as to the best action
    /// </summary>
    /// <param name="current_state">Units and their data</param>
    /// <param name="active_unit">Currently in play</param>
    /// <returns>What the best ability might be</returns>
    public Ability makePrediction(PredictionInput[] current_state, Unit active_unit)
    {
        countDistances(current_state);

        PredictionOutput[] outputs = new PredictionOutput[kNN];
        for (int best = 0; best < kNN; best++)
        {
            int best_idx = 0;
            for (int idx = 1; idx < all_prev_actions.Count; idx++)
            {
                if (all_prev_actions[best_idx].dist < all_prev_actions[idx].dist)
                {
                    best_idx = idx;
                }
            }
            outputs[best] = all_prev_actions[best_idx].output;
            all_prev_actions[best_idx].setDist(float.MaxValue);
        }

        return getAbility(outputs, active_unit);
    }

    /// <summary>
    /// Gets the distances between all previous inputs and the current one
    /// </summary>
    /// <param name="current_state">The current input state</param>
    void countDistances(PredictionInput[] current_state)
    {
        for (int prev_action = 0; prev_action < all_prev_actions.Count; prev_action++)
        {
            float dist = 0;
            for (int input = 0; input < current_state.Length; input++)
            {
                for (int unit = 0; unit < 9; unit++)
                {
                    dist += Mathf.Abs(
                        current_state[input].unit_id[unit]
                        - all_prev_actions[prev_action].input[input].unit_id[unit]
                        );
                }
            }
            all_prev_actions[prev_action].setDist(dist);
        }
    }

    /// <summary>
    /// Takes the best kNN outputs and makes them into one ability
    /// </summary>
    /// <param name="outputs">The best outputs from the kNN</param>
    /// <param name="active_unit">The unit currently playing</param>
    /// <returns>The best prediction for an action</returns>
    Ability getAbility(PredictionOutput[] outputs, Unit active_unit)
    {
        ushort[] actions = new ushort[outputs.Length];
        ushort[] targets = new ushort[outputs.Length];
        for (int i = 0; i < outputs.Length; i++)
        {
            actions[i] = outputs[i].action;
            targets[i] = outputs[i].target;
        }

        Ability output = null;
        ushort best_action = getBest(actions, 5);
        switch (best_action)
        {
            case 0:
                output = new Attack();
                break;
            case 1:
            case 2:
            case 3:
                output = active_unit.getAbility(best_action - 1);
                break;
            case 4:
                output = new Move();
                break;
        }

        ushort best_target = getBest(targets, 32);
        short team = (short)(best_target < 16 ? active_unit.getTeam().player_id : 1 - active_unit.getTeam().player_id);
        short x = (short)(best_target % 4);
        short y = (short)((best_target - (team * 16)) / 4);

        output.setUser(active_unit);
        output.setTarget(field.findUnitAtPos(x, y, team));

        string best_guess = "Best guess is that " + active_unit.getName() + " will use " + output.ability_name;
        if (!output.noTarget())
        {
            if (output.getTarget() == null)
            {
                for (short i = 0; i < 4; i++)
                {
                    output.setTarget(field.findUnitAtPos(x, i, team));
                    if (output.getTarget() != null)
                        break;
                }
                if(output.getTarget() == null)
                {
                    for (short i = 0; i < 4; i++)
                    {
                        for (short j = 0; j < 4; j++)
                        {
                            output.setTarget(field.findUnitAtPos(i, j, team));
                            if (output.getTarget() != null)
                                break;
                        }
                        if (output.getTarget() != null)
                            break;
                    }
                }
            }

            best_guess += " on " + output.getTarget().getName();
        }
        Debug.Log(best_guess);
        return output;
    }

    /// <summary>
    /// Finds the index of the best value in a list of values
    /// </summary>
    /// <param name="values">The list of values</param>
    /// <param name="max_val">How many different values there are</param>
    /// <returns>Index of the highest value</returns>
    ushort getBest(ushort[] values, short max_val)
    {
        ushort[] counts = new ushort[max_val];
        for (ushort i = 0; i < max_val; i++)
            counts[i] = 0;
        foreach (ushort val in values)
        {
            counts[val]++;
        }
        ushort best = 0;
        for (ushort i = 1; i < max_val; i++)
        {
            if (counts[i] > counts[best])
            {
                best = i;
            }
            else if (counts[i] == counts[best])
            {
                best = Random.Range(0, 2) == 0 ? best : i;
            }
        }
        return best;
    }

    /// <summary>
    /// Makes a unit into the input values needed for each unit
    /// </summary>
    /// <param name="unit">The unit in play</param>
    /// <returns>Their id, position and health as it's needed by the struct</returns>
    public static PredictionInput getPredictionInput(Unit unit)
    {
        PredictionInput ret_val = new PredictionInput
        {
            unit_id = new short[9],
            health = (float)unit.getHealth() / unit.getStat(Stat.Max_HP),
            unit_x = (float)unit.getPos() / 4
        };
        for (int i = 0; i < 9; i++)
        {
            if (UnitIDs.getID(unit) == i)
            {
                ret_val.unit_id[i] = 1;
            }
            else
            {
                ret_val.unit_id[i] = 0;
            }
        }
        return ret_val;
    }
}