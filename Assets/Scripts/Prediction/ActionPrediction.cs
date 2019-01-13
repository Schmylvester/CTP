using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;

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
    [SerializeField] Field field;
    [SerializeField] ushort kNN = 5;
    List<Prediction> all_prev_actions = new List<Prediction>();

    Ability makePrediction(PredictionInput[] current_state, Unit active_unit)
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

    Ability getAbility(PredictionOutput[] outputs, Unit active_unit)
    {
        ushort[] actions = new ushort[outputs.Length];
        ushort[] targets = new ushort[outputs.Length];
        for(int i = 0; i < outputs.Length; i++)
        {
            actions[i] = outputs[i].action;
            targets[i] = outputs[i].target;
        }

        Ability output = null;
        ushort best_action = getBest(actions, 5);
        switch(best_action)
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
        short team = (short)(best_target < 16 ? 0 : 1);
        short x = (short)(best_target % 4);
        short y = (short)((best_target - (team * 16)) / 4);

        output.setUser(active_unit);
        output.setTarget(field.findUnitAtPos(x, y, team));

        if(output.getTarget() == null &! output.noTarget())
        {
            Debug.LogError("There was an error with target");
        }

        string best_guess = "Best guess is that " + active_unit.getName() + " will use " + output.ability_name;
        if (!output.noTarget())
        {
            best_guess +=  " on " + output.getTarget().getName();
        }
        Debug.Log(best_guess);
        return output;
    }

    ushort getBest(ushort[] values, short max_val)
    {
        ushort[] counts = new ushort[max_val];
        for (ushort i = 0; i < max_val; i++)
            counts[i] = 0;
        foreach (ushort val in values)
        {
            for (int i = 0; i < max_val; i++)
            {
                if (val == i)
                {
                    counts[i]++;
                    break;
                }
            }
        }
        ushort best = 0;
        for(ushort i = 1; i < max_val; i++)
        {
            if(counts[i] > counts[best])
            {
                best = i;
            }
            else if(counts[i] == counts[best])
            {
                best = Random.Range(0, 2) == 0 ? best : i;
            }
        }

        return best;
    }

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