using Abilities;

public class Ranger : Unit
{
    public override string getName()
    {
        return "Ranger";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Snipe());
        abilities.Add(new RapidFire());
        abilities.Add(new Ambush());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 9;
        base_stats[(int)Stat.Agility] = 5;
        base_stats[(int)Stat.Attack] = 9;
        base_stats[(int)Stat.Defence] = 7;
        base_stats[(int)Stat.Intelligence] = 6;
        base_stats[(int)Stat.Max_HP] = 5;
        base_stats[(int)Stat.Speed] = 8;
    }
}