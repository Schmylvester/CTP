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
        base_stats[(int)Stat.Accuracy] = 60;
        base_stats[(int)Stat.Agility] = 90;
        base_stats[(int)Stat.Attack] = 90;
        base_stats[(int)Stat.Defence] = 50;
        base_stats[(int)Stat.Intelligence] = 40;
        base_stats[(int)Stat.Max_HP] = 80;
        base_stats[(int)Stat.Speed] = 80;

        validateStats(490);
    }
}