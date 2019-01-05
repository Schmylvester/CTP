public class SummonedSkeleton : Unit
{
    public override string getName()
    {
        return "Skeleton";
    }
    protected override void setAbilities()
    {
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 5;
        base_stats[(int)Stat.Agility] = 2;
        base_stats[(int)Stat.Attack] = 5;
        base_stats[(int)Stat.Defence] = 5;
        base_stats[(int)Stat.Intelligence] = 0;
        base_stats[(int)Stat.Max_HP] = -5;
        base_stats[(int)Stat.Speed] = 7;
    }
}
