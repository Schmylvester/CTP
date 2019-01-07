using Abilities;

public class Necromancer : Unit
{
    public override string getName()
    {
        return "Necromancer";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Summon());
        abilities.Add(new SoulHarvest());
        abilities.Add(new CheatDeath());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 50;
        base_stats[(int)Stat.Agility] = 80;
        base_stats[(int)Stat.Attack] = 60;
        base_stats[(int)Stat.Defence] = 50;
        base_stats[(int)Stat.Intelligence] = 100;
        base_stats[(int)Stat.Max_HP] = 90;
        base_stats[(int)Stat.Speed] = 60;

        validateStats(490);

    }
}
