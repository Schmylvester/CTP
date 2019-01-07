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
        base_stats[(int)Stat.Accuracy] = 90;
        base_stats[(int)Stat.Agility] = 50;
        base_stats[(int)Stat.Attack] = 90;
        base_stats[(int)Stat.Defence] = 70;
        base_stats[(int)Stat.Intelligence] = 60;
        base_stats[(int)Stat.Max_HP] = 50;
        base_stats[(int)Stat.Speed] = 80;

        validateStats(490);
    }
}