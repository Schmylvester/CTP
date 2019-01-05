using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUnitPopulate : MonoBehaviour
{
    [SerializeField] UnitSprites sprites;
    [SerializeField] Image[] team1;
    [SerializeField] Image[] team2;
    private void Start()
    {
        Field f = FindObjectOfType<Field>();
        Unit[] units = f.getTeam(0).getUnits(false);
        for(int i = 0; i < team1.Length; i++)
        {
            team1[i].sprite = sprites.getSprite(units[i]);
        }

        units = f.getTeam(1).getUnits(false);
        for (int i = 0; i < team2.Length; i++)
        {
            team2[i].sprite = sprites.getSprite(units[i]);
        }
    }   
}
