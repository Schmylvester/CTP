using Abilities;

public class Rogue : Unit
{
    public int sneak_turns = 0;

    public override string getName()
    {
        return "Rogue";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Ambush());
        abilities.Add(new Sneak());
        abilities.Add(new Taunt());
    }
    
    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 90;
        base_stats[(int)Stat.Agility] = 90;
        base_stats[(int)Stat.Attack] = 50;
        base_stats[(int)Stat.Defence] = 50;
        base_stats[(int)Stat.Intelligence] = 60;
        base_stats[(int)Stat.Max_HP] = 60;
        base_stats[(int)Stat.Speed] = 90;

        validateStats(490);
    }
}