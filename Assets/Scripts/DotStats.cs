using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotStats : MonoBehaviour
{
    Vector2 values;
    string m_name;
    Text t;
    Image i;

    private void Start()
    {
        t = GameObject.Find("Values").GetComponent<Text>();
        i = t.GetComponentInChildren<Image>();
    }

    private void OnMouseEnter()
    {
        NumGenerations n = new NumGenerations();
        t.text = m_name + "\nGeneration: " + values.x + "\nValue: " + values.y;
        i.color = GetComponent<SpriteRenderer>().color;
    }

    public void setValues(Vector2 vals, string value_name)
    {
        values = vals;
        m_name = value_name;
    }
}
