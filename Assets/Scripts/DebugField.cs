using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugField : MonoBehaviour
{

    [SerializeField] Field field;
    private void Awake()
    {
        Unit[] team = new Unit[5] { new Bard(), new Ranger(), new Rogue(), new Knight(), new Necromancer() };
        field.setTeam(0, team);
        team = new Unit[5] { new Paladin(), new Cleric(), new Necromancer(), new Vampire(), new Pirate() };
        field.setTeam(1, team);
    }
}
