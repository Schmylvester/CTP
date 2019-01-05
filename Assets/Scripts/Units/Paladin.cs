using Abilities;

public class Paladin : Unit
{
    public override string getName()
    {
        return "Paladin";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Avenge());
        abilities.Add(new BodyBlock());
        abilities.Add(new Heal());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 7;
        base_stats[(int)Stat.Agility] = 4;
        base_stats[(int)Stat.Attack] = 8;
        base_stats[(int)Stat.Defence] = 9;
        base_stats[(int)Stat.Intelligence] = 8;
        base_stats[(int)Stat.Max_HP] = 9;
        base_stats[(int)Stat.Speed] = 4;
    }
}