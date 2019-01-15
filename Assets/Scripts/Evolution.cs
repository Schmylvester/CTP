using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{
    [SerializeField] TrackFights tracker = null;
    List<Unit> all_fighters;
    List<Unit> winners;

    public void init(List<Unit> all)
    {
        all_fighters = all;
        winners = new List<Unit>();
    }

    public void addWinner(Unit fighter)
    {
        winners.Add(fighter);
    }

    public void newBatch()
    {
        all_fighters.Clear();
        //add winners
        foreach (Unit fighter in winners)
        {
            all_fighters.Add(fighter);
        }

        //breeding
        foreach (Unit fighter in winners)
        {
            Unit parent_one = winners[Random.Range(0, winners.Count)];
            Unit parent_two = winners[Random.Range(0, winners.Count)];

            int[] child_stats = new int[(int)Stat.Count];
            for(int i = 0; i < (int)Stat.Count; i++)
            {
                child_stats[i] = Random.Range(0, 2) == 0 ? parent_one.getStat((Stat)i) : parent_two.getStat((Stat)i);
            }
            
            all_fighters.Add(new Unit(child_stats));
        }
        
        for (int i = 0; i < all_fighters.Count; i++)
        {
            all_fighters[i].mutate();
        }

        winners.Clear();
        tracker.setUpFights();
    }

    public List<Unit> getFighters()
    {
        return all_fighters;
    }
}
