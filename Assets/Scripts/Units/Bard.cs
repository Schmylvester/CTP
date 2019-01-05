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
        base_stats[(int)Stat.Accuracy] = 6;
        base_stats[(int)Stat.Agility] = 10;
        base_stats[(int)Stat.Attack] = 4;
        base_stats[(int)Stat.Defence] = 4;
        base_stats[(int)Stat.Intelligence] = 10;
        base_stats[(int)Stat.Max_HP] = 9;
        base_stats[(int)Stat.Speed] = 6;
    }
}