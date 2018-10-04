using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlaceDots : MonoBehaviour
{
    [SerializeField] GameObject dot_pref;
    StreamReader file_read;

    private void Start()
    {
        List<float> attack_values = getValues("Attack");
        List<float> health_values = getValues("Health");

        List<List<float>> all_values = new List<List<float>>
        {
            attack_values, health_values
        };
        Vector2 scale = getScale(all_values);

        placeDots(attack_values, Color.red, scale);
        placeDots(health_values, Color.green, scale);
    }


    /// <summary>
    /// gets all the values from a file
    /// </summary>
    /// <param name="file_name">name of file</param>
    /// <returns>list of floats with all values in the file</returns>
    List<float> getValues(string file_name)
    {
        file_read = new StreamReader("Assets\\StatFiles\\" + file_name + ".txt");
        List<float> values = new List<float>();
        while (!file_read.EndOfStream)
        {
            values.Add(float.Parse(file_read.ReadLine()));
        }
        return values;
    }


    /// <summary>
    /// set the values of the dot points for the line graph
    /// </summary>
    /// <param name="values">values on the line</param>
    /// <param name="colour">colour of the line</param>
    /// <param name="y_scl">scale to keep higher values on the screen</param>
    public void placeDots(List<float> values, Color colour, Vector2 scl)
    {
        Vector3 last = transform.position;
        for (int i = 0; i < values.Count; i++)
        {
            GameObject dot = Instantiate(dot_pref);
            dot.transform.position = transform.position + new Vector3(i * scl.x, values[i] * scl.y);
            DotStats stats = dot.GetComponent<DotStats>();
            stats.setValues(new Vector2(i, values[i]));
            dot.GetComponent<SpriteRenderer>().color = colour;
        }
    }

    /// <summary>
    /// To prevent the y-values from leaving the screen, scale them to the max value
    /// Get max value
    /// </summary>
    /// <param name="all_values">All values</param>
    /// <returns>The amount that all values need to be scaled by to fit the highest values into the screen</returns>
    Vector2 getScale(List<List<float>> all_values)
    {
        float max = float.MinValue;
        foreach (List<float> values in all_values)
        {
            foreach (float value in values)
            {
                max = value > max ? value : max;
            }
        }
        float y;
        if (max == 0)
        {
            y = 1;
        }
        else
        {
            y = 8 / max;
        }

        float x;
        if (all_values.Count != 0)
            x = (20.0f / all_values[0].Count);
        else
            x = 1;

        return new Vector2(x, y);
    }
}
