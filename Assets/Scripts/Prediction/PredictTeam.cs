using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct TeamPredictionData
{
    public char[] counts;
    public float dist;
}

public class PredictTeam : MonoBehaviour
{
    [SerializeField] short k;
    List<TeamPredictionData> counts = new List<TeamPredictionData>();

    private void Awake()
    {
        StreamReader file_reader = new StreamReader("Assets\\Prediction\\TeamPrediction.txt");
        while (!file_reader.EndOfStream)
            counts.Add(getCount(ushort.Parse(file_reader.ReadLine())));
        file_reader.Close();
    }

    TeamPredictionData getCount(ushort team)
    {
        char[] return_chars = new char[9]
        {
            (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0
        };
        for (int i = 0; i < 5; i++)
        {
            return_chars[(team / (int)Mathf.Pow(9, i)) % 9]++;
        }

        TeamPredictionData prediction_data = new TeamPredictionData();
        prediction_data.counts = return_chars;
        prediction_data.dist = float.MaxValue;
        return prediction_data;
    }

    public void attemptPrediction(Unit[] team_so_far)
    {
        //count the units of the team so far
        char[] team_counts = new char[9];
        for (int i = 0; i < team_so_far.Length; i++)
        {
            team_counts[UnitIDs.getID(team_so_far[i])]++;
        }

        //compare that to the data gathered
        for (int i = 0; i < counts.Count; i++)
        {
            float dist = Mathf.Abs(team_counts[0] - counts[i].counts[0]);
            for (int unit = 1; unit < 9; unit++)
            {
                dist += Mathf.Abs(team_counts[unit] - counts[i].counts[unit]);
                dist = Mathf.Sqrt(dist);
            }
            counts[i] = new TeamPredictionData() { counts = counts[i].counts, dist = dist };
        }

        //get best k units
        char[][] best_candidates = new char[k][];
        for (int i = 0; i < k; i++)
        {
            int best_idx = 0;
            for (int candidate = 0; candidate < counts.Count; candidate++)
            {
                if (counts[candidate].dist < counts[best_idx].dist)
                {
                    best_idx = candidate;
                }
            }
            best_candidates[i] = counts[best_idx].counts;
            counts[best_idx] = new TeamPredictionData() { counts = counts[best_idx].counts, dist = float.MaxValue };
        }

        //subtract what is in this team from the counts on the candidates, then add the candidates together
        char[] final_count = new char[9]
        {
            (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0
        };

        for (int i = 0; i < k; i++)
        {
            for (int unit = 0; unit < 9; unit++)
            {
                best_candidates[i][unit] -= team_counts[unit];
                final_count[unit] += best_candidates[i][unit];
            }
        }

        //get the higest count from the best candidates
        int best = 0;
        for(int i = 1; i < 9; i++)
        {
            if(final_count[i] > final_count[best])
            {
                best = i;
            }
            else if(final_count[i] == final_count[best])
            {
                best = Random.Range(0, 2) == 0 ? i : best;
            }
        }

        Debug.Log("Prediction: " + UnitIDs.getUnit((ushort)best));
    }
}
