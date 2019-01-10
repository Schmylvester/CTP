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
        base_stats[(int)Stat.Accuracy] = 70;
        base_stats[(int)Stat.Agility] = 60;
        base_stats[(int)Stat.Attack] = 80;
        base_stats[(int)Stat.Defence] = 80;
        base_stats[(int)Stat.Intelligence] = 60;
        base_stats[(int)Stat.Max_HP] = 80;
        base_stats[(int)Stat.Speed] = 60;

        validateStats(490);
    }
}