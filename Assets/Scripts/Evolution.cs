using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{
    [SerializeField] TrackFights tracker = null;
    List<Fighter> all_fighters;
    List<Fighter> winners;

    public void init(List<Fighter> all)
    {
        all_fighters = all;
        winners = new List<Fighter>();
    }

    public void addWinner(Fighter fighter)
    {
        winners.Add(fighter);
    }

    public void newBatch()
    {
        all_fighters.Clear();
        //add winners
        foreach (Fighter fighter in winners)
        {
            all_fighters.Add(fighter);
        }

        //breeding
        foreach (Fighter fighter in winners)
        {
            Fighter parent_one = winners[Random.Range(0, winners.Count)];
            Fighter parent_two = winners[Random.Range(0, winners.Count)];

            int[] child_stats = new int[(int)Stat.Count];
            for(int i = 0; i < (int)Stat.Count; i++)
            {
                child_stats[i] = Random.Range(0, 2) == 0 ? parent_one.getStat((Stat)i) : parent_two.getStat((Stat)i);
            }
            
            all_fighters.Add(new Fighter(child_stats));
        }
        
        for (int i = 0; i < all_fighters.Count; i++)
        {
            all_fighters[i].mutate();
            all_fighters[i].heal();
        }

        winners.Clear();
        tracker.setUpFights();
    }

    public List<Fighter> getFighters()
    {
        return all_fighters;
    }
}
