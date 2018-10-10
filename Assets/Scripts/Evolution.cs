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
        for (int i = 0; i < winners.Count; i++)
        {
            Fighter parent_one = winners[Random.Range(0, winners.Count)];
            Fighter parent_two = winners[Random.Range(0, winners.Count)];

            Fighter child = new Fighter
                (Random.Range(0, 2) == 0 ? parent_one.getAttack() : parent_two.getAttack(),
                Random.Range(0, 2) == 0 ? parent_one.getMaxHealth() : parent_two.getMaxHealth(),
                Random.Range(0, 2) == 0 ? parent_one.getClumsiness() : parent_two.getClumsiness());
            all_fighters.Add(child);
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
