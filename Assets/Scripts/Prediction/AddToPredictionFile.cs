using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AddTeamToFile : MonoBehaviour
{
    public static void addTeam(Unit[] units)
    {
        ushort team = 0;
        for(int i = 0; i < 5; i++)
            team += (ushort)(UnitIDs.getID(units[i]) * Mathf.Pow(9, i));
        StreamWriter file_writer = new StreamWriter("Assets\\Prediction\\TeamPrediction.txt", true);
        file_writer.WriteLine(team);
        file_writer.Close();
    }

    public static void addAction(PredictionInput[] inputs, PredictionOutput output)
    {
        //add all the inputs
        //add the output
    }
}
