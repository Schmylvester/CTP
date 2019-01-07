using Abilities;

public class Cleric : Unit
{
    public override string getName()
    {
        return "Cleric";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Heal());
        abilities.Add(new Protect());
        abilities.Add(new Revive());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 50;
        base_stats[(int)Stat.Agility] = 50;
        base_stats[(int)Stat.Attack] = 50;
        base_stats[(int)Stat.Defence] = 80;
        base_stats[(int)Stat.Intelligence] = 90;
        base_stats[(int)Stat.Max_HP] = 90;
        base_stats[(int)Stat.Speed] = 80;

        validateStats(490);
    }
}