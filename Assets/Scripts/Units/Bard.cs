using Abilities;

public class Bard : Unit
{
    public override string getName()
    {
        return "Bard";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Perform());
        abilities.Add(new DedicatedPerform());
        abilities.Add(new SwanSong());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 60;
        base_stats[(int)Stat.Agility] = 100;
        base_stats[(int)Stat.Attack] = 40;
        base_stats[(int)Stat.Defence] = 40;
        base_stats[(int)Stat.Intelligence] = 100;
        base_stats[(int)Stat.Max_HP] = 90;
        base_stats[(int)Stat.Speed] = 60;

        validateStats(490);
    }
}