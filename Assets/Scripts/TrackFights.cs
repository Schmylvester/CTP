using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrackFights : MonoBehaviour
{
    [SerializeField] Evolution evolution;
    [SerializeField] Fight fight;
    [SerializeField] PlaceDots dots;
    int fights = 0;
    StreamWriter file_write;

    List<float> average_attacks;
    List<float> average_healths;
    List<float> average_clumsiness;

    private void Start()
    {
        average_attacks = new List<float>();
        average_healths = new List<float>();
        average_clumsiness = new List<float>();
        setUpFights();
    }

    public void setUpFights()
    {
        NumGenerations n = new NumGenerations();
        if (fights < n.getGenerations())
        {
            List<Fighter> fighters = initialiseFighters();
            for (int i = 0; i < fighters.Count; i += 2)
            {
                fight.fight(fighters[i], fighters[i + 1]);
            }
            fights++;
            getAverage(fighters);
            evolution.evolve();
        }
        else
        {
            writeToFile("Attack", average_attacks);
            writeToFile("Health", average_healths);
            writeToFile("Clumsiness", average_clumsiness);

            List<List<float>> dots_to_draw =
                new List<List<float>>()
                {
                    average_attacks,
                    average_healths,
                    average_clumsiness,
                };
            dots.draw(dots_to_draw);
        }
    }

    List<Fighter> initialiseFighters()
    {
        if (evolution.initialised)
        {
            return evolution.getFighters();
        }
        List<Fighter> fighters = new List<Fighter>();
        for (int i = 0; i < 100; i++)
        {
            Fighter fighter = new Fighter
            {
                attack = 5,
                max_health = 50,
                health = 50,
                clumsiness = 150,
            };
            fighters.Add(fighter);
        }
        evolution.init(fighters);
        return fighters;
    }

    void getAverage(List<Fighter> fighters)
    {
        Fighter average = new Fighter()
        {
            max_health = 0,
            attack = 0,
            clumsiness = 0
        };

        foreach (Fighter f in fighters)
        {
            average.attack += f.attack;
            average.max_health += f.max_health;
            average.clumsiness += f.clumsiness;
        }

        average.attack /= fighters.Count;
        average.max_health /= fighters.Count;
        average.clumsiness /= fighters.Count;

        average_attacks.Add(average.attack);
        average_healths.Add(average.max_health);
        average_clumsiness.Add(average.clumsiness);
    }

    void writeToFile(string file_name, List<float> vals)
    {
        file_write = new StreamWriter("Assets\\StatFiles\\" + file_name + ".txt");
        for (int i = 0; i < vals.Count; i += vals.Count / 50)
        {
            file_write.WriteLine(vals[i].ToString());
        }
        file_write.Close();
    }
}