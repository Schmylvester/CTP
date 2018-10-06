using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotStats : MonoBehaviour
{
    Vector2 values;
    string m_name;
    Text t;

    private void Start()
    {
        t = GameObject.Find("Values").GetComponent<Text>();
    }

    private void OnMouseEnter()
    {
        NumGenerations n = new NumGenerations();
        t.text = m_name + "\nGeneration: " + values.x + "\nValue: " + values.y;
    }

    public void setValues(Vector2 vals, string value_name)
    {
        values = vals;
        m_name = value_name;
    }
}
