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

    List<float>[] averages;

    bool init = false;

    private void Start()
    {
        averages = new List<float>[(int)Stat.Count];
        for (int i = 0; i < (int)Stat.Count; i++)
        {
            averages[i] = new List<float>();
        }
        setUpFights();
    }

    public void setUpFights()
    {
        NumGenerations n = new NumGenerations();
        if (fights < n.getGenerations())
        {
            List<Unit> fighters = initialiseFighters();
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
            for (int i = 0; i < (int)Stat.Count; i++)
            {
                writeToFile(((Stat)i).ToString(), averages[i]);
            }

            dots.draw(averages);
        }
    }

    List<Unit> initialiseFighters()
    {
        if (init)
        {
            return evolution.getFighters();
        }
        List<Unit> fighters = new List<Unit>();
        for (int i = 0; i < 128; i++)
        {
            int[] stats = new int[(int)Stat.Count];
            for (int j = 0; j < (int)Stat.Count; j++)
            {
                stats[j] = 15;
            }
            Unit fighter = new Unit(stats);
            fighters.Add(fighter);
        }
        evolution.init(fighters);
        init = true;
        return fighters;
    }

    void getAverage(List<Unit> fighters)
    {
        int[] stats = new int[(int)Stat.Count];
        for (int i = 0; i < (int)Stat.Count; i++)
        {
            foreach (Unit f in fighters)
            {
                stats[i] += f.getStat((Stat)i);
            }
            stats[i] /= fighters.Count;

            averages[i].Add(stats[i]);
        }
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