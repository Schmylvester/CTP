using Abilities;

public class Vampire : Unit
{
    public override string getName()
    {
        return "Vampire";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Drain());
        abilities.Add(new Enthrall());
        abilities.Add(new Charm());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 50;
        base_stats[(int)Stat.Agility] = 90;
        base_stats[(int)Stat.Attack] = 70;
        base_stats[(int)Stat.Defence] = 40;
        base_stats[(int)Stat.Intelligence] = 80;
        base_stats[(int)Stat.Max_HP] = 70;
        base_stats[(int)Stat.Speed] = 90;

        validateStats(490);
    }
}
