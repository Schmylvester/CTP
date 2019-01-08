using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugField : MonoBehaviour
{

    [SerializeField] Field field;
    private void Awake()
    {
        Unit[] team = new Unit[5] { new Bard(), new Ranger(), new Rogue(), new Knight(), new Necromancer() };
        char[] team_chars = new char[5] { (char)0, (char)6, (char)7, (char)2, (char)3 };
        field.setTeam(0, team, team_chars);
        team = new Unit[5] { new Paladin(), new Cleric(), new Necromancer(), new Vampire(), new Pirate() };
        team_chars = new char[5] { (char)4, (char)1, (char)3, (char)8, (char)5 };
        field.setTeam(1, team, team_chars);
    }
}
