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
        base_stats[(int)Stat.Accuracy] = 70;
        base_stats[(int)Stat.Agility] = 40;
        base_stats[(int)Stat.Attack] = 80;
        base_stats[(int)Stat.Defence] = 90;
        base_stats[(int)Stat.Intelligence] = 80;
        base_stats[(int)Stat.Max_HP] = 90;
        base_stats[(int)Stat.Speed] = 40;

        validateStats(490);
    }
}