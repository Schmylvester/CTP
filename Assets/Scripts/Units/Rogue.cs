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
        base_stats[(int)Stat.Accuracy] = 9;
        base_stats[(int)Stat.Agility] = 9;
        base_stats[(int)Stat.Attack] = 5;
        base_stats[(int)Stat.Defence] = 5;
        base_stats[(int)Stat.Intelligence] = 6;
        base_stats[(int)Stat.Max_HP] = 6;
        base_stats[(int)Stat.Speed] = 9;
    }
}