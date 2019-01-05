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
        base_stats[(int)Stat.Accuracy] = 5;
        base_stats[(int)Stat.Agility] = 5;
        base_stats[(int)Stat.Attack] = 5;
        base_stats[(int)Stat.Defence] = 8;
        base_stats[(int)Stat.Intelligence] = 9;
        base_stats[(int)Stat.Max_HP] = 9;
        base_stats[(int)Stat.Speed] = 8;
    }
}