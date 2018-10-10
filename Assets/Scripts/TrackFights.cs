using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrackFights : MonoBehaviour
{
    [SerializeField] Evolution evolution = null;
    [SerializeField] Fight fight = null;
    [SerializeField] PlaceDots dots = null;
    int fights = 0;

    List<float> average_attacks;
    List<float> average_healths;
    List<float> average_clumsiness;

    [SerializeField] bool start_from_last_avg = false;
    bool init = false;

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
            evolution.newBatch();
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
        if (init)
        {
            return evolution.getFighters();
        }
        List<Fighter> fighters = new List<Fighter>();
        for (int i = 0; i < 100; i++)
        {
            int a, h, c;
            if (start_from_last_avg)
            {
                a = GetFromFile("Attack");
                h = GetFromFile("Health");
                c = GetFromFile("Clumsiness");
            }
            else
            {
                a = 15; h = 15; c = 15;
            }
            Fighter fighter = new Fighter(h, a, c);
            fighters.Add(fighter);
        }
        evolution.init(fighters);
        init = true;
        return fighters;
    }

    int GetFromFile(string file_name)
    {
        StreamReader file_read = new StreamReader("Assets\\StatFiles\\" + file_name + ".txt");
        int ret = 0;
        while (!file_read.EndOfStream)
        {
            ret = int.Parse(file_read.ReadLine());
        }
        file_read.Close();
        return ret;
    }

    void getAverage(List<Fighter> fighters)
    {
        int atk = 0, mhp = 0, clm = 0;

        foreach (Fighter f in fighters)
        {
            atk += f.getAttack();
            mhp += f.getMaxHealth();
            clm += f.getClumsiness();
        }

        atk /= fighters.Count;
        mhp /= fighters.Count;
        clm /= fighters.Count;

        average_attacks.Add(atk);
        average_healths.Add(mhp);
        average_clumsiness.Add(clm);
    }

    void writeToFile(string file_name, List<float> vals)
    {
        StreamWriter file_write = new StreamWriter("Assets\\StatFiles\\" + file_name + ".txt");
        for (int i = 0; i < vals.Count; i += vals.Count / 50)
        {
            file_write.WriteLine(vals[i].ToString());
        }
        file_write.Close();
    }
}