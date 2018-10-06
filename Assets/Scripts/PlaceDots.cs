using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    public string id;
    public Color colour;
}

public class PlaceDots : MonoBehaviour
{
    [SerializeField] GameObject dot_pref;

    public void draw(List<List<float>> all_values)
    {
        Vector2 scale = getScale(all_values);

        Line[] lines = new Line[3];
        lines[0].id = "Attack";
        lines[0].colour = Color.red;
        lines[1].id = "Health";
        lines[1].colour = Color.green;
        lines[2].id = "Clumsiness";
        lines[2].colour = Color.yellow;

        for (int i = 0; i < all_values.Count; i++)
        {
            placeDots(all_values[i], lines[i], scale);
        }
    }

    /// <summary>
    /// set the values of the dot points for the line graph
    /// </summary>
    /// <param name="values">values on the line</param>
    /// <param name="colour">colour of the line</param>
    /// <param name="y_scl">scale to keep higher values on the screen</param>
    public void placeDots(List<float> values, Line line_id, Vector2 scl)
    {
        int generation = 0;
        for (int i = 0; i < values.Count; i += values.Count / 50)
        {
            generation++;
            GameObject dot = Instantiate(dot_pref);
            float x = (i * scl.x) / (values.Count / 50);
            float y = (values[i] * scl.y);
            dot.transform.position = transform.position + new Vector3(x, y);
            DotStats stats = dot.GetComponent<DotStats>();
            stats.setValues(new Vector2(generation, values[i]), line_id.id);
            dot.GetComponent<SpriteRenderer>().color = line_id.colour;
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
        
        return new Vector2(20.0f / 50, y);
    }
}
