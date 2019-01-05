using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    Team[] teams;
    public int ready_count = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        teams = new Team[2] { new Team(), new Team() };
        teams[0].player_id = 0;
        teams[1].player_id = 1;
    }
    public Team getTeam(int t)
    {
        return teams[t];
    }
    public Team[] getTeams()
    {
        return teams;
    }
    public void setTeam(int team_id, Unit[] units)
    {
        teams[team_id].clearUnits();
        foreach (Unit u in units)
        {
            teams[team_id].addUnit(u);
        }

        if(ready_count == 2)
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    public Unit findUnitAtPos(short x, short y)
    {
        foreach (Team t in teams)
            foreach (Unit u in t.getUnits(true))
                if (u.grid_pos.x == x & u.grid_pos.y == y)
                    return u;

        return null;
    }
}
