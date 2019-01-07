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
        base_stats[(int)Stat.Accuracy] = 50;
        base_stats[(int)Stat.Agility] = 20;
        base_stats[(int)Stat.Attack] = 50;
        base_stats[(int)Stat.Defence] = 50;
        base_stats[(int)Stat.Intelligence] = 0;
        base_stats[(int)Stat.Max_HP] = -50;
        base_stats[(int)Stat.Speed] = 70;

        validateStats(190);
    }
}
