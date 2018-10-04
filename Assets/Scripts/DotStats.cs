using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotStats : MonoBehaviour
{
    [SerializeField] Vector2 values;
    Text t;

    private void Start()
    {
        t = GameObject.Find("Values").GetComponent<Text>();
    }

    private void OnMouseEnter()
    {
        NumGenerations n = new NumGenerations();
        t.text = "Generation: " + values.x * n.getGenerations() / 50 + "\nValue: " + values.y;
    }

    public void setValues(Vector2 vals)
    {
        values = vals;
    }
}
