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
        base_stats[(int)Stat.Accuracy] = 5;
        base_stats[(int)Stat.Agility] = 8;
        base_stats[(int)Stat.Attack] = 6;
        base_stats[(int)Stat.Defence] = 5;
        base_stats[(int)Stat.Intelligence] = 10;
        base_stats[(int)Stat.Max_HP] = 9;
        base_stats[(int)Stat.Speed] = 6;
    }
}
