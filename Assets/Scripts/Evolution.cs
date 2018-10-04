using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{
    [SerializeField] TrackFights tracker;
    List<Fighter> all_fighters;
    List<Fighter> winners;
    public bool initialised = false;

    public void init(List<Fighter> all)
    {
        all_fighters = all;
        winners = new List<Fighter>();
        initialised = true;
    }

    public void addWinner(Fighter fighter)
    {
        winners.Add(fighter);
    }

    public void evolve()
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
            {
                attack = (parent_one.attack + parent_two.attack) / 2,
                max_health = (parent_one.max_health + parent_two.max_health) / 2
            };
            all_fighters.Add(child);
        }

        for (int i = 0; i < all_fighters.Count; i++)
        {
            //mutation
            switch (Random.Range(0, 2))
            {
                case 0:
                    all_fighters[i].attack++;
                    break;
                case 1:
                    all_fighters[i].max_health++;
                    break;
                default:
                    break;
            }

            //heal everyone
            all_fighters[i].health = all_fighters[i].max_health;
        }

        winners.Clear();
        tracker.setUpFights();
    }

    public List<Fighter> getFighters()
    {
        return all_fighters;
    }
}
