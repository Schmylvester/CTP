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
                attack = Random.Range(0, 2) == 0 ? parent_one.attack : parent_two.attack,
                max_health = Random.Range(0, 2) == 0 ? parent_one.max_health : parent_two.max_health,
                clumsiness = Random.Range(0, 2) == 0 ? parent_one.clumsiness : parent_two.clumsiness,
            };
            all_fighters.Add(child);
        }

        int break_trap = 0;
        for (int i = 0; i < all_fighters.Count && break_trap++ < 1000; i++)
        {
            bool success = false;
            while (!success)
            {
                //mutation
                switch (Random.Range(0, 6))
                {
                    case 0:
                        success = changeStat(ref all_fighters[i].attack, ref all_fighters[i].max_health, 1);
                        break;
                    case 1:
                        success = changeStat(ref all_fighters[i].attack, ref all_fighters[i].clumsiness, 1);
                        break;
                    case 2:
                        success = changeStat(ref all_fighters[i].max_health, ref all_fighters[i].attack, 1);
                        break;
                    case 3:
                        success = changeStat(ref all_fighters[i].max_health, ref all_fighters[i].clumsiness, 1);
                        break;
                    case 4:
                        success = changeStat(ref all_fighters[i].clumsiness, ref all_fighters[i].attack, 1);
                        break;
                    case 5:
                        success = changeStat(ref all_fighters[i].clumsiness, ref all_fighters[i].max_health, 1);
                        break;
                    default:
                        break;
                }
            }
            if (break_trap > 500)
            {
                Debug.LogError("Attack: " + all_fighters[i].attack +
                    " Max Health: " + all_fighters[i].max_health +
                    " Clumsiness: " + all_fighters[i].clumsiness);
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

    bool changeStat(ref int increase, ref int decrease, int by)
    {
        if (decrease > by)
        {
            increase += by;
            decrease -= by;
            return true;
        }
        return false;
    }
}
