using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHealth : MonoBehaviour
{
    int known_health = 0;
    [SerializeField] short unit_id;
    Unit unit;
    UnityEngine.UI.Text text;

    private void Start()
    {
        text = GetComponent<UnityEngine.UI.Text>();
        short team = (short)(unit_id < 5 ? 0 : 1);
        unit = FindObjectOfType<Field>().getTeam(team).getUnits(false)[unit_id % 5];
    }

    void Update ()
    {
        if(unit.getHealth() != known_health)
        {
            int hp = unit.getHealth();
            int max = unit.getStat(Stat.Max_HP);
            text.text = hp + "\n-\n" + max;
            known_health = hp;
            text.color = Color.Lerp(Color.red, new Color(0, 0.6f, 0), (float)hp / max);
        }
	}
}
