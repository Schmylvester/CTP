using Abilities;

public class Vampire : Unit
{
    public override string getName()
    {
        return "Vampire";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Drain());
        abilities.Add(new Enthrall());
        abilities.Add(new Charm());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 5;
        base_stats[(int)Stat.Agility] = 9;
        base_stats[(int)Stat.Attack] = 7;
        base_stats[(int)Stat.Defence] = 4;
        base_stats[(int)Stat.Intelligence] = 8;
        base_stats[(int)Stat.Max_HP] = 7;
        base_stats[(int)Stat.Speed] = 9;
    }
}
