using Abilities;

public class Pirate : Unit
{
    public override string getName()
    {
        return "Pirate";
    }

    protected override void setAbilities()
    {
        abilities.Add(new Bomb());
        abilities.Add(new RecklessAbandon());
        abilities.Add(new DownWithShip());
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 6;
        base_stats[(int)Stat.Agility] = 9;
        base_stats[(int)Stat.Attack] = 9;
        base_stats[(int)Stat.Defence] = 5;
        base_stats[(int)Stat.Intelligence] = 4;
        base_stats[(int)Stat.Max_HP] = 8;
        base_stats[(int)Stat.Speed] = 8;
    }
}