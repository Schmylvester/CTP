using Abilities;

public class Knight : Unit
{
    public override string getName()
    {
        return "Knight";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Berserk());
        abilities.Add(new Defend());
        abilities.Add(new Protect());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 7;
        base_stats[(int)Stat.Agility] = 6;
        base_stats[(int)Stat.Attack] = 8;
        base_stats[(int)Stat.Defence] = 8;
        base_stats[(int)Stat.Intelligence] = 6;
        base_stats[(int)Stat.Max_HP] = 8;
        base_stats[(int)Stat.Speed] = 6;
    }
}